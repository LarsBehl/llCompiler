using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using LL.AST;
using LL.Exceptions;
using LL.Helper;
using LL.Types;

namespace LL.CodeGeneration
{
    public partial class AssemblerGenerator
    {
        // TODO change properties so that struct definitions declared in other files also get passed to the runtime
        private int Depth = 0;
        private StringBuilder Sb = new StringBuilder();
        private StringBuilder DoubleNumbers = new StringBuilder();
        private StringBuilder Strings = new StringBuilder();
        private StringBuilder StructDefinitionBuilder = new StringBuilder();
        private int LabelCount = 0;
        private int DoubleNumbersLabelCount = 0;
        private int StringLabelCount = 0;
        private string[] IntegerRegisters = { "%rdi", "%rsi", "%rdx", "%rcx", "%r8", "%r9" };
        private string[] DoubleRegisters = { "%xmm0", "%xmm1", "%xmm2", "%xmm3", "%xmm4", "%xmm5", "%xmm6", "%xmm7" };
        private Dictionary<string, FunctionAsm> FunctionMap = new Dictionary<string, FunctionAsm>();
        private Dictionary<double, int> DoubleMap = new Dictionary<double, int>();
        private Dictionary<string, int> VariableMap;
        private Dictionary<string, int> StringLabelMap = new Dictionary<string, int>();
        private Dictionary<string, int> StructIdMap = new Dictionary<string, int>();
        private int LocalVariablePointer = 0;
        private int LocalVariableCount = 0;
        private int StackCounter = 0;
        private bool InnerStruct = false;
        private string CurrentFile;
        private ProgramNode RootProg;

        private static List<string> CompiledPrograms = new List<string>();

        public AssemblerGenerator(string currentFile)
        {
            this.CurrentFile = currentFile;
        }

        public void PrintAssember()
        {
            Console.WriteLine(this.Sb.ToString());
            if (this.DoubleNumbers.Length > 0)
                Console.WriteLine(this.DoubleNumbers.ToString());
            if (this.Strings.Length > 0)
                Console.WriteLine(this.Strings.ToString());
        }

