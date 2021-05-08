using Antlr4.Runtime.Misc;
using LL.AST;
using LL.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LL
{
    public class StructDefinitionVisitor : llBaseVisitor<IAST>
    {
        private Queue<string> Directories;
        private List<string> Files;
        private static readonly string FILE_ENDING = "ll";
        private string CurrentFile;
        public ProgramNode RootProgram { get; set; }

        // hide the default constructor
        private StructDefinitionVisitor() : base()
        {

        }

        public StructDefinitionVisitor(string fileName) : this() => this.CurrentFile = fileName;

        public StructDefinitionVisitor(string fileName, ProgramNode rootProgram) : this(fileName) => this.RootProgram = rootProgram;

        public override IAST VisitCompileUnit([NotNull] llParser.CompileUnitContext context)
        {
            return this.Visit(context.program());
        }

        public override IAST VisitProgram([NotNull] llParser.ProgramContext context)
        {
            ProgramNode result = this.RootProgram ?? new ProgramNode(this.CurrentFile, context.Start.Line, context.Start.Column);
            var loadStatements = context.loadStatement();

            // this should be fine for now, but when loading something like header files
            // this could be a problem
            foreach (var loadStatement in loadStatements)
            {
                string progName = loadStatement.WORD().GetText();

                if (result.Dependencies.ContainsKey(progName))
                    continue;
                result.Dependencies.Add(progName, this.Visit(loadStatement) as LoadStatement);
            }

            // TODO start compilation of the dependencies

            var structDefs = context.structDefinition();

            if (structDefs is null || structDefs.Length <= 0)
                return result;

            // casting here is safe, as we are explicitly processing struct definitions
            foreach (var structDef in structDefs)
            {
                StructDefinition def = this.Visit(structDef) as StructDefinition;
                // TODO add recursive search
                bool success = result.StructDefs.TryAdd(def.Name, def);

                if (!success)
                    throw new StructAlreadyDefinedException(def.Name, this.CurrentFile, def.Line, def.Column);
            }

            return result;
        }

        public override IAST VisitLoadStatement([NotNull] llParser.LoadStatementContext context)
        {
            int line = context.Start.Line;
            int column = context.Start.Column;
            if (this.Directories == null)
            {
                this.Directories = new Queue<string>();
                this.Files = new List<string>();

                // get all directories on the current level
                string[] dirs = Directory.GetDirectories(Environment.CurrentDirectory);

                // safe them on the stack, maybe they are needed later on
                foreach (string dir in dirs)
                    this.Directories.Append(dir);

                // add all files in the current directory to the list of files
                this.Files.AddRange(Directory.GetFiles(Environment.CurrentDirectory));
            }

            string fileToFind = context.fileName.Text;

            if (!IsFilePresent(fileToFind, out string location))
                throw new Exceptions.FileNotFoundException(fileToFind, this.CurrentFile, line, column);

            return new LoadStatement(fileToFind, location, line, column);
        }

        public override IAST VisitStructDefinition(llParser.StructDefinitionContext context)
        {
            string name = context.WORD().GetText();
            int line = context.Start.Line;
            int column = context.Start.Column;

            return new StructDefinition(name, line, column);
        }

        // todo pass location
        private bool IsFilePresent(string fileName, out string location)
        {
            location = FindFile(this.Files, fileName);
            if (location is not null)
                return true;

            while (this.Directories.TryDequeue(out string currentDir))
            {
                // get all files in the directory
                List<string> files = new List<string>(Directory.GetFiles(currentDir));
                // save them for later use
                Files.AddRange(files);
                // get the directories in the directory
                string[] dirs = Directory.GetDirectories(currentDir);

                // store them for later use
                foreach (string dir in dirs)
                    Directories.Append(dir);

                // check whether the file is in the list or not
                location = FindFile(files, fileName);
                if (location is not null)
                    return true;
            }

            return false;
        }

        private string FindFile(List<string> files, string fileName)
        {
            return this.Files.Find(filePath =>
            {
                int lastSlash = filePath.LastIndexOf(Path.DirectorySeparatorChar);
                string fName = filePath.Substring(lastSlash + 1);
                string[] parts = fName.Split('.', StringSplitOptions.RemoveEmptyEntries);

                return parts[0] == fileName && parts[1] == FILE_ENDING;
            });
        }
    }
}