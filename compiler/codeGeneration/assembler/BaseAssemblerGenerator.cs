using System;
using System.Text;
using System.Collections.Generic;
using ll.AST;
using ll.type;
using System.Runtime.InteropServices;
using System.IO;

namespace ll.assembler
{
    public partial class AssemblerGenerator
    {
        private string indent = "    ";
        private int depth = 0;
        private StringBuilder sb = new StringBuilder();
        private StringBuilder doubleNumbers = new StringBuilder();
        private StringBuilder strings = new StringBuilder();
        private StringBuilder structDefinitionBuilder = new StringBuilder();
        private int labelCount = 0;
        private int doubleNumbersLabelCount = 0;
        private int stringLabelCount = 0;
        private string[] integerRegisters = { "%rdi", "%rsi", "%rdx", "%rcx", "%r8", "%r9" };
        private string[] doubleRegisters = { "%xmm0", "%xmm1", "%xmm2", "%xmm3", "%xmm4", "%xmm5", "%xmm6", "%xmm7" };
        private Dictionary<string, FunctionAsm> functionMap = new Dictionary<string, FunctionAsm>();
        private Dictionary<double, int> doubleMap = new Dictionary<double, int>();
        private Dictionary<string, int> variableMap;
        private Dictionary<string, int> stringLabelMap = new Dictionary<string, int>();
        private Dictionary<string, int> structIdMap = new Dictionary<string, int>();
        private int localVariablePointer = 0;
        private int localVariableCount = 0;
        private int stackCounter = 0;
        private bool innerStruct = false;

        public void PrintAssember()
        {
            Console.WriteLine(this.sb.ToString());
            if (this.doubleNumbers.Length > 0)
                Console.WriteLine(this.doubleNumbers.ToString());
            if (this.strings.Length > 0)
                Console.WriteLine(this.strings.ToString());
        }