        /// <summary>Get the assembler code of the given AST node</summary>
        private void GetAssember(IAST astNode)
        {
            switch (astNode)
            {
                case DoubleLit doubleLit:
                    this.DoubleLitAsm(doubleLit); break;
                case IntLit intLit:
                    this.IntLitAsm(intLit); break;
                case BoolLit boolLit:
                    this.BoolLitAsm(boolLit); break;
                case AddExpr addExpr:
                    this.AddExprAsm(addExpr); break;
                case SubExpr subExpr:
                    this.SubExprAsm(subExpr); break;
                case MultExpr multExpr:
                    this.MultExprAsm(multExpr); break;
                case DivExpr divExpr:
                    this.DivExprAsm(divExpr); break;
                case GreaterExpr greaterExpr:
                    this.GreaterExprAsm(greaterExpr); break;
                case LessExpr lessExpr:
                    this.LessExprAsm(lessExpr); break;
                case EqualityExpr equalityExpr:
                    this.EqualityExprAsm(equalityExpr); break;
                case BlockStatement blockStatement:
                    this.BlockStatementAsm(blockStatement); break;
                case ReturnStatement returnStatement:
                    this.ReturnStatementAsm(returnStatement); break;
                case FunctionDefinition funDef:
                    this.FunctionDefinitionAsm(funDef); break;
                case FunctionCall funCall:
                    this.FunctionCallAsm(funCall); break;
                case ProgramNode programNode:
                    this.RootProg = programNode;
                    if (CompiledPrograms.Contains(this.RootProg.FileName))
                        throw new CodeAlreadyGeneratedException(this.RootProg.FileName);
                    CompiledPrograms.Add(this.RootProg.FileName);
                    foreach (var structDef in programNode.StructDefs)
                        this.GetAssember(structDef.Value);
                    foreach (var fun in programNode.FunDefs)
                        this.GetAssember(fun.Value);
                    break;
                case VarExpr varExpr:
                    this.VariableAsm(varExpr); break;
                case AssignStatement assignStatement:
                    this.AssignAsm(assignStatement); break;
                case InstantiationStatement instantiationStatement:
                    this.InstantiationStatementAsm(instantiationStatement); break;
                case WhileStatement whileStatement:
                    this.WhileStatementAsm(whileStatement); break;
                case IfStatement ifStatement:
                    this.IfStatementAsm(ifStatement); break;
                case AddAssignStatement addAssign:
                    this.AddAsignAsm(addAssign); break;
                case SubAssignStatement subAssign:
                    this.SubAssignAsm(subAssign); break;
                case MultAssignStatement multAssign:
                    this.MultAssignAsm(multAssign); break;
                case DivAssignStatement divAssign:
                    this.DivAssignAsm(divAssign); break;
                case IncrementExpr increment:
                    this.IncrementAsm(increment); break;
                case DecrementExpr decrement:
                    this.DecrementAsm(decrement); break;
                case NotExpr notExpr:
                    this.NotExprAsm(notExpr); break;
                case AndExpr andExpr:
                    this.AndExprAsm(andExpr); break;
                case OrExpr orExpr:
                    this.OrExprAsm(orExpr); break;
                case NotEqualExpr notEqualExpr:
                    this.NotEqualExprAsm(notEqualExpr); break;
                case PrintStatement printStatement:
                    this.PrintStatementAsm(printStatement); break;
                case RefTypeCreationStatement refTypeCreation:
                    this.RefTypeCreationStatementAsm(refTypeCreation); break;
                case ArrayIndexing arrayIndexing:
                    this.ArrayIndexingAsm(arrayIndexing); break;
                case AssignArrayField assignArray:
                    this.AssignArrayFieldAsm(assignArray); break;
                case DestructionStatement destruction:
                    this.DestructionStatementAsm(destruction); break;
                case NullLit nullLit:
                    this.NullLitAsm(nullLit); break;
                case StructDefinition structDefinition:
                    this.StructDefinitionAsm(structDefinition); break;
                case StructPropertyAccess structPropertyAccess:
                    this.StructPropertyAccessAsm(structPropertyAccess);
                    break;
                case AssignStructProperty assignStruct:
                    this.AssignStructPropertyAsm(assignStruct); break;
                case ModExpr modExpr:
                    this.ModExprAsm(modExpr); break;
                default:
                    throw new CodeGenerationNotImplementedException(astNode.ToString(), this.CurrentFile, astNode.Line, astNode.Column);
            }
        }

        public void GenerateAssember(IAST astNode)
        {
            this.GetAssember(astNode);
        }

        private void InitializeFile(string fileName)
        {
            this.Depth += 1;

            this.WriteLine($".file \"{fileName}\"");
            this.WriteLine(".text");

            this.Depth -= 1;
        }

        public void WriteToFile(string filePath, IAST astNode)
        {
            int indexOfSlash;
            string fileName;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                indexOfSlash = filePath.LastIndexOf('\\');
            else
                indexOfSlash = filePath.LastIndexOf('/');

            if (indexOfSlash == -1)
                fileName = filePath;
            else
                fileName = filePath.Substring(indexOfSlash + 1, filePath.Length - (indexOfSlash + 1));
            this.InitializeFile(fileName);

            try
            {
                this.GetAssember(astNode);
            }
            catch(CodeAlreadyGeneratedException) // no need to do anything
            {
                return;
            }

            string fileContent = this.Sb.ToString();

            if (this.DoubleNumbers.Length > 0)
                fileContent = fileContent + this.DoubleNumbers.ToString();
            if (this.Strings.Length > 0)
                fileContent = fileContent + this.Strings.ToString();

            fileName = filePath.Substring(0, filePath.IndexOf($".{Constants.SOURCE_FILE_ENDING}")) + ".S";

            using (StreamWriter sw = File.CreateText(fileName))
            {
                sw.Write(fileContent);
            }
        }

