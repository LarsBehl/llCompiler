using System;

using LL.AST;
using LL.Types;

namespace LL.CodeGeneration
{
    public partial class AssemblerGenerator
    {
        private void BlockStatementAsm(BlockStatement blockStatement)
        {
            foreach (var comp in blockStatement.Body)
                this.GetAssember(comp);
        }

        private void ReturnStatementAsm(ReturnStatement returnStatement)
        {
            this.GetAssember(returnStatement.ReturnValue);
            this.WriteLine("movq %rbp, %rsp");
            this.WritePop("%rbp");
            this.WriteLine("ret");
        }

        private void AssignAsm(AssignStatement assignStatement)
        {
            // if it is the first time the variable is mentioned in this context
            if (!this.VariableMap.ContainsKey(assignStatement.Variable.Name))
            {
                // and there are still more local variables in this function
                if (++this.LocalVariablePointer > this.LocalVariableCount)
                    throw new IndexOutOfRangeException("Tried to create more local variables than the detected amount");
                // map the next empty spot of the reserved stack to the given variable
                this.VariableMap.Add(assignStatement.Variable.Name, this.LocalVariablePointer * (-8));
            }

            // generate the code of the variables value
            this.GetAssember(assignStatement.Value);

            string register = "";

            if (assignStatement.Variable.Type is IntType
            || assignStatement.Variable.Type is BooleanType
            || assignStatement.Value.Type is IntArrayType
            || assignStatement.Value.Type is BoolArrayType
            || assignStatement.Value.Type is DoubleArrayType
            || assignStatement.Value.Type is NullType
            || assignStatement.Value.Type is StructType)
                register = "%rax";

            if (assignStatement.Variable.Type is DoubleType)
            {
                if (assignStatement.Value.Type is IntType)
                    this.WriteLine("cvtsi2sdq %rax, %xmm0");

                register = "%xmm0";
            }


            // save the value of the variable on the stack
            this.WriteLine($"movq {register}, {this.VariableMap[assignStatement.Variable.Name]}(%rbp)");
        }

        private void InstantiationStatementAsm(InstantiationStatement instantiationStatement)
        {
            // map the next empty reserved spot on the stack for the given variable
            this.VariableMap.Add(instantiationStatement.Name, ++this.LocalVariablePointer * (-8));
        }

        private void WhileStatementAsm(WhileStatement whileStatement)
        {
            int nextLabel = this.LabelCount;
            this.LabelCount += 2;

            this.WriteLine($"jmp .L{nextLabel + 1}");
            // create a label for the body
            this.Depth -= 1;
            this.WriteLine($".L{nextLabel++}:");
            this.Depth += 1;

            // generate the code for the body
            this.GetAssember(whileStatement.Body);

            // create the label for the condition
            this.Depth -= 1;
            this.WriteLine($".L{nextLabel++}:");
            this.Depth += 1;

            // generate the code for the condition
            this.GetAssember(whileStatement.Condition);

            // if the value of the condition is true, jump to the body
            this.WriteLine("cmpq $1, %rax");
            this.WriteLine($"je .L{nextLabel - 2}");
        }

        private void IfStatementAsm(IfStatement ifStatement)
        {
            int nextLabel = this.LabelCount;
            this.LabelCount += 3;
            // generate the code of the condition
            this.GetAssember(ifStatement.Cond);

            // if the condition is true
            this.WriteLine("cmpq $1, %rax");
            // jump to the label of the if clause
            this.WriteLine($"je .L{nextLabel + 1}");

            // create a label for the else case
            this.Depth -= 1;
            this.WriteLine($".L{nextLabel++}:");
            this.Depth += 1;

            // if there is an else case
            if (ifStatement.ElseBody != null)
                // generate the assembler for the else case
                this.GetAssember(ifStatement.ElseBody);

            // jump to the end of the if statement
            this.WriteLine($"jmp .L{nextLabel + 1}");

            // create a label for the if case
            this.Depth -= 1;
            this.WriteLine($".L{nextLabel++}:");
            this.Depth += 1;

            // generate the assembler for the if case
            this.GetAssember(ifStatement.IfBody);

            // create a label for the end of the if statement
            this.Depth -= 1;
            this.WriteLine($".L{nextLabel++}:");
            this.Depth += 1;
        }