        /**
        * <summary>Get the assembler code of the given AST node</summary>
        */
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
                    foreach (var fun in programNode.funDefs)
                        this.GetAssember(fun);
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
                    // this.StructDefinitionAsm(structDefinition);
                    break;
                case StructPropertyAccess structPropertyAccess:
                    this.StructPropertyAccessAsm(structPropertyAccess);
                    break;
                case AssignStructProperty assignStruct:
                    this.AssignStructPropertyAsm(assignStruct); break;
                case ModExpr modExpr:
                    this.ModExprAsm(modExpr); break;
                default:
                    throw new NotImplementedException($"Assembler generation not implemented for {astNode.ToString()}");
            }
        }

        public void GenerateAssember(IAST astNode)
        {
            this.GetAssember(astNode);
        }

        private void InitializeFile(string fileName)
        {
            this.depth += 1;

            this.WriteLine($".file \"{fileName}\"");
            this.WriteLine(".text");

            this.depth -= 1;
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

            this.GetAssember(astNode);

            string fileContent = this.sb.ToString();

            if (this.doubleNumbers.Length > 0)
                fileContent = fileContent + this.doubleNumbers.ToString();
            if (this.strings.Length > 0)
                fileContent = fileContent + this.strings.ToString();

            fileName = filePath.Substring(0, filePath.IndexOf(".ll")) + ".S";

            using (StreamWriter sw = File.CreateText(fileName))
            {
                sw.Write(fileContent);
            }
        }

        private void FunctionDefinitionAsm(FunctionDefinition funDef)
        {
            FunctionAsm funAsm;

            if (this.functionMap.ContainsKey(funDef.name))
                funAsm = this.functionMap[funDef.name];
            else
            {
                funAsm = new FunctionAsm(funDef.name);
                this.functionMap.Add(funDef.name, funAsm);
                this.FillVariableMap(funAsm, funDef);
            }

            this.variableMap = funAsm.variableMap;
            this.localVariablePointer = 0;
            this.stackCounter = 0;

            this.depth += 1;

            this.WriteLine($".global {funDef.name}");

            this.depth -= 1;

            this.WriteLine($"{funDef.name}:");

            this.depth += 1;

            // save previous basepointer
            this.WritePush("%rbp");
            // set the new basepointer
            this.WriteLine("movq %rsp, %rbp");

            this.localVariableCount = funDef.GetLocalVariables();
            int offSet = this.localVariableCount * -8;
            this.stackCounter -= offSet;

            if (offSet < 0)
                this.WriteLine($"addq ${offSet}, %rsp");

            // push all argument-registers onto the stack
            this.ArgumentTypeCount(funDef.args, out int intArgCount, out int doubleArgCount);
            int index = Math.Min(intArgCount, this.integerRegisters.Length);
            int lastFound = -1;

            for (int i = 0; i < index; i++)
            {
                this.WritePush(this.integerRegisters[i]);
                offSet -= 8;
                lastFound = this.GetNextIntArg(funDef.args, lastFound);
                this.variableMap[funDef.args[lastFound].name] = offSet;
            }

            index = Math.Min(doubleArgCount, this.doubleRegisters.Length);
            lastFound = -1;

            for (int i = 0; i < index; i++)
            {
                this.WriteLine($"movq {this.doubleRegisters[i]}, %rax");
                this.WritePush();
                offSet -= 8;
                lastFound = this.GetNextDoubleArg(funDef.args, lastFound);
                this.variableMap[funDef.args[lastFound].name] = offSet;
            }

            this.GetAssember(funDef.body);

            // if the function is a void function, make sure the important registers are set to 0
            if (funDef.returnType is VoidType)
            {
                this.WriteLine("movq $0, %rax");
                this.WriteLine("movl $0, %eax");
                this.WriteLine("cvtsi2sd %rax, %xmm0");
                this.WriteLine("movq %rbp, %rsp");
                this.WritePop("%rbp");
                this.WriteLine("ret");
            }

            this.depth -= 1;

        }

        private void StructDefinitionAsm(StructDefinition structDef)
        {
            Random random = new Random();
            int id = random.Next();

            while(this.structIdMap.ContainsValue(id))
                id = random.Next();
            
            this.structIdMap[structDef.name] = id;

            this.structDefinitionBuilder.AppendLine($"{this.indent}movq {id}, {this.integerRegisters[0]}");
            this.structDefinitionBuilder.AppendLine($"{this.indent}movq {structDef.GetSize()}, {this.integerRegisters[0]}");
        }

        private void WriteLine(string op)
        {
            for (int i = 0; i < this.depth; i++)
                this.sb.Append(this.indent);

            var indexOfSpace = op.IndexOf(' ');

            if (indexOfSpace >= 0)
            {
                var first = op.Substring(0, indexOfSpace);
                first = first.PadRight(7, ' ');

                this.sb.Append(first);
                this.sb.Append(op.Substring(indexOfSpace));
            }
            else
            {
                this.sb.Append(op);
            }

            this.sb.Append("\n");
        }

        private void WritePush(string register = "%rax")
        {
            this.WriteLine($"pushq {register}");
            this.stackCounter += 8;
        }

        private void WritePop(string register = "%rax")
        {
            this.WriteLine($"popq {register}");
            this.stackCounter -= 8;
        }

        private void WriteDoubleValue(DoubleLit doubleLit)
        {
            if (doubleLit.n == null)
                throw new ArgumentNullException();

            if (doubleMap.ContainsKey(doubleLit.n ?? 0))
                return;

            // generate new label for new double number
            this.doubleNumbers.Append($".LD{this.doubleNumbersLabelCount}:\n");
            // convert the double into two integer strings
            // where each of them represents 32bit of the IEEE754 representation
            this.DoubleToAssemblerString(doubleLit, out string second, out string first);
            // write the two values
            this.doubleNumbers.Append($"{indent}.long {first}\n");
            this.doubleNumbers.Append($"{indent}.long {second}\n");
            this.doubleNumbers.Append($"{indent}.align 8\n");
            // remember which label corresponds to the given double value
            this.doubleMap.Add(doubleLit.n ?? 0, this.doubleNumbersLabelCount);
            this.doubleNumbersLabelCount += 1;
        }

        private void WriteString(IAST value)
        {
            this.strings.Append($".LS{this.stringLabelCount}:\n");
            string stringVal = "";
            string type = "";

            switch (value.type)
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
                    throw new ArgumentException($"Type {value.type.typeName} not supported for print operation");
            }
            this.stringLabelMap[type] = this.stringLabelCount++;
            this.strings.Append($"{this.indent}.string \"{stringVal}\"\n");
        }

        private void DoubleToAssemblerString(DoubleLit doubleLit, out string leftPart, out string rightPart)
        {
            if (doubleLit.n == null)
                throw new ArgumentNullException();

            // convert the double into ieee754 number 
            var tmp = Convert.ToString(BitConverter.DoubleToInt64Bits((doubleLit.n ?? 0)), 2).PadLeft(64, '0');
            leftPart = Convert.ToInt32(tmp.Substring(0, 32), 2).ToString();
            rightPart = Convert.ToInt32(tmp.Substring(32, 32), 2).ToString();
        }

        /**
        * <summary> Calulates wether there are enought registers for the parameters, or not.
        * If there are not enought registers of one of the two types (integer/boolean, double),
        * the position in the parameter list where the overflow happend is returned in the
        * coresponding integer argument </summary> 
        */
        private bool DoesOverflowRegisters(List<IAST> args, out int integerOverflowPosition, out int doubleOverflowPosition)
        {
            bool result = false;
            integerOverflowPosition = Int32.MaxValue;
            doubleOverflowPosition = Int32.MaxValue;
            int usedInt = 0;
            int usedDouble = 0;

            for (int i = 0; i < args.Count; i++)
            {
                if (args[i].type is IntType || args[i].type is BooleanType || args[i].type is RefType)
                {
                    usedInt += 1;

                    if (usedInt > this.integerRegisters.Length)
                    {
                        result = true;
                        integerOverflowPosition = integerOverflowPosition == Int32.MaxValue ? i : integerOverflowPosition;
                    }
                }

                if (args[i].type is DoubleType)
                {
                    usedDouble += 1;

                    if (usedDouble > this.doubleRegisters.Length)
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
                if (args[i].type is IntType || args[i].type is BooleanType || args[i].type is RefType)
                {
                    usedInt += 1;

                    if (usedInt > this.integerRegisters.Length)
                    {
                        result = true;
                        integerOverflowPosition = integerOverflowPosition != Int32.MaxValue ? integerOverflowPosition : i;
                    }
                }

                if (args[i].type is DoubleType)
                {
                    usedDouble += 1;

                    if (usedDouble > this.doubleRegisters.Length)
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
                functionDefinition.args,
                out int integerOverflowPosition,
                out int doubleOverflowPosition
            );

            // if necessary reserve space for overflown arguments on the stack
            if (doesOverflow)
            {
                int min = Math.Min(integerOverflowPosition, doubleOverflowPosition);
                int rbpOffset = +16;

                // calculate the position of the overflown arguments on the stack
                for (int i = functionDefinition.args.Count - 1; i >= min; i--)
                {
                    switch (functionDefinition.args[i].type)
                    {
                        case IntType intType:
                            if (i >= integerOverflowPosition)
                            {
                                functionAsm.variableMap.Add(functionDefinition.args[i].name, rbpOffset);
                                rbpOffset += 8;
                            }

                            break;
                        case DoubleType doubleType:
                            if (i >= doubleOverflowPosition)
                            {
                                functionAsm.variableMap.Add(functionDefinition.args[i].name, rbpOffset);
                                rbpOffset += 8;
                            }

                            break;
                        case BooleanType booleanType:
                            if (i >= integerOverflowPosition)
                            {
                                functionAsm.variableMap.Add(functionDefinition.args[i].name, rbpOffset);
                                rbpOffset += 8;
                            }

                            break;
                        case RefType refType:
                            if (i >= integerOverflowPosition)
                            {
                                functionAsm.variableMap.Add(functionDefinition.args[i].name, rbpOffset);
                                rbpOffset += 8;
                            }

                            break;
                        default:
                            throw new ArgumentException($"Unknown type {functionDefinition.args[i].type.typeName}");
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
                if (arg.type is DoubleType)
                    doubleArgCount++;

                if (arg.type is BooleanType || arg.type is IntType || arg.type is RefType)
                    intArgCount++;
            }
        }

        private int GetNextIntArg(List<InstantiationStatement> args, int startIndex)
        {
            for (int i = startIndex + 1; i < args.Count; i++)
            {
                if (args[i].type is IntType || args[i].type is BooleanType || args[i].type is RefType)
                    return i;
            }

            throw new ArgumentOutOfRangeException("Expected to find more integer arguments");
        }

        private int GetNextDoubleArg(List<InstantiationStatement> args, int startIndex)
        {
            for (int i = startIndex + 1; i < args.Count; i++)
            {
                if (args[i].type is DoubleType)
                    return i;
            }

            throw new ArgumentOutOfRangeException("Expected to find more double arguments");
        }

        private void LoadArrayField(ArrayIndexing arrayIndexing)
        {
            this.GetAssember(arrayIndexing.left);

            this.WritePush();

            this.GetAssember(arrayIndexing.right);
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
            StructType st = structProperty.structRef.type as StructType;
            var structDef = IAST.structs[st.structName];
            string propName = "";
            bool hasInnerStruct = false;
            bool isArrayIndexing = false;

            switch (structProperty.prop)
            {
                case VarExpr varExpr:
                    propName = varExpr.name;
                    break;
                case ArrayIndexing arrayIndexing:
                    propName = (arrayIndexing.left as VarExpr).name;
                    isArrayIndexing = true;
                    break;
                case StructPropertyAccess propertyAccess:
                    propName = propertyAccess.structRef.name;
                    hasInnerStruct = true;
                    break;
                default:
                    throw new ArgumentException("Unknown property type");
            }
            int propIndex = structDef.properties.FindIndex(sp => sp.name == propName);

            if (!this.innerStruct)
                this.GetAssember(structProperty.structRef);
            this.WriteLine($"addq ${propIndex * 8}, %rax");

            if (isArrayIndexing)
            {
                // save the base address of the array
                this.WritePush("(%rax)");
                // calculate the offset
                this.GetAssember((structProperty.prop as ArrayIndexing).right);
                // load the base address of the array
                this.WritePop("%rbx");
                this.WriteLine("imulq $8, %rax");
                // add the offset
                this.WriteLine("addq %rbx, %rax");
            }

            if (hasInnerStruct)
            {
                this.innerStruct = true;
                // load the base address of the inner struct
                this.WriteLine("movq (%rax), %rax");
                // walk recursivly
                this.LoadStructProperty(structProperty.prop as StructPropertyAccess);
                this.innerStruct = false;
            }
        }

        private bool AlignStack()
        {
            bool result = false;

            if(this.stackCounter % 16 == 0)
            {
                this.WritePush("$0");
                result = true;
            }

            return result;
        }
    }
}