using System;
using System.Text;
using System.Collections.Generic;
using ll.AST;
using ll.type;
using System.Runtime.InteropServices;
using System.IO;

namespace ll.assembler
{
    public class GenAssembler
    {
        private string indent = "    ";
        private int depth = 0;
        private StringBuilder sb = new StringBuilder();
        private StringBuilder doubleNumbers = new StringBuilder();
        private StringBuilder strings = new StringBuilder();
        private int labelCount = 0;
        private int doubleNumbersLabelCount = 0;
        private int stringLabelCount = 0;
        private string[] integerRegisters = { "%rdi", "%rsi", "%rdx", "%rcx", "%r8", "%r9" };
        private string[] doubleRegisters = { "%xmm0", "%xmm1", "%xmm2", "%xmm3", "%xmm4", "%xmm5", "%xmm6", "%xmm7" };
        private Dictionary<string, FunctionAsm> functionMap = new Dictionary<string, FunctionAsm>();
        private Dictionary<double, int> doubleMap = new Dictionary<double, int>();
        private Dictionary<string, int> variableMap;
        private Dictionary<string, int> stringLabelMap = new Dictionary<string, int>();
        private int localVariablePointer = 0;
        private int localVariableCount = 0;
        private int stackCounter = 0;

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
                    break;
                case StructPropertyAccess structPropertyAccess:
                    this.StructPropertyAccessAsm(structPropertyAccess); break;
                case AssignStructProperty assignStruct:
                    this.AssignStructPropertyAsm(assignStruct); break;
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

        private void IntLitAsm(IntLit intLit)
        {
            this.WriteLine($"movq ${intLit.n}, %rax");
        }

        private void DoubleLitAsm(DoubleLit doubleLit)
        {
            this.WriteDoubleValue(doubleLit);
            this.WriteLine($"movq .LD{this.doubleMap[doubleLit.n ?? 0]}(%rip), %xmm0");
        }

        private void BoolLitAsm(BoolLit boolLit)
        {
            this.WriteLine($"movq ${((boolLit.value ?? false) ? 1 : 0)}, %rax");
        }

        private void AddExprAsm(AddExpr addExpr)
        {
            switch (addExpr.right.type)
            {
                case DoubleType doubleType:
                    this.GetAssember(addExpr.right);
                    this.WriteLine("movq %xmm0, %rax");
                    this.WritePush();
                    this.GetAssember(addExpr.left);

                    // convert int to double
                    if (addExpr.left.type is IntType)
                        this.WriteLine("cvtsi2sdq %rax, %xmm0");

                    this.WritePop();
                    this.WriteLine("movq %rax, %xmm1");
                    this.WriteLine("addsd %xmm1, %xmm0");

                    break;
                case IntType intType:
                    this.GetAssember(addExpr.right);
                    this.WritePush();
                    this.GetAssember(addExpr.left);

                    if (addExpr.left.type is DoubleType)
                    {
                        this.WritePop();
                        // convert int to double
                        this.WriteLine("cvtsi2sdq %rax, %xmm1");
                        this.WriteLine("addsd %xmm1, %xmm0");
                    }
                    else
                    {
                        this.WritePop("%rbx");
                        this.WriteLine("addq %rbx, %rax");
                    }
                    break;
                default:
                    throw new InvalidOperationException($"Could not perform addition on type {addExpr.type.typeName}");
            }
        }

        private void SubExprAsm(SubExpr subExpr)
        {
            switch (subExpr.right.type)
            {
                case DoubleType doubleType:
                    this.GetAssember(subExpr.right);
                    this.WriteLine("movq %xmm0, %rax");
                    this.WritePush();
                    this.GetAssember(subExpr.left);

                    // convert int to double
                    if (subExpr.left.type is IntType)
                        this.WriteLine("cvtsi2sdq %rax, %xmm0");

                    this.WritePop();
                    this.WriteLine("movq %rax, %xmm1");
                    this.WriteLine("subsd %xmm1, %xmm0");

                    break;
                case IntType intType:
                    this.GetAssember(subExpr.right);
                    this.WritePush();
                    this.GetAssember(subExpr.left);

                    if (subExpr.left.type is DoubleType)
                    {
                        this.WritePop();
                        // convert int to double
                        this.WriteLine("cvtsi2sdq %rax, %xmm1");
                        this.WriteLine("subsd %xmm1, %xmm0");
                    }
                    else
                    {
                        WritePop("%rbx");
                        this.WriteLine("subq %rbx, %rax");
                    }
                    break;
                default:
                    throw new InvalidOperationException($"Could not perform addition on type {subExpr.type.typeName}");
            }
        }

