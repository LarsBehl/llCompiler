using System;

using ll.AST;
using ll.type;

namespace ll.assembler
{
    public partial class AssemblerGenerator
    {
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
            int nextLabel = this.labelCount;
            this.labelCount += 2;

            this.WriteLine($"jmp .L{nextLabel + 1}");
            // create a label for the body
            this.depth -= 1;
            this.WriteLine($".L{nextLabel++}:");
            this.depth += 1;

            // generate the code for the body
            this.GetAssember(whileStatement.body);

            // create the label for the condition
            this.depth -= 1;
            this.WriteLine($".L{nextLabel++}:");
            this.depth += 1;

            // generate the code for the condition
            this.GetAssember(whileStatement.condition);

            // if the value of the condition is true, jump to the body
            this.WriteLine("cmpq $1, %rax");
            this.WriteLine($"je .L{nextLabel - 2}");
        }

        private void IfStatementAsm(IfStatement ifStatement)
        {
            int nextLabel = this.labelCount;
            this.labelCount += 3;
            // generate the code of the condition
            this.GetAssember(ifStatement.cond);

            // if the condition is true
            this.WriteLine("cmpq $1, %rax");
            // jump to the label of the if clause
            this.WriteLine($"je .L{nextLabel + 1}");

            // create a label for the else case
            this.depth -= 1;
            this.WriteLine($".L{nextLabel++}:");
            this.depth += 1;

            // if there is an else case
            if (ifStatement.elseBody != null)
                // generate the assembler for the else case
                this.GetAssember(ifStatement.elseBody);

            // jump to the end of the if statement
            this.WriteLine($"jmp .L{nextLabel + 1}");

            // create a label for the if case
            this.depth -= 1;
            this.WriteLine($".L{nextLabel++}:");
            this.depth += 1;

            // generate the assembler for the if case
            this.GetAssember(ifStatement.ifBody);

            // create a label for the end of the if statement
            this.depth -= 1;
            this.WriteLine($".L{nextLabel++}:");
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
                    this.WriteLine("cqto");
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

            this.WriteLine("call createHeapObject@PLT");
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

            // value is int but should be interpreted as double
            if (assignArray.value.type is IntType && assignArray.arrayIndex.type is DoubleType)
            {
                this.WriteLine("cvtsi2sdq %rbx, %xmm0");
                this.WriteLine("movq %xmm0, %rbx");
            }

            // save the value in the array
            this.WriteLine("movq %rbx, (%rax)");
        }

        private void DestructionStatementAsm(DestructionStatement destruction)
        {
            this.GetAssember(destruction.refType);
            this.WriteLine("movq %rax, %rdi");

            if (this.stackCounter % 16 == 0)
                this.WriteLine("push $0");

            this.WriteLine("call destroyHeapObject@PLT");
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

            // get the address of the property
            this.LoadStructProperty(assignStruct.structProp);
            // get the value from the stack
            this.WritePop("%rbx");

            if (assignStruct.structProp.type is DoubleType && assignStruct.val.type is IntType)
            {
                this.WriteLine("cvtsi2sdq %rbx, %xmm0");
                this.WriteLine("movq %xmm0, %rbx");
            }

            // save the calculated value
            this.WriteLine("movq %rbx, (%rax)");
        }
    }
}