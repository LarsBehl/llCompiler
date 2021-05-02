using System;

using LL.AST;
using LL.Types;

namespace LL.CodeGeneration
{
    public partial class AssemblerGenerator
    {
        private void AddExprAsm(AddExpr addExpr)
        {
            switch (addExpr.Right.Type)
            {
                case DoubleType doubleType:
                    this.GetAssember(addExpr.Right);
                    this.WriteLine("movq %xmm0, %rax");
                    this.WritePush();
                    this.GetAssember(addExpr.Left);

                    // convert int to double
                    if (addExpr.Left.Type is IntType)
                        this.WriteLine("cvtsi2sdq %rax, %xmm0");

                    this.WritePop();
                    this.WriteLine("movq %rax, %xmm1");
                    this.WriteLine("addsd %xmm1, %xmm0");

                    break;
                case IntType intType:
                    this.GetAssember(addExpr.Right);
                    this.WritePush();
                    this.GetAssember(addExpr.Left);

                    if (addExpr.Left.Type is DoubleType)
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
                    throw new InvalidOperationException($"Could not perform addition on type {addExpr.Type.typeName}");
            }
        }

        private void SubExprAsm(SubExpr subExpr)
        {
            switch (subExpr.Right.Type)
            {
                case DoubleType doubleType:
                    this.GetAssember(subExpr.Right);
                    this.WriteLine("movq %xmm0, %rax");
                    this.WritePush();
                    this.GetAssember(subExpr.Left);

                    // convert int to double
                    if (subExpr.Left.Type is IntType)
                        this.WriteLine("cvtsi2sdq %rax, %xmm0");

                    this.WritePop();
                    this.WriteLine("movq %rax, %xmm1");
                    this.WriteLine("subsd %xmm1, %xmm0");

                    break;
                case IntType intType:
                    this.GetAssember(subExpr.Right);
                    this.WritePush();
                    this.GetAssember(subExpr.Left);

                    if (subExpr.Left.Type is DoubleType)
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
                    throw new InvalidOperationException($"Could not perform addition on type {subExpr.Type.typeName}");
            }
        }

        private void MultExprAsm(MultExpr multExpr)
        {
            switch (multExpr.Right.Type)
            {
                case DoubleType doubleType:
                    this.GetAssember(multExpr.Right);
                    this.WriteLine("movq %xmm0, %rax");
                    this.WritePush();
                    this.GetAssember(multExpr.Left);

                    // convert int to double
                    if (multExpr.Left.Type is IntType)
                        this.WriteLine("cvtsi2sdq %rax, %xmm0");

                    this.WritePop();
                    this.WriteLine("movq %rax, %xmm1");
                    this.WriteLine("mulsd %xmm1, %xmm0");

                    break;
                case IntType intType:
                    this.GetAssember(multExpr.Right);
                    this.WritePush();
                    this.GetAssember(multExpr.Left);

                    if (multExpr.Left.Type is DoubleType)
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
                    throw new InvalidOperationException($"Could not perform addition on type {multExpr.Type.typeName}");
            }
        }

        private void DivExprAsm(DivExpr divExpr)
        {
            switch (divExpr.Right.Type)
            {
                case DoubleType doubleType:
                    this.GetAssember(divExpr.Right);
                    this.WriteLine("movq %xmm0, %rax");
                    this.WritePush();
                    this.GetAssember(divExpr.Left);

                    // convert int to double
                    if (divExpr.Left.Type is IntType)
                        this.WriteLine("cvtsi2sdq %rax, %xmm0");

                    this.WritePop();
                    this.WriteLine("movq %rax, %xmm1");
                    this.WriteLine("divsd %xmm1, %xmm0");

                    break;
                case IntType intType:
                    this.GetAssember(divExpr.Right);
                    this.WritePush();
                    this.GetAssember(divExpr.Left);

                    if (divExpr.Left.Type is DoubleType)
                    {
                        this.WritePop();
                        // convert int to double
                        this.WriteLine("cvtsi2sdq %rax, %xmm1");
                        this.WriteLine("divsd %xmm1, %xmm0");
                    }
                    else
                    {
                        this.WritePop("%rbx");
                        // clear %rdx as idiv divides rdx:rax by specified register
                        this.WriteLine("cqto");

                        this.WriteLine("idivq %rbx");
                    }
                    break;
                default:
                    throw new InvalidOperationException($"Could not perform addition on type {divExpr.Type.typeName}");
            }
        }