        private void MultExprAsm(MultExpr multExpr)
        {
            switch (multExpr.right.type)
            {
                case DoubleType doubleType:
                    this.GetAssember(multExpr.right);
                    this.WriteLine("movq %xmm0, %rax");
                    this.WritePush();
                    this.GetAssember(multExpr.left);

                    // convert int to double
                    if (multExpr.left.type is IntType)
                        this.WriteLine("cvtsi2sdq %rax, %xmm0");

                    this.WritePop();
                    this.WriteLine("movq %rax, %xmm1");
                    this.WriteLine("mulsd %xmm1, %xmm0");

                    break;
                case IntType intType:
                    this.GetAssember(multExpr.right);
                    this.WritePush();
                    this.GetAssember(multExpr.left);

                    if (multExpr.left.type is DoubleType)
                    {
                        this.WritePop();
                        // convert int to double
                        this.WriteLine("cvtsi2sdq %rax, %xmm1");
                        this.WriteLine("mulsd %xmm1, %xmm0");
                    }
                    else
                    {
                        this.WritePop("%rbx");
                        this.WriteLine("imulq %rbx, %rax");
                    }
                    break;
                default:
                    throw new InvalidOperationException($"Could not perform addition on type {multExpr.type.typeName}");
            }
        }

        private void DivExprAsm(DivExpr divExpr)
        {
            switch (divExpr.right.type)
            {
                case DoubleType doubleType:
                    this.GetAssember(divExpr.right);
                    this.WriteLine("movq %xmm0, %rax");
                    this.WritePush();
                    this.GetAssember(divExpr.left);

                    // convert int to double
                    if (divExpr.left.type is IntType)
                        this.WriteLine("cvtsi2sdq %rax, %xmm0");

                    this.WritePop();
                    this.WriteLine("movq %rax, %xmm1");
                    this.WriteLine("divsd %xmm1, %xmm0");

                    break;
                case IntType intType:
                    this.GetAssember(divExpr.right);
                    this.WritePush();
                    this.GetAssember(divExpr.left);

                    if (divExpr.left.type is DoubleType)
                    {
                        this.WritePop();
                        // convert int to double
                        this.WriteLine("cvtsi2sdq %rax, %xmm1");
                        this.WriteLine("divsd %xmm1, %xmm0");
                    }
                    else
                    {
                        this.WritePop("%rbx");
                        this.WriteLine("movq $0, %rdx");
                        this.WriteLine("idivq %rbx");
                    }
                    break;
                default:
                    throw new InvalidOperationException($"Could not perform addition on type {divExpr.type.typeName}");
            }
        }

        private void GreaterExprAsm(GreaterExpr greaterExpr)
        {
            switch (greaterExpr.right.type)
            {
                case DoubleType doubleType:
                    this.GetAssember(greaterExpr.right);
                    this.WriteLine("movq %xmm0, %rax");
                    this.WritePush();

                    this.GetAssember(greaterExpr.left);

                    // convert int to double
                    if (greaterExpr.left.type is IntType)
                        this.WriteLine("cvtsi2sdq %rax, %xmm0");

                    this.WritePop();
                    this.WriteLine("movq %rax, %xmm1");
                    this.WriteLine("ucomisd %xmm1, %xmm0");

                    if (greaterExpr.equal)
                        this.WriteLine("setae %al");
                    else
                        this.WriteLine("seta %al");

                    break;
                case IntType intType:
                    this.GetAssember(greaterExpr.right);
                    this.WritePush();

                    this.GetAssember(greaterExpr.left);

                    if (greaterExpr.left.type is DoubleType)
                    {
                        this.WritePop();
                        // convert int to double
                        this.WriteLine("cvtsi2sdq %rax, %xmm1");
                        this.WriteLine("ucomisd %xmm1, %xmm0");

                        if (greaterExpr.equal)
                            this.WriteLine("setae %al");
                        else
                            this.WriteLine("seta %al");
                    }
                    else
                    {
                        this.WritePop("%rbx");
                        this.WriteLine("cmpq %rbx, %rax");

                        if (greaterExpr.equal)
                            this.WriteLine("setge %al");
                        else
                            this.WriteLine("setg %al");
                    }

                    break;
                default:
                    throw new InvalidOperationException($"Can not use \"greater\" operator with {greaterExpr.right.type}");
            }

            this.WriteLine("movzbl %al, %rax");
        }

