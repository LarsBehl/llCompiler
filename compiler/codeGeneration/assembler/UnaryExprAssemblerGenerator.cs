using System;
using System.Collections.Generic;

using ll.AST;
using ll.type;

namespace ll.assembler
{
    public partial class AssemblerGenerator
    {
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

            functionAsm.usedDoubleRegisters = 0;
            functionAsm.usedIntegerRegisters = 0;

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
                for (int i = funDef.args.Count - 1; i >= min; i--)
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
                        case RefType refType:
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

                // align the stack if needed
                if (this.stackCounter % 16 == 0)
                {
                    this.stackCounter += 8;
                    rbpOffset += 8;
                }

                this.WriteLine($"subq ${rbpOffset - 16}, %rsp");
            }

            int index = Math.Min(functionCall.args.Count, integerOverflowPosition);
            // move integer/boolean arguments into registers until they are full
            for (int i = 0; i < index; i++)
            {
                if (funDef.args[i].type is IntType || funDef.args[i].type is BooleanType || funDef.args[i].type is RefType)
                {
                    this.GetAssember(functionCall.args[i]);

                    this.WriteLine($"movq %rax, {this.integerRegisters[functionAsm.usedIntegerRegisters]}");
                    functionAsm.usedIntegerRegisters++;
                }
            }

            // move integer/boolean arguments that overflew on stack
            for (int i = functionCall.args.Count - 1; i >= integerOverflowPosition; i++)
            {
                if (funDef.args[i].type is IntType || funDef.args[i].type is BooleanType || funDef.args[i].type is RefType)
                {
                    this.GetAssember(functionCall.args[i]);

                    this.WriteLine($"movq %rax, {functionAsm.variableMap[funDef.args[i].name] - 16}(%rsp)");
                }
            }

            // move double arguments that overflew on the stack
            for (int i = functionCall.args.Count - 1; i >= doubleOverflowPosition; i++)
            {
                if (funDef.args[i].type is DoubleType)
                {
                    this.GetAssember(functionCall.args[i]);

                    if (functionCall.args[i].type is IntType)
                        this.WriteLine("cvtsi2sdq %rax, %xmm0");

                    this.WriteLine($"movq %xmm0, {functionAsm.variableMap[funDef.args[i].name] - 16}(%rsp)");
                }
            }

            // calculate the rest of the double arguments and push them on the stack
            for (int i = Math.Min(functionCall.args.Count - 1, doubleOverflowPosition); i >= 0; i--)
            {
                if (funDef.args[i].type is DoubleType)
                {
                    this.GetAssember(functionCall.args[i]);

                    if (functionCall.args[i].type is IntType)
                        this.WriteLine("cvtsi2sdq %rax, %xmm0");

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

            // this should only happen if the registers do not overflow and the stack is not aligned
            this.AlignStack();

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

        private void NullLitAsm(NullLit nullLit)
        {
            this.WriteLine("movq $0, %rax");
        }
        
        private void ArrayIndexingAsm(ArrayIndexing arrayIndexing)
        {
            this.LoadArrayField(arrayIndexing);
            if (arrayIndexing.type is DoubleType)
                this.WriteLine("movq (%rax), %xmm0");
            else
                this.WriteLine("movq (%rax), %rax");
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
    }
}