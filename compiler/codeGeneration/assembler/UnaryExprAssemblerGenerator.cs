using System;
using System.Text;

using LL.AST;
using LL.Exceptions;
using LL.Helper;
using LL.Types;

namespace LL.CodeGeneration
{
    public partial class AssemblerGenerator
    {
        private void IntLitAsm(IntLit intLit)
        {
            this.WriteLine($"movq ${intLit.Value}, %rax");
        }

        private void DoubleLitAsm(DoubleLit doubleLit)
        {
            this.WriteDoubleValue(doubleLit);
            this.WriteLine($"movq .LD{this.DoubleMap[doubleLit.Value ?? 0]}(%rip), %xmm0");
        }

        private void BoolLitAsm(BoolLit boolLit)
        {
            this.WriteLine($"movq ${((boolLit.Value ?? false) ? 1 : 0)}, %rax");
        }

        private void CharLitAsm(CharLit charLit)
        {
            this.WriteLine($"movq ${(byte)charLit.Value.Value}, %rax");
        }

        private void StringLitAsm(StringLit stringLit)
        {
            bool alligned = this.AlignStack();

            this.WriteLine(this.CreateStringLitAsm(stringLit));

            if(alligned)
                this.WritePop("%rbx");
        }

        private String CreateStringLitAsm(StringLit stringLit)
        {
            StringBuilder bob = new StringBuilder();

            this.WriteString(stringLit);
            bob.AppendLine($"{Constants.INDENTATION}leaq .LS{this.StringLabelMap[stringLit.Value]}(%rip), {Constants.INTEGER_REGISTERS[0]}");
            bob.AppendLine($"{Constants.INDENTATION}movq ${stringLit.Length}, {Constants.INTEGER_REGISTERS[1]}");

            bob.AppendLine($"{Constants.INDENTATION}call createStringFromLiteral@PLT");

            return bob.ToString();
        }