        private void FunctionDefinitionAsm(FunctionDefinition funDef)
        {
            // if FunctionDefinition is only prototype, nothing todo -> return
            if(funDef.isPrototype())
                return;
            FunctionAsm funAsm;

            if (this.FunctionMap.ContainsKey(funDef.Name))
                funAsm = this.FunctionMap[funDef.Name];
            else
            {
                funAsm = new FunctionAsm(funDef.Name);
                this.FunctionMap.Add(funDef.Name, funAsm);
                this.FillVariableMap(funAsm, funDef);
            }

            this.VariableMap = funAsm.VariableMap;
            this.LocalVariablePointer = 0;
            this.StackCounter = 0;

            this.Depth += 1;

            this.WriteLine($".global {funDef.Name}");

            this.Depth -= 1;

            this.WriteLine($"{funDef.Name}:");

            this.Depth += 1;

            // save previous basepointer
            this.WritePush("%rbp");
            // set the new basepointer
            this.WriteLine("movq %rsp, %rbp");

            this.LocalVariableCount = funDef.GetLocalVariables();
            int offSet = this.LocalVariableCount * -8;
            this.StackCounter -= offSet;

            if (offSet < 0)
                this.WriteLine($"addq ${offSet}, %rsp");

            // push all argument-registers onto the stack
            this.ArgumentTypeCount(funDef.Args, out int intArgCount, out int doubleArgCount);
            int index = Math.Min(intArgCount, this.IntegerRegisters.Length);
            int lastFound = -1;

            for (int i = 0; i < index; i++)
            {
                this.WritePush(this.IntegerRegisters[i]);
                offSet -= 8;
                lastFound = this.GetNextIntArg(funDef.Args, lastFound);
                this.VariableMap[funDef.Args[lastFound].Name] = offSet;
            }

            index = Math.Min(doubleArgCount, this.DoubleRegisters.Length);
            lastFound = -1;

            for (int i = 0; i < index; i++)
            {
                this.WriteLine($"movq {this.DoubleRegisters[i]}, %rax");
                this.WritePush();
                offSet -= 8;
                lastFound = this.GetNextDoubleArg(funDef.Args, lastFound);
                this.VariableMap[funDef.Args[lastFound].Name] = offSet;
            }


            if (funDef.Name == "main")
            {
                this.InitializeRuntime();

                bool aligned = false;

                if (this.StackCounter % 16 == 0)
                {
                    aligned = true;
                    this.WritePush("$0");
                }

                this.WriteLine(this.StructDefinitionBuilder.ToString());

                if (aligned)
                    this.WritePop("%rbx");
            }

            this.GetAssember(funDef.Body);

            if (funDef.Name == "main")
                this.CleanUpRuntime();

            // if the function is a void function, make sure the important registers are set to 0
            if (funDef.ReturnType is VoidType)
            {
                this.WriteLine("movq $0, %rax");
                this.WriteLine("movl $0, %eax");
                this.WriteLine("cvtsi2sd %rax, %xmm0");
                this.WriteLine("movq %rbp, %rsp");
                this.WritePop("%rbp");
                this.WriteLine("ret");
            }

            this.Depth -= 1;
        }

        private void StructDefinitionAsm(StructDefinition structDef)
        {
            Random random = new Random();
            int id = random.Next();

            while (this.StructIdMap.ContainsValue(id))
                id = random.Next();

            this.StructIdMap[structDef.Name] = id;

            this.StructDefinitionBuilder.AppendLine($"{Constants.INDENTATION}movq ${id}, {this.IntegerRegisters[0]}");
            this.StructDefinitionBuilder.AppendLine($"{Constants.INDENTATION}movq ${structDef.GetSize()}, {this.IntegerRegisters[1]}");

            this.StructDefinitionBuilder.AppendLine($"{Constants.INDENTATION}call registerClass@PLT");
        }