        private void LessExprAsm(LessExpr lessExpr)
        {
            switch (lessExpr.right.type)
            {
                case DoubleType doubleType:
                    this.GetAssember(lessExpr.right);
                    this.WriteLine("movq %xmm0, %rax");
                    this.WritePush();

                    this.GetAssember(lessExpr.left);

                    // convert int to double
                    if (lessExpr.left.type is IntType)
                        this.WriteLine("cvtsi2sdq %rax, %xmm0");

                    this.WritePop();
                    this.WriteLine("movq %rax, %xmm1");

                    // compare the two values
                    // it is strange that, when one of the values was converted from int
                    // the order has to change
                    this.WriteLine("ucomisd %xmm1, %xmm0");

                    if (lessExpr.equal)
                        this.WriteLine("setbe %al");
                    else
                        this.WriteLine("setb %al");

                    break;
                case IntType intType:
                    this.GetAssember(lessExpr.right);
                    this.WritePush();

                    this.GetAssember(lessExpr.left);

                    if (lessExpr.left.type is DoubleType)
                    {
                        this.WritePop();
                        // convert int to double
                        this.WriteLine("cvtsi2sdq %rax, %xmm1");
                        // compare the two values
                        this.WriteLine("ucomisd %xmm1, %xmm0");

                        if (lessExpr.equal)
                            this.WriteLine("setbe %al");
                        else
                            this.WriteLine("setb %al");
                    }
                    else
                    {
                        this.WritePop("%rbx");
                        this.WriteLine("cmpq %rbx, %rax");

                        if (lessExpr.equal)
                            this.WriteLine("setle %al");
                        else
                            this.WriteLine("setl %al");
                    }

                    break;
                default:
                    throw new InvalidOperationException($"Can not use \"less\" operator with {lessExpr.right.type}");
            }

            this.WriteLine("movzbl %al, %rax");
        }

        private void EqualityExprAsm(EqualityExpr equalityExpr)
        {
            switch (equalityExpr.right.type)
            {
                case DoubleType doubleType:
                    this.GetAssember(equalityExpr.right);
                    this.WriteLine("movq %xmm0, %rax");
                    this.WritePush();

                    this.GetAssember(equalityExpr.left);

                    // convert int to double
                    if (equalityExpr.left.type is IntType)
                        this.WriteLine("cvtsi2sdq %rax, %xmm0");

                    this.WritePop();
                    this.WriteLine("movq %rax, %xmm1");
                    this.WriteLine("ucomisd %xmm0, %xmm1");
                    this.WriteLine("setz %al");

                    break;
                case IntType intType:
                    this.GetAssember(equalityExpr.right);
                    this.WritePush();

                    this.GetAssember(equalityExpr.left);

                    if (equalityExpr.left.type is DoubleType)
                    {
                        this.WritePop();
                        // convert int to double
                        this.WriteLine("cvtsi2sdq %rax, %xmm1");
                        this.WriteLine("ucomisd %xmm0, %xmm1");
                        this.WriteLine("setz %al");
                    }
                    else
                    {
                        this.WritePop("%rbx");
                        this.WriteLine("cmpq %rbx, %rax");
                        this.WriteLine("sete %al");
                    }

                    break;
                case BooleanType booleanType:
                    this.GetAssember(equalityExpr.right);
                    this.WritePush();

                    this.GetAssember(equalityExpr.left);
                    this.WritePop("%rbx");
                    this.WriteLine("cmpq %rbx, %rax");
                    this.WriteLine("sete %al");

                    break;
                default:
                    throw new InvalidOperationException($"Can not use \"equal\" operator with {equalityExpr.right.type}");
            }

            this.WriteLine("movzbl %al, %rax");
        }

        private void BlockStatementAsm(BlockStatement blockStatement)
        {
            foreach (var comp in blockStatement.body)
                this.GetAssember(comp);
        }