        private void FunctionCallAsm(FunctionCall functionCall)
        {
            FunctionDefinition funDef = this.RootProg.GetFunctionDefinition(functionCall.FunctionName);
            FunctionAsm functionAsm;

            if (this.FunctionMap.ContainsKey(functionCall.FunctionName))
                functionAsm = this.FunctionMap[functionCall.FunctionName];
            else
            {
                functionAsm = new FunctionAsm(functionCall.FunctionName);
                this.FunctionMap.Add(functionCall.FunctionName, functionAsm);
            }

            functionAsm.UsedDoubleRegisters = 0;
            functionAsm.UsedIntegerRegisters = 0;

            bool doesOverflow = this.DoesOverflowRegisters(
                functionCall.Args,
                out int integerOverflowPosition,
                out int doubleOverflowPosition
            );

            // if necessary reserve space for overflown arguments on the stack
            if (doesOverflow)
            {
                int min = Math.Min(integerOverflowPosition, doubleOverflowPosition);
                int rbpOffset = +16;

                // calculate the position of the overflown arguments on the stack
                for (int i = funDef.Args.Count - 1; i >= min; i--)
                {
                    InstantiationStatement arg = funDef.Args[i];

                    switch (functionCall.Args[i].Type)
                    {
                        case IntType intType:
                            if (i >= integerOverflowPosition)
                            {
                                functionAsm.VariableMap[arg.Name] = rbpOffset;
                                rbpOffset += 8;
                            }

                            break;
                        case DoubleType doubleType:
                            if (i >= doubleOverflowPosition)
                            {
                                functionAsm.VariableMap[arg.Name] = rbpOffset;
                                rbpOffset += 8;
                            }

                            break;
                        case BooleanType booleanType:
                            if (i >= integerOverflowPosition)
                            {
                                functionAsm.VariableMap[arg.Name] = rbpOffset;
                                rbpOffset += 8;
                            }

                            break;
                        case CharType charType:
                            if (i >= integerOverflowPosition)
                            {
                                functionAsm.VariableMap[arg.Name] = rbpOffset;
                                rbpOffset += 8;
                            }

                            break;
                        case RefType refType:
                            if (i >= integerOverflowPosition)
                            {
                                functionAsm.VariableMap[arg.Name] = rbpOffset;
                                rbpOffset += 8;
                            }

                            break;
                        default:
                            throw new UnknownTypeException(arg.Type.ToString(), this.CurrentFile, arg.Line, arg.Column);
                    }
                }
                this.StackCounter += rbpOffset - 16;

                // align the stack if needed
                if (this.StackCounter % 16 == 0)
                {
                    this.StackCounter += 8;
                    rbpOffset += 8;
                }

                this.WriteLine($"subq ${rbpOffset - 16}, %rsp");
            }

            int index = Math.Min(functionCall.Args.Count, integerOverflowPosition);
            // move integer/boolean arguments into registers until they are full
            for (int i = 0; i < index; i++)
            {
                if (funDef.Args[i].Type is IntType || funDef.Args[i].Type is BooleanType || funDef.Args[i].Type is CharType || funDef.Args[i].Type is RefType)
                {
                    this.GetAssember(functionCall.Args[i]);

                    this.WriteLine($"movq %rax, {Constants.INTEGER_REGISTERS[functionAsm.UsedIntegerRegisters]}");
                    functionAsm.UsedIntegerRegisters++;
                }
            }

            // move integer/boolean arguments that overflew on stack
            for (int i = functionCall.Args.Count - 1; i >= integerOverflowPosition; i--)
            {
                if (funDef.Args[i].Type is IntType || funDef.Args[i].Type is BooleanType || funDef.Args[i].Type is CharType || funDef.Args[i].Type is RefType)
                {
                    this.GetAssember(functionCall.Args[i]);

                    this.WriteLine($"movq %rax, {functionAsm.VariableMap[funDef.Args[i].Name] - 16}(%rsp)");
                }
            }

            // move double arguments that overflew on the stack
            for (int i = functionCall.Args.Count - 1; i >= doubleOverflowPosition; i--)
            {
                if (funDef.Args[i].Type is DoubleType)
                {
                    this.GetAssember(functionCall.Args[i]);

                    if (functionCall.Args[i].Type is IntType)
                        this.WriteLine("cvtsi2sdq %rax, %xmm0");

                    this.WriteLine($"movq %xmm0, {functionAsm.VariableMap[funDef.Args[i].Name] - 16}(%rsp)");
                }
            }

            // calculate the rest of the double arguments and push them on the stack
            for (int i = Math.Min(functionCall.Args.Count - 1, doubleOverflowPosition - 1); i >= 0; i--)
            {
                if (funDef.Args[i].Type is DoubleType)
                {
                    this.GetAssember(functionCall.Args[i]);

                    if (functionCall.Args[i].Type is IntType)
                        this.WriteLine("cvtsi2sdq %rax, %xmm0");

                    this.WriteLine($"movq %xmm0, %rax");
                    this.WritePush();
                    functionAsm.UsedDoubleRegisters++;
                }
            }

            // move the previously pushed double arguments into the double registers
            for (int i = 0; i < functionAsm.UsedDoubleRegisters; i++)
            {
                this.WritePop();
                this.WriteLine($"movq %rax, {Constants.DOUBLE_REGISTERS[i]}");
            }

            // this should only happen if the registers do not overflow and the stack is not aligned
            bool aligned = this.AlignStack();

            this.WriteLine($"call {functionCall.FunctionName}@PLT");

            if (aligned)
                this.WritePop("%rbx");
        }