        private void InitializeRuntime()
        {
            bool aligned = this.AlignStack();

            this.WriteLine("call initializeRuntime@PLT");

            if (aligned)
                this.WritePop("%rbx");
        }

        private void CleanUpRuntime()
        {
            bool aligned = this.AlignStack();

            this.WriteLine("call cleanUpRuntime@PLT");

            if (aligned)
                this.WritePop("%rbx");
        }

        private void WriteLine(string op)
        {
            for (int i = 0; i < this.Depth; i++)
                this.Sb.Append(Constants.INDENTATION);

            var indexOfSpace = op.IndexOf(' ');

            if (indexOfSpace >= 0)
            {
                var first = op.Substring(0, indexOfSpace);
                first = first.PadRight(7, ' ');

                this.Sb.Append(first);
                this.Sb.Append(op.Substring(indexOfSpace));
            }
            else
            {
                this.Sb.Append(op);
            }

            this.Sb.Append("\n");
        }

        private void WritePush(string register = "%rax")
        {
            this.WriteLine($"pushq {register}");
            this.StackCounter += 8;
        }

        private void WritePop(string register = "%rax")
        {
            this.WriteLine($"popq {register}");
            this.StackCounter -= 8;
        }

        private void WriteDoubleValue(DoubleLit doubleLit)
        {
            if (doubleLit.Value == null)
                throw new UnexpectedErrorException(this.CurrentFile, doubleLit.Line, doubleLit.Column);

            if (DoubleMap.ContainsKey(doubleLit.Value ?? 0))
                return;

            // generate new label for new double number
            this.DoubleNumbers.Append($".LD{this.DoubleNumbersLabelCount}:\n");
            // convert the double into two integer strings
            // where each of them represents 32bit of the IEEE754 representation
            this.DoubleToAssemblerString(doubleLit, out string second, out string first);
            // write the two values
            this.DoubleNumbers.Append($"{Constants.INDENTATION}.long {first}\n");
            this.DoubleNumbers.Append($"{Constants.INDENTATION}.long {second}\n");
            this.DoubleNumbers.Append($"{Constants.INDENTATION}.align 8\n");
            // remember which label corresponds to the given double value
            this.DoubleMap.Add(doubleLit.Value ?? 0, this.DoubleNumbersLabelCount);
            this.DoubleNumbersLabelCount += 1;
        }

        private void WriteString(IAST value)
        {
            this.Strings.Append($".LS{this.StringLabelCount}:\n");
            string stringVal = "";
            string type = "";

            switch (value.Type)
            {
                case IntType it:
                    stringVal = "%ld\\n";
                    type = "int";
                    break;
                case DoubleType dt:
                    stringVal = "%f\\n";
                    type = "double";
                    break;
                case BooleanType bt:
                    stringVal = "%ld\\n";
                    type = "int";
                    break;
                default:
                    throw new TypeNotAllowedException(value.Type.ToString(), this.CurrentFile, value.Line, value.Column);
            }
            this.StringLabelMap[type] = this.StringLabelCount++;
            this.Strings.Append($"{Constants.INDENTATION}.string \"{stringVal}\"\n");
        }

        private void DoubleToAssemblerString(DoubleLit doubleLit, out string leftPart, out string rightPart)
        {
            if (doubleLit.Value == null)
                throw new UnexpectedErrorException(this.CurrentFile, doubleLit.Line, doubleLit.Column);

            // convert the double into ieee754 number 
            var tmp = Convert.ToString(BitConverter.DoubleToInt64Bits((doubleLit.Value ?? 0)), 2).PadLeft(64, '0');
            leftPart = Convert.ToInt32(tmp.Substring(0, 32), 2).ToString();
            rightPart = Convert.ToInt32(tmp.Substring(32, 32), 2).ToString();
        }