        private void ReturnStatementAsm(ReturnStatement returnStatement)
        {
            this.GetAssember(returnStatement.returnValue);
            this.WriteLine("movq %rbp, %rsp");
            this.WritePop("%rbp");
            this.WriteLine("ret");
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

            this.depth += 1;

            this.WriteLine($".global {funDef.name}");
            this.WriteLine($".type {funDef.name}, @function");

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

        private void FunctionCallAsm(FunctionCall functionCall)
        {
            FunctionDefinition funDef = IAST.funs.GetValueOrDefault(functionCall.name);
            FunctionAsm functionAsm;

            if (this.functionMap.ContainsKey(functionCall.name))
                functionAsm = this.functionMap[functionCall.name];
            else
            {
                functionAsm = new FunctionAsm(functionCall.name);
                this.functionMap.Add(functionCall.name, functionAsm);

            }

            bool doesOverflow = this.DoesOverflowRegisters(
                functionCall.args,
                out int integerOverflowPosition,
                out int doubleOverflowPosition
            );

            // if necessary reserve space for overflown arguments on the stack
            if (doesOverflow)
            {
                int min = Math.Min(integerOverflowPosition, doubleOverflowPosition);
                int rbpOffset = +16;

                // calculate the position of the overflown arguments on the stack
                for (int i = functionCall.args.Count - 1; i >= min; i--)
                {
                    switch (functionCall.args[i].type)
                    {
                        case IntType intType:
                            if (i >= integerOverflowPosition)
                            {
                                functionAsm.variableMap[funDef.args[i].name] = rbpOffset;
                                rbpOffset += 8;
                            }

                            break;
                        case DoubleType doubleType:
                            if (i >= doubleOverflowPosition)
                            {
                                functionAsm.variableMap[funDef.args[i].name] = rbpOffset;
                                rbpOffset += 8;
                            }

                            break;
                        case BooleanType booleanType:
                            if (i >= integerOverflowPosition)
                            {
                                functionAsm.variableMap[funDef.args[i].name] = rbpOffset;
                                rbpOffset += 8;
                            }

                            break;
                        default:
                            throw new ArgumentException($"Unknown type {functionCall.args[i].type.typeName}");
                    }
                }
                this.stackCounter += rbpOffset - 16;
                this.WriteLine($"subq ${rbpOffset - 16}, %rsp");
            }

            int index = Math.Min(functionCall.args.Count, integerOverflowPosition);
            // move integer/boolean arguments into registers until they are full
            for (int i = 0; i < index; i++)
            {
                if (functionCall.args[i].type is IntType || functionCall.args[i].type is BooleanType)
                {
                    this.GetAssember(functionCall.args[i]);

                    this.WriteLine($"movq %rax, {this.integerRegisters[functionAsm.usedIntegerRegisters]}");
                    functionAsm.usedIntegerRegisters++;
                }
            }

            // move integer/boolean arguments that overflew on stack
            for (int i = functionCall.args.Count - 1; i >= integerOverflowPosition; i++)
            {
                if (functionCall.args[i].type is IntType || functionCall.args[i].type is BooleanType)
                {
                    this.GetAssember(functionCall.args[i]);

                    this.WriteLine($"movq %rax, {functionAsm.variableMap[funDef.args[i].name] - 16}(%rsp)");
                }
            }

            // move double arguments that overflew on the stack
            for (int i = functionCall.args.Count - 1; i >= doubleOverflowPosition; i++)
            {
                if (functionCall.args[i].type is DoubleType)
                {
                    this.GetAssember(functionCall.args[i]);

                    this.WriteLine($"movq %xmm0, {functionAsm.variableMap[funDef.args[i].name] - 16}(%rsp)");
                }
            }

            // calculate the rest of the double arguments and push them on the stack
            for (int i = Math.Min(functionCall.args.Count - 1, doubleOverflowPosition); i >= 0; i--)
            {
                if (functionCall.args[i].type is DoubleType)
                {
                    this.GetAssember(functionCall.args[i]);

                    this.WriteLine($"movq %xmm0, %rax");
                    this.WritePush();
                    functionAsm.usedDoubleRegisters++;
                }
            }

            // move the previously pushed double arguments into the double registers
            for (int i = 0; i < functionAsm.usedDoubleRegisters; i++)
            {
                this.WritePop();
                this.WriteLine($"movq %rax, {doubleRegisters[i]}");
            }

            this.WriteLine($"call {functionCall.name}");
        }

        private void VariableAsm(VarExpr varExpr)
        {
            // move the variables value from the stack into the type specific registers
            if (varExpr.type is DoubleType)
                this.WriteLine($"movq {this.variableMap[varExpr.name]}(%rbp), %xmm0");

            if (varExpr.type is BooleanType
            || varExpr.type is IntType
            || varExpr.type is IntArrayType
            || varExpr.type is DoubleArrayType
            || varExpr.type is BoolArrayType
            || varExpr.type is StructType)
                this.WriteLine($"movq {this.variableMap[varExpr.name]}(%rbp), %rax");
        }

        private void AssignAsm(AssignStatement assignStatement)
        {
            // if it is the first time the variable is mentioned in this context
            if (!this.variableMap.ContainsKey(assignStatement.variable.name))
            {
                // and there are still more local variables in this function
                if (++this.localVariablePointer > this.localVariableCount)
                    throw new IndexOutOfRangeException("Tried to create more local variables than the detected amount");
                // map the next empty spot of the reserved stack to the given variable
                this.variableMap.Add(assignStatement.variable.name, this.localVariablePointer * (-8));
            }

            // generate the code of the variables value
            this.GetAssember(assignStatement.value);

            string register = "";

            if (assignStatement.variable.type is IntType
            || assignStatement.variable.type is BooleanType
            || assignStatement.value.type is IntArrayType
            || assignStatement.value.type is BoolArrayType
            || assignStatement.value.type is DoubleArrayType
            || assignStatement.value.type is NullType
            || assignStatement.value.type is StructType)
                register = "%rax";

            if (assignStatement.variable.type is DoubleType)
            {
                if (assignStatement.value.type is IntType)
                    this.WriteLine("cvtsi2sdq %rax, %xmm0");

                register = "%xmm0";
            }


            // save the value of the variable on the stack
            this.WriteLine($"movq {register}, {this.variableMap[assignStatement.variable.name]}(%rbp)");
        }

        private void InstantiationStatementAsm(InstantiationStatement instantiationStatement)
        {
            // map the next empty reserved spot on the stack for the given variable
            this.variableMap.Add(instantiationStatement.name, ++this.localVariablePointer * (-8));
        }

        private void WhileStatementAsm(WhileStatement whileStatement)
        {
            this.WriteLine($"jmp .L{this.labelCount + 1}");
            // create a label for the body
            this.depth -= 1;
            this.WriteLine($".L{this.labelCount++}:");
            this.depth += 1;

            // generate the code for the body
            this.GetAssember(whileStatement.body);

            // create the label for the condition
            this.depth -= 1;
            this.WriteLine($".L{this.labelCount++}:");
            this.depth += 1;

            // generate the code for the condition
            this.GetAssember(whileStatement.condition);

            // if the value of the condition is true, jump to the body
            this.WriteLine("cmpq $1, %rax");
            this.WriteLine($"je .L{this.labelCount - 2}");
        }

        private void IfStatementAsm(IfStatement ifStatement)
        {
            // generate the code of the condition
            this.GetAssember(ifStatement.cond);

            // if the condition is true
            this.WriteLine("cmpq $1, %rax");
            // jump to the label of the if clause
            this.WriteLine($"je .L{this.labelCount + 1}");

            // create a label for the else case
            this.depth -= 1;
            this.WriteLine($".L{this.labelCount++}:");
            this.depth += 1;

            // if there is an else case
            if (ifStatement.elseBody != null)
                // generate the assembler for the else case
                this.GetAssember(ifStatement.elseBody);

            // jump to the end of the if statement
            this.WriteLine($"jmp .L{this.labelCount + 1}");

            // create a label for the if case
            this.depth -= 1;
            this.WriteLine($".L{this.labelCount++}:");
            this.depth += 1;

            // generate the assembler for the if case
            this.GetAssember(ifStatement.ifBody);

            // create a label for the end of the if statement
            this.depth -= 1;
            this.WriteLine($".L{this.labelCount++}:");
            this.depth += 1;
        }

        private void AddAsignAsm(AddAssignStatement addAssignStatement)
        {
            this.GetAssember(addAssignStatement.right);

            switch (addAssignStatement.left.type)
            {
                case IntType intType:
                    this.WriteLine($"addq %rax, {this.variableMap[addAssignStatement.left.name]}(%rbp)"); break;
                case DoubleType doubleType:
                    if (addAssignStatement.right.type is IntType)
                        this.WriteLine("cvtsi2sdq %rax, %xmm0");

                    this.WriteLine("movq %xmm0, %xmm1");
                    this.WriteLine($"movq {this.variableMap[addAssignStatement.left.name]}(%rbp), %xmm0");

                    this.WriteLine("addsd %xmm1, %xmm0");
                    this.WriteLine($"movq %xmm0, {this.variableMap[addAssignStatement.left.name]}(%rbp)");
                    break;
                default:
                    throw new ArgumentException($"AddAssign Statement not compatible with type \"{addAssignStatement.left.type.typeName}\"");
            }
        }

        private void SubAssignAsm(SubAssignStatement subAssignStatement)
        {
            this.GetAssember(subAssignStatement.right);

            switch (subAssignStatement.left.type)
            {
                case IntType intType:
                    this.WriteLine($"subq %rax, {this.variableMap[subAssignStatement.left.name]}(%rbp)"); break;
                case DoubleType doubleType:
                    if (subAssignStatement.right.type is IntType)
                        this.WriteLine("cvtsi2sdq %rax, %xmm0");

                    this.WriteLine("movq %xmm0, %xmm1");
                    this.WriteLine($"movq {this.variableMap[subAssignStatement.left.name]}(%rbp), %xmm0");

                    this.WriteLine("subsd %xmm1, %xmm0");
                    this.WriteLine($"movq %xmm0, {this.variableMap[subAssignStatement.left.name]}(%rbp)");
                    break;
                default:
                    throw new ArgumentException($"SubAssign Statement not compatible with type \"{subAssignStatement.left.type.typeName}\"");
            }
        }

        private void MultAssignAsm(MultAssignStatement multAssignStatement)
        {
            this.GetAssember(multAssignStatement.right);

            switch (multAssignStatement.left.type)
            {
                case IntType intType:
                    this.WriteLine($"imulq {this.variableMap[multAssignStatement.left.name]}(%rbp), %rax");
                    this.WriteLine($"movq %rax, {this.variableMap[multAssignStatement.left.name]}(%rbp)");
                    break;
                case DoubleType doubleType:
                    if (multAssignStatement.right.type is IntType)
                        this.WriteLine("cvtsi2sdq %rax, %xmm0");

                    this.WriteLine("movq %xmm0, %xmm1");
                    this.WriteLine($"mulsd {this.variableMap[multAssignStatement.left.name]}(%rbp), %xmm0");
                    this.WriteLine($"movq %xmm0, {this.variableMap[multAssignStatement.left.name]}(%rbp)");
                    break;
                default:
                    throw new ArgumentException($"MultAssign Statment not compatible with type \"{multAssignStatement.left.type.typeName}\"");
            }
        }

        private void DivAssignAsm(DivAssignStatement divAssignStatement)
        {
            this.GetAssember(divAssignStatement.right);

            switch (divAssignStatement.left.type)
            {
                case IntType intType:
                    this.WriteLine("movq %rax, %rbx");
                    this.WriteLine("movq $0, %rdx");
                    this.WriteLine($"movq {this.variableMap[divAssignStatement.left.name]}(%rbp), %rax");
                    this.WriteLine($"idivq %rbx");
                    this.WriteLine($"movq %rax, {this.variableMap[divAssignStatement.left.name]}(%rbp)");
                    break;
                case DoubleType doubleType:
                    if (divAssignStatement.right.type is IntType)
                        this.WriteLine("cvtsi2sdq %rax, %xmm0");

                    this.WriteLine("movq %xmm0, %xmm1");
                    this.WriteLine($"movq {this.variableMap[divAssignStatement.left.name]}(%rbp), %xmm0");

                    this.WriteLine("divsd %xmm1, %xmm0");
                    this.WriteLine($"movq %xmm0, {this.variableMap[divAssignStatement.left.name]}(%rbp)");
                    break;
                default:
                    throw new ArgumentException($"DivAssign Statement not compatible with type \"{divAssignStatement.left.type.typeName}\"");
            }
        }

        private void IncrementAsm(IncrementExpr increment)
        {
            this.GetAssember(increment.variable);
            string register = null;

            if (!increment.post)
            {
                if (increment.type is IntType)
                {
                    this.WriteLine("incq %rax");
                    register = "%rax";
                }
                else
                {
                    this.WriteLine("movq $1, %rax");
                    this.WriteLine("cvtsi2sdq %rax, %xmm1");
                    this.WriteLine("addsd %xmm1, %xmm0");
                    register = "%xmm0";
                }
            }
            else
            {
                if (increment.type is IntType)
                {
                    this.WriteLine("movq %rax, %rbx");
                    this.WriteLine("incq %rbx");
                    register = "%rbx";
                }
                else
                {
                    this.WriteLine("movq $1, %rax");
                    this.WriteLine("cvtsi2sdq %rax, %xmm1");
                    this.WriteLine("addsd %xmm0, %xmm1");
                    register = "%xmm1";
                }
            }

            switch (increment.variable)
            {
                case VarExpr varExpr:
                    this.WriteLine($"movq {register}, {this.variableMap[varExpr.name]}(%rbp)");
                    if (varExpr.type is DoubleType)
                        this.WriteLine("movq %xmm0, %rax");
                    break;
                case ArrayIndexing arrayIndexing:
                    if (increment.post)
                    {
                        if (arrayIndexing.type is DoubleType)
                            this.WriteLine("movq %xmm0, %rax");

                        this.WritePush();
                    }

                    if (register != "%rbx")
                        this.WriteLine($"movq {register}, %rbx");

                    this.WritePush("%rbx");
                    this.LoadArrayField(arrayIndexing);
                    this.WritePop("%rbx");
                    // move the value into the correct adresse
                    this.WriteLine("movq %rbx, (%rax)");
                    this.WriteLine("movq %rbx, %rax");

                    if (increment.post)
                        this.WritePop();
                    break;
                case StructPropertyAccess propertyAccess:
                    if (increment.post)
                    {
                        if (propertyAccess.type is DoubleType)
                            this.WriteLine("movq %xmm0, %rax");

                        this.WritePush();
                    }

                    if (register != "%rbx")
                        this.WriteLine($"movq {register}, %rbx");

                    this.WritePush("%rbx");
                    this.LoadStructProperty(propertyAccess);
                    this.WritePop("%rbx");
                    this.WriteLine("movq %rbx, (%rax)");
                    this.WriteLine("movq %rbx, %rax");

                    if (increment.post)
                        this.WritePop();
                    break;
            }

            if (increment.type is DoubleType)
                this.WriteLine("movq %rax, %xmm0");
        }

        private void DecrementAsm(DecrementExpr decrement)
        {
            this.GetAssember(decrement.variable);
            string register = null;

            if (!decrement.post)
            {
                if (decrement.type is IntType)
                {
                    this.WriteLine("decq %rax");
                    register = "%rax";
                }
                else
                {
                    this.WriteLine("movq $-1, %rax");
                    this.WriteLine("cvtsi2sdq %rax, %xmm1");
                    this.WriteLine("addsd %xmm1, %xmm0");
                    register = "%xmm0";
                }
            }
            else
            {
                if (decrement.type is IntType)
                {
                    this.WriteLine("movq %rax, %rbx");
                    this.WriteLine("decq %rbx");
                    register = "%rbx";
                }
                else
                {
                    this.WriteLine("movq $-1, %rax");
                    this.WriteLine("cvtsi2sdq %rax, %xmm1");
                    this.WriteLine("addsd %xmm0, %xmm1");
                    register = "%xmm1";
                }
            }

            switch (decrement.variable)
            {
                case VarExpr varExpr:
                    this.WriteLine($"movq {register}, {variableMap[varExpr.name]}(%rbp)");

                    if (varExpr.type is DoubleType)
                        this.WriteLine("movq %xmm0, %rax");
                    break;
                case ArrayIndexing arrayIndexing:
                    if (decrement.post)
                    {
                        if (arrayIndexing.type is DoubleType)
                            this.WriteLine("movq %xmm0, %rax");

                        this.WritePush();
                    }

                    if (register != "%rbx")
                        this.WriteLine($"movq {register}, %rbx");

                    this.WritePush("%rbx");
                    this.LoadArrayField(arrayIndexing);
                    this.WritePop("%rbx");
                    this.WriteLine("movq %rbx, (%rax)");
                    this.WriteLine("movq %rbx, %rax");

                    if (decrement.post)
                        this.WritePop();
                    break;
                case StructPropertyAccess propertyAccess:
                    if (decrement.post)
                    {
                        if (propertyAccess.type is DoubleType)
                            this.WriteLine("movq %xmm0, %rax");

                        this.WritePush();
                    }

                    if (register != "%rbx")
                        this.WriteLine($"movq {register}, %rbx");

                    this.WritePush("%rbx");
                    this.LoadStructProperty(propertyAccess);
                    this.WritePop("%rbx");
                    this.WriteLine("movq %rbx, (%rax)");
                    this.WriteLine("movq %rbx, %rax");

                    if (decrement.post)
                        this.WritePop();
                    break;
            }

            if (decrement.type is DoubleType)
                this.WriteLine("movq %rax, %xmm0");
        }

        private void NotExprAsm(NotExpr notExpr)
        {
            this.GetAssember(notExpr.value);
            this.WriteLine("cmpq $0, %rax");
            this.WriteLine("sete %al");
            this.WriteLine("movzbl %al, %rax");
        }

        private void AndExprAsm(AndExpr andExpr)
        {
            this.GetAssember(andExpr.left);

            this.WriteLine("cmpq $0, %rax");
            this.WriteLine($"je .L{this.labelCount}");

            this.GetAssember(andExpr.right);

            this.WriteLine("cmpq $0, %rax");
            this.WriteLine($"je .L{this.labelCount}");
            this.WriteLine("movq $1, %rax");
            this.WriteLine($"jmp .L{this.labelCount + 1}");


            this.depth -= 1;
            this.WriteLine($".L{this.labelCount++}:");
            this.depth += 1;
            this.WriteLine("movq $0, %rax");
            this.depth -= 1;
            this.WriteLine($".L{this.labelCount++}:");
            this.depth += 1;
        }

        private void OrExprAsm(OrExpr orExpr)
        {
            this.GetAssember(orExpr.left);

            this.WriteLine("cmpq $1, %rax");
            this.WriteLine($"je .L{this.labelCount}");

            this.GetAssember(orExpr.right);
            this.WriteLine("cmpq $1, %rax");
            this.WriteLine($"je .L{this.labelCount}");
            this.WriteLine("movq $0, %rax");
            this.WriteLine($"jmp .L{this.labelCount + 1}");

            this.depth -= 1;
            this.WriteLine($".L{this.labelCount++}:");
            this.depth += 1;
            this.WriteLine("movq $1, %rax");

            this.depth -= 1;
            this.WriteLine($".L{this.labelCount++}:");
            this.depth += 1;
        }

        private void NotEqualExprAsm(NotEqualExpr notEqualExpr)
        {
            this.GetAssember(notEqualExpr.left);

            switch (notEqualExpr.left.type)
            {
                case IntType it:
                    this.WritePush();

                    this.GetAssember(notEqualExpr.right);

                    if (notEqualExpr.right.type is DoubleType)
                    {
                        this.WritePop();
                        this.WriteLine("cvtsi2sd %rax, %xmm1");
                        this.WriteLine("ucomisd %xmm0, %xmm1");
                    }
                    else
                    {
                        this.WritePop();
                        this.WriteLine("cmpq %rax, %rbx");
                    }

                    break;
                case DoubleType doubleType:
                    this.WritePush();

                    this.GetAssember(notEqualExpr.right);

                    if (notEqualExpr.right.type is IntType)
                        this.WriteLine("cvtsi2sd %rax, %xmm0");

                    this.WritePop();
                    this.WriteLine("movq %rax, %xmm1");
                    this.WriteLine("ucomisd %xmm0, %xmm1");

                    break;
                case BooleanType booleanType:
                    this.WritePush();

                    this.GetAssember(notEqualExpr.right);

                    this.WritePop("%rbx");
                    this.WriteLine("cmpq %rax, %rbx");

                    break;
            }

            this.WriteLine("setne %al");
            this.WriteLine("movzbl %al, %rax");
        }

        private void PrintStatementAsm(PrintStatement print)
        {
            this.WriteString(print.value);

            this.GetAssember(print.value);

            switch (print.value.type)
            {
                case IntType it:
                    this.WriteLine("movq %rax, %rsi");
                    this.WriteLine($"leaq .LS{this.stringLabelMap["int"]}(%rip), %rdi");
                    this.WriteLine("movl $0, %eax");

                    break;
                case DoubleType dt:
                    this.WriteLine($"leaq .LS{this.stringLabelMap["double"]}(%rip), %rdi");
                    this.WriteLine("movl $1, %eax");

                    break;
                case BooleanType bt:
                    this.WriteLine("movq %rax, %rsi");
                    this.WriteLine($"leaq .LS{this.stringLabelMap["int"]}(%rip), %rdi");
                    this.WriteLine("movl $0, %eax");

                    break;
            }

            if (this.stackCounter % 16 == 0)
                this.WritePush("$0");


            this.WriteLine("call printf@PLT");
        }

        private void RefTypeCreationStatementAsm(RefTypeCreationStatement refTypeCreation)
        {
            switch (refTypeCreation.createdReftype)
            {
                case AST.Array array:
                    this.GetAssember(array.size);

                    this.WriteLine("movq $8, %rbx");
                    this.WriteLine("imulq %rbx, %rax");

                    this.WriteLine("movq %rax, %rdi");
                    break;
                case Struct @struct:
                    StructDefinition structDef = IAST.structs[@struct.name];
                    int size = structDef.GetSize();

                    this.WriteLine($"movq ${size}, %rdi");
                    break;
                default:
                    throw new NotImplementedException("Omega NASA");
            }

            if (this.stackCounter % 16 == 0)
                this.WritePush("$0");

            this.WriteLine("call malloc@PLT");
        }

        private void ArrayIndexingAsm(ArrayIndexing arrayIndexing)
        {
            this.LoadArrayField(arrayIndexing);
            if (arrayIndexing.type is DoubleType)
                this.WriteLine("movq (%rax), %xmm0");
            else
                this.WriteLine("movq (%rax), %rax");
        }

        private void AssignArrayFieldAsm(AssignArrayField assignArray)
        {
            // calculate the value
            this.GetAssember(assignArray.value);

            // save the value on the stack
            if (assignArray.value.type is DoubleType)
                this.WriteLine("movq %xmm0, %rax");

            this.WritePush();

            // load the value of the variable
            this.LoadArrayField(assignArray.arrayIndex);
            this.WritePop("%rbx");
            // save the value in the array
            this.WriteLine("movq %rbx, (%rax)");
        }

        private void DestructionStatementAsm(DestructionStatement destruction)
        {
            this.GetAssember(destruction.refType);
            this.WriteLine("movq %rax, %rdi");

            if (this.stackCounter % 16 == 0)
                this.WriteLine("push $0");

            this.WriteLine("call free@PLT");
        }

        private void NullLitAsm(NullLit nullLit)
        {
            this.WriteLine("movq $0, %rax");
        }

        private void StructPropertyAccessAsm(StructPropertyAccess structPropertyAccess)
        {
            this.LoadStructProperty(structPropertyAccess);
            // write the value in the correct register
            if (structPropertyAccess.type is DoubleType)
                this.WriteLine("movq (%rax), %xmm0");
            else
                this.WriteLine("movq (%rax), %rax");
        }

        private void AssignStructPropertyAsm(AssignStructProperty assignStruct)
        {
            // calculate the new value
            this.GetAssember(assignStruct.val);

            // if it is a double, move it into %rax
            if (assignStruct.val.type is DoubleType)
                this.WriteLine("movq %xmm0, %rax");

            // push the value on the stack
            this.WritePush();

            // get the index of the property
            this.LoadStructProperty(assignStruct.structProp);
            // get the value from the stack
            this.WritePop("%rbx");
            // save the calculated value
            this.WriteLine("movq %rbx, (%rax)");
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
                if (args[i].type is IntType || args[i].type is BooleanType)
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
                if (args[i].type is IntType || args[i].type is BooleanType)
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

                if (arg.type is BooleanType || arg.type is IntType)
                    intArgCount++;
            }
        }

        private int GetNextIntArg(List<InstantiationStatement> args, int startIndex)
        {
            for (int i = startIndex + 1; i < args.Count; i++)
            {
                if (args[i].type is IntType || args[i].type is BooleanType)
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

        private void LoadStructProperty(StructPropertyAccess structProperty)
        {
            StructType st = structProperty.structRef.type as StructType;
            var structDef = IAST.structs[st.structName];
            int propIndex = structDef.properties.FindIndex(sp => sp.name == structProperty.propName);

            this.GetAssember(structProperty.structRef);
            this.WriteLine($"addq ${propIndex * 8}, %rax");
        }
    }
}