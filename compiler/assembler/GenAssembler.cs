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
        private int labelCount = 0;
        private int doubleNumbersLabelCount = 0;
        private string[] integerRegisters = { "%rdi", "%rsi", "%rdx", "%rcx", "%r8", "%r9" };
        private string[] doubleRegisters = { "%xmm0", "%xmm1", "%xmm2", "%xmm3", "%xmm4", "%xmm5", "%xmm6", "%xmm7" };
        private Dictionary<string, FunctionAsm> functionMap = new Dictionary<string, FunctionAsm>();
        private Dictionary<double, int> doubleMap = new Dictionary<double, int>();
        private Dictionary<string, int> variableMap;
        private int localVariablePointer = 0;

        public void PrintAssember()
        {
            Console.WriteLine(this.sb.ToString());
            if (this.doubleNumbers.Length > 0)
                Console.WriteLine(this.doubleNumbers.ToString());
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
                default:
                    throw new NotImplementedException($"Assembler generation not implemented for {astNode.type.typeName}");
            }
        }

        public void GenerateAssember(IAST astNode)
        {
            this.GetAssember(astNode);
            this.PrintAssember();
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
            
            if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                indexOfSlash = filePath.IndexOf('\\');
            else
                indexOfSlash = filePath.IndexOf('/');
            
            if(indexOfSlash == -1)
                fileName = filePath;
            else
                fileName = filePath.Substring(indexOfSlash + 1, filePath.Length - (indexOfSlash + 1));
            this.InitializeFile(fileName);

            this.GetAssember(astNode);

            string fileContent = this.sb.ToString();

            if(this.doubleNumbers.Length > 0)
                fileContent = fileContent + this.doubleNumbers.ToString();
            
            fileName = fileName.Substring(0, fileName.IndexOf(".ll")) + ".S";

            using(StreamWriter sw = File.CreateText(fileName))
            {
                sw.Write(fileContent);
            }

            Console.WriteLine(fileContent);
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
                    this.WriteLine("pushq %rax");
                    this.GetAssember(addExpr.left);

                    if (addExpr.left.type is IntType)
                        this.WriteLine("cvtsi2sdq %rax, %xmm0");

                    this.WriteLine("popq %rax");
                    this.WriteLine("movq %rax, %xmm1");
                    this.WriteLine("addsd %xmm1, %xmm0");

                    break;
                case IntType intType:
                    this.GetAssember(addExpr.right);
                    this.WriteLine("pushq %rax");
                    this.GetAssember(addExpr.left);

                    if (addExpr.left.type is DoubleType)
                    {
                        this.WriteLine("popq %rax");
                        this.WriteLine("cvtsi2sdq %rax, %xmm1");
                        this.WriteLine("addsd %xmm1, %xmm0");
                    }
                    else
                    {
                        this.WriteLine("popq %rbx");
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
                    this.WriteLine("pushq %rax");
                    this.GetAssember(subExpr.left);

                    if (subExpr.left is IntLit)
                        this.WriteLine("cvtsi2sdq %rax, %xmm0");

                    this.WriteLine("popq %rax");
                    this.WriteLine("movq %rax, %xmm1");
                    this.WriteLine("subsd %xmm1, %xmm0");

                    break;
                case IntType intType:
                    this.GetAssember(subExpr.right);
                    this.WriteLine("pushq %rax");
                    this.GetAssember(subExpr.left);

                    if (subExpr.left is DoubleLit)
                    {
                        this.WriteLine("popq %rax");
                        this.WriteLine("cvtsi2sdq %rax, %xmm1");
                        this.WriteLine("subsd %xmm1, %xmm0");
                    }
                    else
                    {
                        this.WriteLine("popq %rbx");
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
                    this.WriteLine("pushq %rax");
                    this.GetAssember(multExpr.left);

                    if (multExpr.left.type is IntType)
                        this.WriteLine("cvtsi2sdq %rax, %xmm0");

                    this.WriteLine("popq %rax");
                    this.WriteLine("movq %rax, %xmm1");
                    this.WriteLine("mulsd %xmm1, %xmm0");

                    break;
                case IntType intType:
                    this.GetAssember(multExpr.right);
                    this.WriteLine("pushq %rax");
                    this.GetAssember(multExpr.left);

                    if (multExpr.left is DoubleLit)
                    {
                        this.WriteLine("popq %rax");
                        this.WriteLine("cvtsi2sdq %rax, %xmm1");
                        this.WriteLine("mulsd %xmm1, %xmm0");
                    }
                    else
                    {
                        this.WriteLine("popq %rbx");
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
                    this.WriteLine("pushq %rax");
                    this.GetAssember(divExpr.left);

                    if (divExpr.left is IntLit)
                        this.WriteLine("cvtsi2sdq %rax, %xmm0");

                    this.WriteLine("popq %rax");
                    this.WriteLine("movq %rax, %xmm1");
                    this.WriteLine("divsd %xmm1, %xmm0");

                    break;
                case IntType intType:
                    this.GetAssember(divExpr.right);
                    this.WriteLine("pushq %rax");
                    this.GetAssember(divExpr.left);

                    if (divExpr.left is DoubleLit)
                    {
                        this.WriteLine("popq %rax");
                        this.WriteLine("cvtsi2sdq %rax, %xmm1");
                        this.WriteLine("divsd %xmm1, %xmm0");
                    }
                    else
                    {
                        this.WriteLine("popq %rbx");
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
                    this.WriteLine("pushq %rax");

                    this.GetAssember(greaterExpr.left);

                    if (greaterExpr.left.type is IntType)
                        this.WriteLine("cvtsi2sdq %rax, %xmm0");

                    this.WriteLine("popq %rax");
                    this.WriteLine("movq %rax, %xmm1");
                    this.WriteLine("ucomisd %xmm1, %xmm0");

                    if (greaterExpr.equal)
                        this.WriteLine("setae %al");
                    else
                        this.WriteLine("seta %al");

                    break;
                case IntType intType:
                    this.GetAssember(greaterExpr.right);
                    this.WriteLine("pushq %rax");

                    this.GetAssember(greaterExpr.left);

                    if (greaterExpr.left.type is DoubleType)
                    {
                        this.WriteLine("popq %rax");
                        this.WriteLine("cvtsi2sdq %rax, %xmm1");
                        this.WriteLine("ucomisd %xmm1, %xmm0");

                        if (greaterExpr.equal)
                            this.WriteLine("setae %al");
                        else
                            this.WriteLine("seta %al");
                    }
                    else
                    {
                        this.WriteLine("popq %rbx");
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
                    this.WriteLine("pushq %rax");

                    this.GetAssember(lessExpr.left);

                    if (lessExpr.left.type is IntType)
                        this.WriteLine("cvtsi2sdq %rax, %xmm0");

                    this.WriteLine("popq %rax");
                    this.WriteLine("movq %rax, %xmm1");
                    this.WriteLine("ucomisd %xmm1, %xmm0");

                    if (lessExpr.equal)
                        this.WriteLine("setbe %al");
                    else
                        this.WriteLine("setb %al");

                    break;
                case IntType intType:
                    this.GetAssember(lessExpr.right);
                    this.WriteLine("pushq %rax");

                    this.GetAssember(lessExpr.left);

                    if (lessExpr.left.type is DoubleType)
                    {
                        this.WriteLine("popq %rax");
                        this.WriteLine("cvtsi2sdq %rax, %xmm1");
                        this.WriteLine("ucomisd %xmm1, %xmm0");

                        if (lessExpr.equal)
                            this.WriteLine("setbe %al");
                        else
                            this.WriteLine("setb %al");
                    }
                    else
                    {
                        this.WriteLine("popq %rbx");
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
                    this.WriteLine("pushq %rax");

                    this.GetAssember(equalityExpr.left);

                    if (equalityExpr.left.type is DoubleType)
                        this.WriteLine("movq %xmm0, %rax");

                    break;
                case IntType intType:
                    this.GetAssember(equalityExpr.right);
                    this.WriteLine("pushq %rax");

                    this.GetAssember(equalityExpr.left);

                    if (equalityExpr.left.type is DoubleType)
                    {
                        this.WriteLine("popq %rax");
                        this.WriteLine("cvtsi2sdq %rax, %xmm1");
                        this.WriteLine("movq %xmm1, %rax");
                        this.WriteLine("pushq %rax");
                        this.WriteLine("movq %xmm0, %rax");
                    }

                    break;
                default:
                    throw new InvalidOperationException($"Can not use \"greater\" operator with {equalityExpr.right.type}");
            }

            this.WriteLine("popq %rbx");
            this.WriteLine("cmpq %rbx, %rax");
            this.WriteLine("sete %al");

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
            this.WriteLine("popq %rbp");
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

            this.depth += 1;

            this.WriteLine($".global {funDef.name}");
            this.WriteLine($".type {funDef.name}, @function");

            this.depth -= 1;

            this.WriteLine($"{funDef.name}:");

            this.depth += 1;

            this.WriteLine("pushq %rbp");
            this.WriteLine("movq %rsp, %rbp");

            int offSet = funDef.GetLocalVariables() * 8;

            if (offSet > 0)
                this.WriteLine($"subq ${offSet}, %rsp");

            // push all argument-registers onto the stack
            this.ArgumentTypeCount(funDef.args, out int intArgCount, out int doubleArgCount);
            int index = Math.Min(intArgCount, this.integerRegisters.Length);
            int lastFound = -1;

            for (int i = 0; i < index; i++)
            {
                this.WriteLine($"pushq {this.integerRegisters[i]}");
                offSet -= 8;
                lastFound = this.GetNextIntArg(funDef.args, lastFound);
                this.variableMap[funDef.args[lastFound].name] = offSet;
            }

            index = Math.Min(doubleArgCount, this.doubleRegisters.Length);
            lastFound = -1;

            for (int i = 0; i < index; i++)
            {
                this.WriteLine($"movq {this.doubleRegisters[i]}, %rax");
                this.WriteLine($"pushq %rax");
                offSet -= 8;
                lastFound = this.GetNextDoubleArg(funDef.args, lastFound);
                this.variableMap[funDef.args[lastFound].name] = offSet;
            }

            this.GetAssember(funDef.body);

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
                    this.WriteLine($"pushq %rax");
                    functionAsm.usedDoubleRegisters++;
                }
            }

            // move the previously pushed double arguments into the double registers
            for (int i = 0; i < functionAsm.usedDoubleRegisters; i++)
            {
                this.WriteLine("popq %rax");
                this.WriteLine($"movq %rax, {doubleRegisters[i]}");
            }

            this.WriteLine($"call {functionCall.name}");
        }

        private void VariableAsm(VarExpr varExpr)
        {
            if(varExpr.type is DoubleType)
                this.WriteLine($"movq {this.variableMap[varExpr.name]}(%rbp), %xmm0");
            
            if(varExpr.type is BooleanType || varExpr.type is IntType)
                this.WriteLine($"movq {this.variableMap[varExpr.name]}(%rbp), %rax");
            else
                throw new ArgumentException($"Varibale type {varExpr.type.typeName} is not supported");
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
                        integerOverflowPosition = integerOverflowPosition == Int32.MaxValue ? i : doubleOverflowPosition;
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
    }
}