        /// <summary>
        /// Calulates wether there are enought registers for the parameters, or not.
        /// If there are not enought registers of one of the two types (integer/boolean, double),
        /// the position in the parameter list where the overflow happend is returned in the
        /// coresponding integer argument
        /// </summary>
        private bool DoesOverflowRegisters(List<IAST> args, out int integerOverflowPosition, out int doubleOverflowPosition)
        {
            bool result = false;
            integerOverflowPosition = Int32.MaxValue;
            doubleOverflowPosition = Int32.MaxValue;
            int usedInt = 0;
            int usedDouble = 0;

            for (int i = 0; i < args.Count; i++)
            {
                if (args[i].Type is IntType || args[i].Type is BooleanType || args[i].Type is RefType)
                {
                    usedInt += 1;

                    if (usedInt > this.IntegerRegisters.Length)
                    {
                        result = true;
                        integerOverflowPosition = integerOverflowPosition == Int32.MaxValue ? i : integerOverflowPosition;
                    }
                }

                if (args[i].Type is DoubleType)
                {
                    usedDouble += 1;

                    if (usedDouble > this.DoubleRegisters.Length)
                    {
                        result = true;
                        doubleOverflowPosition = doubleOverflowPosition == Int32.MaxValue ? i : doubleOverflowPosition;
                    }
                }
            }

            return result;
        }

        private bool DoesOverflowRegistersFunDef(List<InstantiationStatement> args, out int integerOverflowPosition, out int doubleOverflowPosition)
        {
            bool result = false;
            integerOverflowPosition = Int32.MaxValue;
            doubleOverflowPosition = Int32.MaxValue;
            int usedInt = 0;
            int usedDouble = 0;

            for (int i = 0; i < args.Count; i++)
            {
                if (args[i].Type is IntType || args[i].Type is BooleanType || args[i].Type is RefType)
                {
                    usedInt += 1;

                    if (usedInt > this.IntegerRegisters.Length)
                    {
                        result = true;
                        integerOverflowPosition = integerOverflowPosition != Int32.MaxValue ? integerOverflowPosition : i;
                    }
                }

                if (args[i].Type is DoubleType)
                {
                    usedDouble += 1;

                    if (usedDouble > this.DoubleRegisters.Length)
                    {
                        result = true;
                        doubleOverflowPosition = doubleOverflowPosition != Int32.MaxValue ? doubleOverflowPosition : i;
                    }
                }
            }

            return result;
        }

        private void FillVariableMap(FunctionAsm functionAsm, FunctionDefinition functionDefinition)
        {
            bool doesOverflow = this.DoesOverflowRegistersFunDef(
                functionDefinition.Args,
                out int integerOverflowPosition,
                out int doubleOverflowPosition
            );

            // if necessary reserve space for overflown arguments on the stack
            if (doesOverflow)
            {
                int min = Math.Min(integerOverflowPosition, doubleOverflowPosition);
                int rbpOffset = +16;

                // calculate the position of the overflown arguments on the stack
                for (int i = functionDefinition.Args.Count - 1; i >= min; i--)
                {
                    InstantiationStatement arg = functionDefinition.Args[i];
                    switch (functionDefinition.Args[i].Type)
                    {
                        case IntType intType:
                            if (i >= integerOverflowPosition)
                            {
                                functionAsm.VariableMap.Add(arg.Name, rbpOffset);
                                rbpOffset += 8;
                            }

                            break;
                        case DoubleType doubleType:
                            if (i >= doubleOverflowPosition)
                            {
                                functionAsm.VariableMap.Add(arg.Name, rbpOffset);
                                rbpOffset += 8;
                            }

                            break;
                        case BooleanType booleanType:
                            if (i >= integerOverflowPosition)
                            {
                                functionAsm.VariableMap.Add(arg.Name, rbpOffset);
                                rbpOffset += 8;
                            }

                            break;
                        case RefType refType:
                            if (i >= integerOverflowPosition)
                            {
                                functionAsm.VariableMap.Add(arg.Name, rbpOffset);
                                rbpOffset += 8;
                            }

                            break;
                        default:
                            throw new UnknownTypeException(arg.Type.ToString(), this.CurrentFile, arg.Line, arg.Column);
                    }
                }
            }
        }