        private void AddAsignAsm(AddAssignStatement addAssignStatement)
        {
            this.GetAssember(addAssignStatement.Right);

            switch (addAssignStatement.Left.Type)
            {
                case IntType intType:
                    this.WriteLine($"addq %rax, {this.VariableMap[addAssignStatement.Left.Name]}(%rbp)"); break;
                case DoubleType doubleType:
                    if (addAssignStatement.Right.Type is IntType)
                        this.WriteLine("cvtsi2sdq %rax, %xmm0");

                    this.WriteLine("movq %xmm0, %xmm1");
                    this.WriteLine($"movq {this.VariableMap[addAssignStatement.Left.Name]}(%rbp), %xmm0");

                    this.WriteLine("addsd %xmm1, %xmm0");
                    this.WriteLine($"movq %xmm0, {this.VariableMap[addAssignStatement.Left.Name]}(%rbp)");
                    break;
                default:
                    throw new ArgumentException($"AddAssign Statement not compatible with type \"{addAssignStatement.Left.Type.TypeName}\"");
            }
        }

        private void SubAssignAsm(SubAssignStatement subAssignStatement)
        {
            this.GetAssember(subAssignStatement.Right);

            switch (subAssignStatement.Left.Type)
            {
                case IntType intType:
                    this.WriteLine($"subq %rax, {this.VariableMap[subAssignStatement.Left.Name]}(%rbp)"); break;
                case DoubleType doubleType:
                    if (subAssignStatement.Right.Type is IntType)
                        this.WriteLine("cvtsi2sdq %rax, %xmm0");

                    this.WriteLine("movq %xmm0, %xmm1");
                    this.WriteLine($"movq {this.VariableMap[subAssignStatement.Left.Name]}(%rbp), %xmm0");

                    this.WriteLine("subsd %xmm1, %xmm0");
                    this.WriteLine($"movq %xmm0, {this.VariableMap[subAssignStatement.Left.Name]}(%rbp)");
                    break;
                default:
                    throw new ArgumentException($"SubAssign Statement not compatible with type \"{subAssignStatement.Left.Type.TypeName}\"");
            }
        }

        private void MultAssignAsm(MultAssignStatement multAssignStatement)
        {
            this.GetAssember(multAssignStatement.Right);

            switch (multAssignStatement.Left.Type)
            {
                case IntType intType:
                    this.WriteLine($"imulq {this.VariableMap[multAssignStatement.Left.Name]}(%rbp), %rax");
                    this.WriteLine($"movq %rax, {this.VariableMap[multAssignStatement.Left.Name]}(%rbp)");
                    break;
                case DoubleType doubleType:
                    if (multAssignStatement.Right.Type is IntType)
                        this.WriteLine("cvtsi2sdq %rax, %xmm0");

                    this.WriteLine("movq %xmm0, %xmm1");
                    this.WriteLine($"mulsd {this.VariableMap[multAssignStatement.Left.Name]}(%rbp), %xmm0");
                    this.WriteLine($"movq %xmm0, {this.VariableMap[multAssignStatement.Left.Name]}(%rbp)");
                    break;
                default:
                    throw new ArgumentException($"MultAssign Statment not compatible with type \"{multAssignStatement.Left.Type.TypeName}\"");
            }
        }

        private void DivAssignAsm(DivAssignStatement divAssignStatement)
        {
            this.GetAssember(divAssignStatement.Right);

            switch (divAssignStatement.Left.Type)
            {
                case IntType intType:
                    this.WriteLine("movq %rax, %rbx");
                    this.WriteLine("cqto");
                    this.WriteLine($"movq {this.VariableMap[divAssignStatement.Left.Name]}(%rbp), %rax");
                    this.WriteLine($"idivq %rbx");
                    this.WriteLine($"movq %rax, {this.VariableMap[divAssignStatement.Left.Name]}(%rbp)");
                    break;
                case DoubleType doubleType:
                    if (divAssignStatement.Right.Type is IntType)
                        this.WriteLine("cvtsi2sdq %rax, %xmm0");

                    this.WriteLine("movq %xmm0, %xmm1");
                    this.WriteLine($"movq {this.VariableMap[divAssignStatement.Left.Name]}(%rbp), %xmm0");

                    this.WriteLine("divsd %xmm1, %xmm0");
                    this.WriteLine($"movq %xmm0, {this.VariableMap[divAssignStatement.Left.Name]}(%rbp)");
                    break;
                default:
                    throw new ArgumentException($"DivAssign Statement not compatible with type \"{divAssignStatement.Left.Type.TypeName}\"");
            }
        }