        private void GreaterExprAsm(GreaterExpr greaterExpr)
        {
            switch (greaterExpr.Right.Type)
            {
                case DoubleType doubleType:
                    this.GetAssember(greaterExpr.Right);
                    this.WriteLine("movq %xmm0, %rax");
                    this.WritePush();

                    this.GetAssember(greaterExpr.Left);

                    // convert int to double
                    if (greaterExpr.Left.Type is IntType)
                        this.WriteLine("cvtsi2sdq %rax, %xmm0");

                    this.WritePop();
                    this.WriteLine("movq %rax, %xmm1");
                    this.WriteLine("ucomisd %xmm1, %xmm0");

                    if (greaterExpr.Equal)
                        this.WriteLine("setae %al");
                    else
                        this.WriteLine("seta %al");

                    break;
                case IntType intType:
                    this.GetAssember(greaterExpr.Right);
                    this.WritePush();

                    this.GetAssember(greaterExpr.Left);

                    if (greaterExpr.Left.Type is DoubleType)
                    {
                        this.WritePop();
                        // convert int to double
                        this.WriteLine("cvtsi2sdq %rax, %xmm1");
                        this.WriteLine("ucomisd %xmm1, %xmm0");

                        if (greaterExpr.Equal)
                            this.WriteLine("setae %al");
                        else
                            this.WriteLine("seta %al");
                    }
                    else
                    {
                        this.WritePop("%rbx");
                        this.WriteLine("cmpq %rbx, %rax");

                        if (greaterExpr.Equal)
                            this.WriteLine("setge %al");
                        else
                            this.WriteLine("setg %al");
                    }

                    break;
                default:
                    throw new InvalidOperationException($"Can not use \"greater\" operator with {greaterExpr.Right.Type}");
            }

            this.WriteLine("movzbl %al, %rax");
        }

        private void LessExprAsm(LessExpr lessExpr)
        {
            switch (lessExpr.Right.Type)
            {
                case DoubleType doubleType:
                    this.GetAssember(lessExpr.Right);
                    this.WriteLine("movq %xmm0, %rax");
                    this.WritePush();

                    this.GetAssember(lessExpr.Left);

                    // convert int to double
                    if (lessExpr.Left.Type is IntType)
                        this.WriteLine("cvtsi2sdq %rax, %xmm0");

                    this.WritePop();
                    this.WriteLine("movq %rax, %xmm1");

                    // compare the two values
                    // it is strange that, when one of the values was converted from int
                    // the order has to change
                    this.WriteLine("ucomisd %xmm1, %xmm0");

                    if (lessExpr.Equal)
                        this.WriteLine("setbe %al");
                    else
                        this.WriteLine("setb %al");

                    break;
                case IntType intType:
                    this.GetAssember(lessExpr.Right);
                    this.WritePush();

                    this.GetAssember(lessExpr.Left);

                    if (lessExpr.Left.Type is DoubleType)
                    {
                        this.WritePop();
                        // convert int to double
                        this.WriteLine("cvtsi2sdq %rax, %xmm1");
                        // compare the two values
                        this.WriteLine("ucomisd %xmm1, %xmm0");

                        if (lessExpr.Equal)
                            this.WriteLine("setbe %al");
                        else
                            this.WriteLine("setb %al");
                    }
                    else
                    {
                        this.WritePop("%rbx");
                        this.WriteLine("cmpq %rbx, %rax");

                        if (lessExpr.Equal)
                            this.WriteLine("setle %al");
                        else
                            this.WriteLine("setl %al");
                    }

                    break;
                default:
                    throw new InvalidOperationException($"Can not use \"less\" operator with {lessExpr.Right.Type}");
            }

            this.WriteLine("movzbl %al, %rax");
        }

        private void EqualityExprAsm(EqualityExpr equalityExpr)
        {
            switch (equalityExpr.Right.Type)
            {
                case DoubleType doubleType:
                    this.GetAssember(equalityExpr.Right);
                    this.WriteLine("movq %xmm0, %rax");
                    this.WritePush();

                    this.GetAssember(equalityExpr.Left);

                    // convert int to double
                    if (equalityExpr.Left.Type is IntType)
                        this.WriteLine("cvtsi2sdq %rax, %xmm0");

                    this.WritePop();
                    this.WriteLine("movq %rax, %xmm1");
                    this.WriteLine("ucomisd %xmm0, %xmm1");
                    this.WriteLine("setz %al");

                    break;
                case IntType intType:
                    this.GetAssember(equalityExpr.Right);
                    this.WritePush();

                    this.GetAssember(equalityExpr.Left);

                    if (equalityExpr.Left.Type is DoubleType)
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
                    this.GetAssember(equalityExpr.Right);
                    this.WritePush();

                    this.GetAssember(equalityExpr.Left);
                    this.WritePop("%rbx");
                    this.WriteLine("cmpq %rbx, %rax");
                    this.WriteLine("sete %al");

                    break;
                case NullType nullType:
                    this.GetAssember(equalityExpr.Right);
                    this.WritePush();

                    this.GetAssember(equalityExpr.Left);
                    this.WritePop("%rbx");
                    this.WriteLine("cmpq %rbx, %rax");
                    this.WriteLine("sete %al");

                    break;
                case ArrayType arrayType:
                    this.GetAssember(equalityExpr.Right);
                    this.WritePush();

                    this.GetAssember(equalityExpr.Left);
                    this.WritePop("%rbx");
                    this.WriteLine("cmpq %rbx, %rax");
                    this.WriteLine("sete %al");

                    break;
                case StructType structType:
                    this.GetAssember(equalityExpr.Right);
                    this.WritePush();

                    this.GetAssember(equalityExpr.Left);
                    this.WritePop("%rbx");
                    this.WriteLine("cmpq %rbx, %rax");
                    this.WriteLine("sete %al");

                    break;
                default:
                    throw new InvalidOperationException($"Can not use \"equal\" operator with {equalityExpr.Right.Type}");
            }

            this.WriteLine("movzbl %al, %rax");
        }