        private void ArgumentTypeCount(List<InstantiationStatement> args, out int intArgCount, out int doubleArgCount)
        {
            intArgCount = 0;
            doubleArgCount = 0;

            foreach (var arg in args)
            {
                if (arg.Type is DoubleType)
                    doubleArgCount++;

                if (arg.Type is BooleanType || arg.Type is IntType || arg.Type is RefType)
                    intArgCount++;
            }
        }

        private int GetNextIntArg(List<InstantiationStatement> args, int startIndex)
        {
            int i;
            for (i = startIndex + 1; i < args.Count; i++)
            {
                if (args[i].Type is IntType || args[i].Type is BooleanType || args[i].Type is RefType)
                    return i;
            }

            throw new ArgumentCountException("integer", this.CurrentFile, args[i - 1].Line, args[i - 1].Column);
        }

        private int GetNextDoubleArg(List<InstantiationStatement> args, int startIndex)
        {
            int i;
            for (i = startIndex + 1; i < args.Count; i++)
            {
                if (args[i].Type is DoubleType)
                    return i;
            }

            throw new ArgumentCountException("double", this.CurrentFile, args[i - 1].Line, args[i - 1].Column);
        }

        private void LoadArrayField(ArrayIndexing arrayIndexing)
        {
            this.GetAssember(arrayIndexing.Left);

            this.WritePush();

            this.GetAssember(arrayIndexing.Right);
            this.WriteLine("imulq $8, %rax");

            this.WritePop("%rbx");
            this.WriteLine("addq %rbx, %rax");
        }

        /*
        * if is VarExpr, just load the struct from the env and calculate the offset
        * if is ArrayIndexing, just load the struct form the env and calculate the offset
        * if is StructPropertyAccess:
        *   1. load the outer struct from the env
        *   2. calculate the offset of the inner struct
        *   3. add the offset to the address and store this on the stack
        *   4. set flag (C# code)
        *   5. do again
        */
        private void LoadStructProperty(StructPropertyAccess structProperty)
        {
            StructType st = structProperty.StructRef.Type as StructType;
            var structDef = this.RootProg.GetStructDefinition(st.StructName);
            string propName = "";
            bool hasInnerStruct = false;
            bool isArrayIndexing = false;

            switch (structProperty.Prop)
            {
                case VarExpr varExpr:
                    propName = varExpr.Name;
                    break;
                case ArrayIndexing arrayIndexing:
                    propName = (arrayIndexing.Left as VarExpr).Name;
                    isArrayIndexing = true;
                    break;
                case StructPropertyAccess propertyAccess:
                    propName = propertyAccess.StructRef.Name;
                    hasInnerStruct = true;
                    break;
                default:
                    throw new UnknownTypeException(structProperty.Prop.Type.ToString(), this.CurrentFile, structProperty.Prop.Line, structProperty.Prop.Column);
            }
            int propIndex = structDef.Properties.FindIndex(sp => sp.Name == propName);

            if (!this.InnerStruct)
                this.GetAssember(structProperty.StructRef);
            this.WriteLine($"addq ${propIndex * 8}, %rax");

            if (isArrayIndexing)
            {
                // save the base address of the array
                this.WritePush("(%rax)");
                // calculate the offset
                this.GetAssember((structProperty.Prop as ArrayIndexing).Right);
                // load the base address of the array
                this.WritePop("%rbx");
                this.WriteLine("imulq $8, %rax");
                // add the offset
                this.WriteLine("addq %rbx, %rax");
            }

            if (hasInnerStruct)
            {
                this.InnerStruct = true;
                // load the base address of the inner struct
                this.WriteLine("movq (%rax), %rax");
                // walk recursivly
                this.LoadStructProperty(structProperty.Prop as StructPropertyAccess);
                this.InnerStruct = false;
            }
        }

        private bool AlignStack()
        {
            bool result = false;

            if (this.StackCounter % 16 == 0)
            {
                this.WritePush("$0");
                result = true;
            }

            return result;
        }
    }
}