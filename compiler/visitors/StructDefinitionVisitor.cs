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

        // hide the default constructor
        private StructDefinitionVisitor(): base()
        {

        }

        public StructDefinitionVisitor(string fileName): this()
        {
            this.CurrentFile = fileName;
        }

        public override IAST VisitLoadStatement([NotNull] llParser.LoadStatementContext context)
        {
            if (Directories == null)
            {
                Directories = new Queue<string>();
                Files = new List<string>();

                // get all directories on the current level
                string[] dirs = Directory.GetDirectories(Environment.CurrentDirectory);

                // safe them on the stack, maybe they are needed later on
                foreach (string dir in dirs)
                    Directories.Append(dir);

                // add all files in the current directory to the list of files
                Files.AddRange(Directory.GetFiles(Environment.CurrentDirectory));
            }

            string fileToFind = context.fileName.Text;

            if(!IsFilePresent(fileToFind))
                throw new Exceptions.FileNotFoundException(fileToFind, CurrentFile, context.Start.Line, context.Start.Column);

            throw new NotImplementedException();
        }

        public override IAST VisitStructDefinition(llParser.StructDefinitionContext context)
        {
            string name = context.WORD().GetText();
            int line = context.Start.Line;
            int column = context.Start.Column;

            if(IAST.Structs.ContainsKey(name))
                throw new StructAlreadyDefinedException(name, this.CurrentFile, line, column);

            IAST.Structs[name] = new StructDefinition(name, line, column);

            // unused value
            return null;
        }

        private bool IsFilePresent(string fileName)
        {
            if(IsFileInList(Files, fileName))
                return true;

            while (Directories.TryDequeue(out string currentDir))
            {
                // get all files in the directory
                List<string> files = new List<string>(Directory.GetFiles(currentDir));
                // save them for later use
                Files.AddRange(files);
                // get the directories in the directory
                string[] dirs = Directory.GetDirectories(currentDir);
                
                // store them for later use
                foreach(string dir in dirs)
                    Directories.Append(dir);
                
                // check whether the file is in the list or not
                if(IsFileInList(files, fileName))
                    return true;
            }

            return false;
        }

        private bool IsFileInList(List<string> files, string fileName)
        {
            return Files.Find(filePath =>
            {
                int lastSlash = filePath.LastIndexOf(Path.DirectorySeparatorChar);
                string fName = filePath.Substring(lastSlash+1);
                string[] parts = fName.Split('.', StringSplitOptions.RemoveEmptyEntries);

                return parts[0] == fileName && parts[1] == FILE_ENDING;
            }) is not null;
        }
    }
}