        private void PrintStatementAsm(PrintStatement print)
        {
            this.WriteString(print.Value);

            this.GetAssember(print.Value);

            switch (print.Value.Type)
            {
                case IntType it:
                    this.WriteLine("movq %rax, %rsi");
                    this.WriteLine($"leaq .LS{this.StringLabelMap["int"]}(%rip), %rdi");
                    this.WriteLine("movl $0, %eax");

                    break;
                case DoubleType dt:
                    this.WriteLine($"leaq .LS{this.StringLabelMap["double"]}(%rip), %rdi");
                    this.WriteLine("movl $1, %eax");

                    break;
                case BooleanType bt:
                    this.WriteLine("movq %rax, %rsi");
                    this.WriteLine($"leaq .LS{this.StringLabelMap["int"]}(%rip), %rdi");
                    this.WriteLine("movl $0, %eax");

                    break;
            }

            bool aligned = this.AlignStack();

            this.WriteLine("call printf@PLT");

            if(aligned)
                this.WritePop("%rbx");
        }

        private void RefTypeCreationStatementAsm(RefTypeCreationStatement refTypeCreation)
        {
            switch (refTypeCreation.CreatedReftype)
            {
                case AST.Array array:
                    this.GetAssember(array.Size);

                    this.WriteLine("movq $8, %rbx");
                    this.WriteLine("imulq %rbx, %rax");

                    this.WriteLine("movq %rax, %rdi");
                    this.WriteLine("movq $1, %rsi");
                    break;
                case Struct @struct:
                    int structId = this.StructIdMap[@struct.Name];

                    this.WriteLine($"movq ${structId}, %rdi");
                    this.WriteLine($"movq $0, %rsi");
                    break;
                default:
                    throw new NotImplementedException("Omega NASA");
            }

            bool aligned = this.AlignStack();

            this.WriteLine("call createHeapObject@PLT");

            if(aligned)
                this.WritePop("%rbx");
        }

        private void AssignArrayFieldAsm(AssignArrayField assignArray)
        {
            // calculate the value
            this.GetAssember(assignArray.Value);

            // save the value on the stack
            if (assignArray.Value.Type is DoubleType)
                this.WriteLine("movq %xmm0, %rax");

            this.WritePush();

            // load the value of the variable
            this.LoadArrayField(assignArray.ArrayIndex);
            this.WritePop("%rbx");

            // value is int but should be interpreted as double
            if (assignArray.Value.Type is IntType && assignArray.ArrayIndex.Type is DoubleType)
            {
                this.WriteLine("cvtsi2sdq %rbx, %xmm0");
                this.WriteLine("movq %xmm0, %rbx");
            }

            // save the value in the array
            this.WriteLine("movq %rbx, (%rax)");
        }

        private void DestructionStatementAsm(DestructionStatement destruction)
        {
            this.GetAssember(destruction.RefType);
            this.WriteLine("movq %rax, %rdi");

            bool aligned = this.AlignStack();

            this.WriteLine("call destroyHeapObject@PLT");

            if(aligned)
                this.WritePop("%rbx");
        }

        private void AssignStructPropertyAsm(AssignStructProperty assignStruct)
        {
            // calculate the new value
            this.GetAssember(assignStruct.Value);

            // if it is a double, move it into %rax
            if (assignStruct.Value.Type is DoubleType)
                this.WriteLine("movq %xmm0, %rax");

            // push the value on the stack
            this.WritePush();

            // get the address of the property
            this.LoadStructProperty(assignStruct.StructProp);
            // get the value from the stack
            this.WritePop("%rbx");

            if (assignStruct.StructProp.Type is DoubleType && assignStruct.Value.Type is IntType)
            {
                this.WriteLine("cvtsi2sdq %rbx, %xmm0");
                this.WriteLine("movq %xmm0, %rbx");
            }

            // save the calculated value
            this.WriteLine("movq %rbx, (%rax)");
        }
    }
}