        private void AndExprAsm(AndExpr andExpr)
        {
            int nextLabel = this.labelCount;
            this.labelCount += 2;

            this.GetAssember(andExpr.Left);

            this.WriteLine("cmpq $0, %rax");
            this.WriteLine($"je .L{nextLabel}");

            this.GetAssember(andExpr.Right);

            this.WriteLine("cmpq $0, %rax");
            this.WriteLine($"je .L{nextLabel}");
            this.WriteLine("movq $1, %rax");
            this.WriteLine($"jmp .L{nextLabel + 1}");

            this.depth -= 1;
            this.WriteLine($".L{nextLabel++}:");
            this.depth += 1;
            this.WriteLine("movq $0, %rax");
            this.depth -= 1;
            this.WriteLine($".L{nextLabel++}:");
            this.depth += 1;
        }

        private void OrExprAsm(OrExpr orExpr)
        {
            int nextLabel = this.labelCount;
            this.labelCount += 2;

            this.GetAssember(orExpr.Left);

            this.WriteLine("cmpq $1, %rax");
            this.WriteLine($"je .L{nextLabel}");

            this.GetAssember(orExpr.Right);
            this.WriteLine("cmpq $1, %rax");
            this.WriteLine($"je .L{nextLabel}");
            this.WriteLine("movq $0, %rax");
            this.WriteLine($"jmp .L{nextLabel + 1}");

            this.depth -= 1;
            this.WriteLine($".L{nextLabel++}:");
            this.depth += 1;
            this.WriteLine("movq $1, %rax");

            this.depth -= 1;
            this.WriteLine($".L{nextLabel++}:");
            this.depth += 1;
        }

        private void NotEqualExprAsm(NotEqualExpr notEqualExpr)
        {
            this.GetAssember(notEqualExpr.Left);

            switch (notEqualExpr.Left.Type)
            {
                case IntType it:
                    this.WritePush();

                    this.GetAssember(notEqualExpr.Right);

                    if (notEqualExpr.Right.Type is DoubleType)
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

                    this.GetAssember(notEqualExpr.Right);

                    if (notEqualExpr.Right.Type is IntType)
                        this.WriteLine("cvtsi2sd %rax, %xmm0");

                    this.WritePop();
                    this.WriteLine("movq %rax, %xmm1");
                    this.WriteLine("ucomisd %xmm0, %xmm1");

                    break;
                case BooleanType booleanType:
                    this.WritePush();

                    this.GetAssember(notEqualExpr.Right);

                    this.WritePop("%rbx");
                    this.WriteLine("cmpq %rax, %rbx");

                    break;
                case NullType nullType:
                    this.WritePush();

                    this.GetAssember(notEqualExpr.Right);

                    this.WritePop("%rbx");
                    this.WriteLine("cmpq %rax, %rbx");

                    break;
                case ArrayType arrayType:
                    this.WritePush();

                    this.GetAssember(notEqualExpr.Right);

                    this.WritePop("%rbx");
                    this.WriteLine("cmpq %rax, %rbx");

                    break;
                case StructType structType:
                    this.WritePush();

                    this.GetAssember(notEqualExpr.Right);

                    this.WritePop("%rbx");
                    this.WriteLine("cmpq %rax, %rbx");

                    break;
            }

            this.WriteLine("setne %al");
            this.WriteLine("movzbl %al, %rax");
        }

        private void ModExprAsm(ModExpr modExpr)
        {
            this.GetAssember(modExpr.Right);
            this.WritePush();
            this.GetAssember(modExpr.Left);

            this.WritePop("%rbx");
            // clear %rdx as idiv divides rdx:rax by specified register
            this.WriteLine("cqto");

            this.WriteLine("idivq %rbx");

            this.WriteLine("movq %rdx, %rax");
        }
    }
}