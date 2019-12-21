using System;
using System.Text;
using System.Collections.Generic;
using ll.AST;
using ll.type;

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
        private Dictionary<double, int> doubleMap = new Dictionary<double, int>();

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
                default:
                    throw new NotImplementedException($"Assembler generation not implemented for {astNode.type.typeName}");
            }
        }

        private void InitializeFile()
        {
            this.depth += 1;

            this.WriteLine(".file testFile");
            this.WriteLine(".text");

            this.depth -= 1;
        }

        public void GenerateAssembler(IAST astNode)
        {
            //this.InitializeFile();
            this.GetAssember(astNode);
        }

        public void WriteToFile(IAST astNode)
        {
            throw new NotImplementedException();
        }

        private void IntLitAsm(IntLit intLit)
        {
            this.WriteLine($"movq ${intLit.n}, %rax");
        }

        private void DoubleLitAsm(DoubleLit doubleLit)
        {
            this.WriteDoubleValue(doubleLit);
            this.WriteLine($"movq .LD{this.doubleMap[doubleLit.n ?? 0]}(%rip), %rax");
        }

        private void BoolLitAsm(BoolLit boolLit)
        {
            this.WriteLine($"movq ${((boolLit.value ?? false) ? 1 : 0)}, %rax");
        }

        private void AddExprAsm(AddExpr addExpr)
        {
            this.depth += 1;

            switch (addExpr.right)
            {
                case DoubleLit doubleLit:
                    this.GetAssember(doubleLit);
                    this.WriteLine("pushq %rax");
                    this.GetAssember(addExpr.left);

                    if (addExpr.left is IntLit)
                        this.WriteLine("cvtsi2sdq %rax, %xmm0");
                    else
                        this.WriteLine("movq %rax, %xmm0");

                    this.WriteLine("popq %rax");
                    this.WriteLine("movq %rax, %xmm1");
                    this.WriteLine("addsd %xmm1, %xmm0");

                    break;
                case IntLit intLit:
                    this.GetAssember(intLit);
                    this.WriteLine("pushq %rax");
                    this.GetAssember(addExpr.left);

                    if (addExpr.left is DoubleLit)
                    {
                        this.WriteLine("movq %rax, %xmm1");
                        this.WriteLine("popq %rax");
                        this.WriteLine("cvtsi2sdq %rax, %xmm0");
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

            this.depth -= 1;
        }

        private void SubExprAsm(SubExpr subExpr)
        {
            this.depth += 1;

            switch (subExpr.right)
            {
                case DoubleLit doubleLit:
                    this.GetAssember(doubleLit);
                    this.WriteLine("pushq %rax");
                    this.GetAssember(subExpr.left);

                    if (subExpr.left is IntLit)
                        this.WriteLine("cvtsi2sdq %rax, %xmm0");
                    else
                        this.WriteLine("movq %rax, %xmm0");

                    this.WriteLine("popq %rax");
                    this.WriteLine("movq %rax, %xmm1");
                    this.WriteLine("subsd %xmm1, %xmm0");

                    break;
                case IntLit intLit:
                    this.GetAssember(intLit);
                    this.WriteLine("pushq %rax");
                    this.GetAssember(subExpr.left);

                    if (subExpr.left is DoubleLit)
                    {
                        this.WriteLine("movq %rax, %xmm1");
                        this.WriteLine("popq %rax");
                        this.WriteLine("cvtsi2sdq %rax, %xmm0");
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

            this.depth -= 1;
        }

        private void MultExprAsm(MultExpr multExpr)
        {
            this.depth += 1;

            switch (multExpr.right)
            {
                case DoubleLit doubleLit:
                    this.GetAssember(doubleLit);
                    this.WriteLine("pushq %rax");
                    this.GetAssember(multExpr.left);

                    if (multExpr.left is IntLit)
                        this.WriteLine("cvtsi2sdq %rax, %xmm0");
                    else
                        this.WriteLine("movq %rax, %xmm0");

                    this.WriteLine("popq %rax");
                    this.WriteLine("movq %rax, %xmm1");
                    this.WriteLine("mulsd %xmm1, %xmm0");

                    break;
                case IntLit intLit:
                    this.GetAssember(intLit);
                    this.WriteLine("pushq %rax");
                    this.GetAssember(multExpr.left);

                    if (multExpr.left is DoubleLit)
                    {
                        this.WriteLine("movq %rax, %xmm1");
                        this.WriteLine("popq %rax");
                        this.WriteLine("cvtsi2sdq %rax, %xmm0");
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

            this.depth -= 1;
        }

        private void DivExprAsm(DivExpr divExpr)
        {
            this.depth += 1;

            switch (divExpr.right)
            {
                case DoubleLit doubleLit:
                    this.GetAssember(doubleLit);
                    this.WriteLine("pushq %rax");
                    this.GetAssember(divExpr.left);

                    if (divExpr.left is IntLit)
                        this.WriteLine("cvtsi2sdq %rax, %xmm0");
                    else
                        this.WriteLine("movq %rax, %xmm0");

                    this.WriteLine("popq %rax");
                    this.WriteLine("movq %rax, %xmm1");
                    this.WriteLine("divsd %xmm1, %xmm0");

                    break;
                case IntLit intLit:
                    this.GetAssember(intLit);
                    this.WriteLine("pushq %rax");
                    this.GetAssember(divExpr.left);

                    if (divExpr.left is DoubleLit)
                    {
                        this.WriteLine("movq %rax, %xmm1");
                        this.WriteLine("popq %rax");
                        this.WriteLine("cvtsi2sdq %rax, %xmm0");
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

            this.depth -= 1;
        }

        private void GreaterExprAsm(GreaterExpr greaterExpr)
        {
            this.depth += 1;

            this.GetAssember(greaterExpr.right);
            this.WriteLine("pushq %rax");
            this.GetAssember(greaterExpr.left);
            this.WriteLine("popq %rbx");
            this.WriteLine("cmpq %rbx, %rax");

            if (greaterExpr.equal)
                this.WriteLine("setge %al");
            else
                this.WriteLine("setg %al");

            this.WriteLine("movzbl %al, %rax");

            this.depth -= 1;
        }

        private void LessExprAsm(LessExpr lessExpr)
        {
            this.depth += 1;

            this.GetAssember(lessExpr.right);
            this.WriteLine("pushq %rax");
            this.GetAssember(lessExpr.left);
            this.WriteLine("popq %rbx");
            this.WriteLine("cmpq %rbx, %rax");

            if (lessExpr.equal)
                this.WriteLine("setle %al");
            else
                this.WriteLine("setl %al");

            this.WriteLine("movzbl %al, %rax");

            this.depth -= 1;
        }

        private void EqualityExprAsm(EqualityExpr equalityExpr)
        {
            this.depth += 1;

            this.GetAssember(equalityExpr.right);
            this.WriteLine("pushq %rax");
            this.GetAssember(equalityExpr.left);
            this.WriteLine("popq %rbx");
            this.WriteLine("cmp %rax, %rbx");
            this.WriteLine("sete %al");
            this.WriteLine("movzbl %al, %rax");

            this.depth -= 1;
        }

        private void BlockStatementAsm(BlockStatement blockStatement)
        {
            foreach (var comp in blockStatement.body)
                this.GetAssember(comp);
        }

        private void ReturnStatementAsm(ReturnStatement returnStatement)
        {
            this.depth += 1;

            this.WriteLine("movq %rbp, %rsp");
            this.WriteLine("popq %rbp");
            this.WriteLine("ret");

            this.depth -= 1;
        }

        // TODO move arguments from registers onto stack; safe position in current env
        // TODO allocate space on stack for local variables
        // TODO add return if void function; rax, eax and xmm0 should be empty
        private void FunctionDefinitionAsm(FunctionDefinition funDef)
        {
            this.depth += 1;

            this.WriteLine($".global {funDef.name}");
            this.WriteLine($".type {funDef.name}, @function");

            this.depth -= 1;

            this.WriteLine($"{funDef.name}:");

            this.depth += 1;

            this.WriteLine("pushq %rbp");
            this.WriteLine("movq %rsp, %rbp");
        }

        // TODO move arguments in registers and stack
        // TODO add new environment, where position of additional arguments getting saved
        private void FunctionCallAsm(FunctionCall functionCall)
        {
            throw new NotImplementedException();
        }

        // TODO push arguments into the correct registers, depending on wether they are floating point numbers or not
        private void ArgumentsAsm(List<InstantiationStatement> args)
        {
            throw new NotImplementedException();
        }

        private void WriteLine(string op)
        {
            for (int i = 0; i < this.depth; i++)
                this.sb.Append(this.indent);

            var indexOfSpace = op.IndexOf(' ');
            var first = op.Substring(0, indexOfSpace);
            first = first.PadRight(7, ' ');

            this.sb.Append(first);
            this.sb.Append(op.Substring(indexOfSpace));

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
    }
}