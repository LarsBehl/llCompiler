using System;

using ll.AST;
using ll.type;

namespace ll.assembler
{
    public partial class AssemblerGenerator
    {
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
                        // clear %rdx as idiv divides rdx:rax by specified register
                        this.WriteLine("cqto");

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
                case NullType nullType:
                    this.GetAssember(equalityExpr.right);
                    this.WritePush();

                    this.GetAssember(equalityExpr.left);
                    this.WritePop("%rbx");
                    this.WriteLine("cmpq %rbx, %rax");
                    this.WriteLine("sete %al");

                    break;
                case ArrayType arrayType:
                    this.GetAssember(equalityExpr.right);
                    this.WritePush();

                    this.GetAssember(equalityExpr.left);
                    this.WritePop("%rbx");
                    this.WriteLine("cmpq %rbx, %rax");
                    this.WriteLine("sete %al");

                    break;
                case StructType structType:
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

        private void AndExprAsm(AndExpr andExpr)
        {
            int nextLabel = this.labelCount;
            this.labelCount += 2;

            this.GetAssember(andExpr.left);

            this.WriteLine("cmpq $0, %rax");
            this.WriteLine($"je .L{nextLabel}");

            this.GetAssember(andExpr.right);

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

            this.GetAssember(orExpr.left);

            this.WriteLine("cmpq $1, %rax");
            this.WriteLine($"je .L{nextLabel}");

            this.GetAssember(orExpr.right);
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
                case NullType nullType:
                    this.WritePush();

                    this.GetAssember(notEqualExpr.right);

                    this.WritePop("%rbx");
                    this.WriteLine("cmpq %rax, %rbx");

                    break;
                case ArrayType arrayType:
                    this.WritePush();

                    this.GetAssember(notEqualExpr.right);

                    this.WritePop("%rbx");
                    this.WriteLine("cmpq %rax, %rbx");

                    break;
                case StructType structType:
                    this.WritePush();

                    this.GetAssember(notEqualExpr.right);

                    this.WritePop("%rbx");
                    this.WriteLine("cmpq %rax, %rbx");

                    break;
            }

            this.WriteLine("setne %al");
            this.WriteLine("movzbl %al, %rax");
        }

        private void ModExprAsm(ModExpr modExpr)
        {
            this.GetAssember(modExpr.right);
            this.WritePush();
            this.GetAssember(modExpr.left);

            this.WritePop("%rbx");
            // clear %rdx as idiv divides rdx:rax by specified register
            this.WriteLine("cqto");

            this.WriteLine("idivq %rbx");

            this.WriteLine("movq %rdx, %rax");
        }
    }
}