        private void VariableAsm(VarExpr varExpr)
        {
            string register;

            switch (varExpr.Type)
            {
                case IntType it:
                case BooleanType bt:
                case CharType ct:
                case RefType rt:
                    register = "%rax";
                    break;
                case DoubleType dt:
                    register = "%xmm0";
                    break;
                default:
                    throw new TypeNotAllowedException(varExpr.Type.ToString(), this.CurrentFile, varExpr.Line, varExpr.Column);
            }

            if(this.RootProg.IsGlobalVariableDefined(varExpr.Name))
                this.WriteLine($"movq {varExpr.Name}(%rip), {register}");
            else
                this.WriteLine($"movq {this.VariableMap[varExpr.Name]}(%rbp), {register}");
        }

        private void IncrementAsm(IncrementExpr increment)
        {
            this.GetAssember(increment.Variable);
            string register = null;

            if (!increment.Post)
            {
                if (increment.Type is IntType)
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
                if (increment.Type is IntType)
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

            switch (increment.Variable)
            {
                case VarExpr varExpr:
                    this.WriteLine($"movq {register}, {this.VariableMap[varExpr.Name]}(%rbp)");
                    if (varExpr.Type is DoubleType)
                        this.WriteLine("movq %xmm0, %rax");
                    break;
                case ArrayIndexing arrayIndexing:
                    if (increment.Post)
                    {
                        if (arrayIndexing.Type is DoubleType)
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

                    if (increment.Post)
                        this.WritePop();
                    break;
                case StructPropertyAccess propertyAccess:
                    if (increment.Post)
                    {
                        if (propertyAccess.Type is DoubleType)
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

                    if (increment.Post)
                        this.WritePop();
                    break;
            }

            if (increment.Type is DoubleType)
                this.WriteLine("movq %rax, %xmm0");
        }

        private void DecrementAsm(DecrementExpr decrement)
        {
            this.GetAssember(decrement.Variable);
            string register = null;

            if (!decrement.Post)
            {
                if (decrement.Type is IntType)
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
                if (decrement.Type is IntType)
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

            switch (decrement.Variable)
            {
                case VarExpr varExpr:
                    this.WriteLine($"movq {register}, {VariableMap[varExpr.Name]}(%rbp)");

                    if (varExpr.Type is DoubleType)
                        this.WriteLine("movq %xmm0, %rax");
                    break;
                case ArrayIndexing arrayIndexing:
                    if (decrement.Post)
                    {
                        if (arrayIndexing.Type is DoubleType)
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

                    if (decrement.Post)
                        this.WritePop();
                    break;
                case StructPropertyAccess propertyAccess:
                    if (decrement.Post)
                    {
                        if (propertyAccess.Type is DoubleType)
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

                    if (decrement.Post)
                        this.WritePop();
                    break;
            }

            if (decrement.Type is DoubleType)
                this.WriteLine("movq %rax, %xmm0");
        }

        private void NotExprAsm(NotExpr notExpr)
        {
            this.GetAssember(notExpr.Value);
            this.WriteLine("cmpq $0, %rax");
            this.WriteLine("sete %al");
            this.WriteLine("movzbl %al, %rax");
        }

        private void NullLitAsm(NullLit nullLit)
        {
            this.WriteLine("movq $0, %rax");
        }

        private void ArrayIndexingAsm(ArrayIndexing arrayIndexing)
        {
            this.LoadArrayField(arrayIndexing);
            switch (arrayIndexing.Type)
            {
                case DoubleType dt:
                    this.WriteLine("movq (%rax), %xmm0");
                    break;
                case IntType it:
                case BooleanType bt:
                    this.WriteLine("movq (%rax), %rax");
                    break;
                case CharType ct:
                    // %rax needs to be cleared
                    this.WriteLine("movq %rax, %rbx");
                    this.WriteLine("movq $0, %rax");
                    this.WriteLine("mov (%rbx), %al");
                    break;
            }
        }

        private void StructPropertyAccessAsm(StructPropertyAccess structPropertyAccess)
        {
            this.LoadStructProperty(structPropertyAccess);
            // write the value in the correct register
            if (structPropertyAccess.Type is DoubleType)
                this.WriteLine("movq (%rax), %xmm0");
            else
                this.WriteLine("movq (%rax), %rax");
        }
    }
}