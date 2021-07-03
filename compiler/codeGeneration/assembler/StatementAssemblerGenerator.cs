using System.Text;

using LL.AST;
using LL.Exceptions;
using LL.Helper;
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
            bool isGlobal = this.RootProg.IsGlobalVariableDefined(assignStatement.Variable.Name);
            // if it is the first time the variable is mentioned in this context
            if (!this.VariableMap.ContainsKey(assignStatement.Variable.Name) && !isGlobal)
            {
                // and there are still more local variables in this function
                if (++this.LocalVariablePointer > this.LocalVariableCount)
                    throw new OutOfRangeException(this.LocalVariablePointer, this.LocalVariableCount, this.CurrentFile, assignStatement.Line, assignStatement.Column);
                // map the next empty spot of the reserved stack to the given variable
                this.VariableMap.Add(assignStatement.Variable.Name, this.LocalVariablePointer * (-8));
            }

            // generate the code of the variables value
            this.GetAssember(assignStatement.Value);

            string register = "";

            switch (assignStatement.Variable.Type)
            {
                case IntType it:
                case CharType ct:
                case BooleanType bt:
                case IntArrayType iat:
                case DoubleArrayType dat:
                case BoolArrayType bat:
                case CharArrayType cat:
                case NullType nt:
                case StructType st:
                    register = "%rax";
                    break;
                case DoubleType dt:
                    if (assignStatement.Value.Type is IntType)
                        this.WriteLine("cvtsi2sdq %rax, %xmm0");

                    register = "%xmm0";
                    break;
                default:
                    throw new TypeNotAllowedException(assignStatement.Value.Type.ToString(), this.CurrentFile, assignStatement.Line, assignStatement.Column);
            }

            // save the value of the variable on the stack
            if (!isGlobal)
                this.WriteLine($"movq {register}, {this.VariableMap[assignStatement.Variable.Name]}(%rbp)");
            else
                this.WriteLine($"movq {register}, {assignStatement.Variable.Name}(%rip)");
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
                    throw new TypeNotAllowedException(addAssignStatement.Left.Type.ToString(), this.CurrentFile, addAssignStatement.Left.Line, addAssignStatement.Left.Column);
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
                    throw new TypeNotAllowedException(subAssignStatement.Left.Type.ToString(), this.CurrentFile, subAssignStatement.Left.Line, subAssignStatement.Left.Column);
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
                    throw new TypeNotAllowedException(multAssignStatement.Left.Type.ToString(), this.CurrentFile, multAssignStatement.Left.Line, multAssignStatement.Left.Column);
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
                    throw new TypeNotAllowedException(divAssignStatement.Left.Type.ToString(), this.CurrentFile, divAssignStatement.Left.Line, divAssignStatement.Left.Column);
            }
        }

        private void PrintStatementAsm(PrintStatement print)
        {
            this.WriteString(print.Value);

            if (print.Value is not StringLit sl)
                this.GetAssember(print.Value);
            else
                this.WriteLine($"leaq .LS{this.StringLabelMap[sl.Value]}(%rip), %rax");


            if (print.Value.Type is not DoubleType)
                this.WriteLine("movq %rax, %rsi");
            string mapKey = string.Empty;

            switch (print.Value.Type)
            {
                case IntType it:
                    mapKey = Constants.INT_PRINT_STRING;
                    break;
                case DoubleType dt:
                    mapKey = Constants.DOUBLE_PRINT_STRING;
                    break;
                case BooleanType bt:
                    mapKey = Constants.BOOL_PRINT_STRING;
                    break;
                case CharType ct:
                    mapKey = Constants.CHAR_PRINT_STRING;
                    break;
                case CharArrayType ca:
                    mapKey = Constants.STRING_PRINT_STRING;
                    break;
            }

            this.WriteLine($"leaq .LS{this.StringLabelMap[mapKey]}(%rip), {Constants.INTEGER_REGISTERS[0]}");

            if (print.Value.Type is DoubleType)
                this.WriteLine("movl $1, %eax");
            else
                this.WriteLine("movl $0, %eax");

            bool aligned = this.AlignStack();
            this.WriteLine("call printf@PLT");

            if (aligned)
                this.WritePop("%rbx");
        }

        private void RefTypeCreationStatementAsm(RefTypeCreationStatement refTypeCreation)
        {
            switch (refTypeCreation.CreatedReftype)
            {
                case AST.Array array:
                    this.CreateArrayAsm(array);
                    break;
                case Struct @struct:
                    int structId = StructIdMap[@struct.Name];

                    this.WriteLine($"movq ${structId}, %rdi");
                    this.WriteLine($"movq $0, %rsi");
                    break;
                default:
                    throw new TypeNotAllowedException(refTypeCreation.CreatedReftype.Type.ToString(), this.CurrentFile, refTypeCreation.Line, refTypeCreation.Column);
            }

            bool aligned = this.AlignStack();

            this.WriteLine("call createHeapObject@PLT");

            if (aligned)
                this.WritePop("%rbx");
        }

        private string GlobalVariableStruct(Struct @struct)
        {
            StringBuilder builder = new StringBuilder();

            int structId = StructIdMap[@struct.Name];
            builder.AppendLine($"{Constants.INDENTATION}movq ${structId}, %rdi");
            builder.AppendLine($"{Constants.INDENTATION}movq $0, %rsi");

            return builder.ToString();
        }

        private string GlobalVariableArray(AST.Array array)
        {
            StringBuilder bob = new StringBuilder();
            // casting should be safe at this point
            IntLit size = array.Size as IntLit;
            bob.AppendLine($"{Constants.INDENTATION}movq ${size.Value.Value}, %rax");

            switch (array)
            {
                // 64bit arrays
                case IntArray intArray:
                case DoubleArray doubleArray:
                case BoolArray boolArray:
                    bob.AppendLine($"{Constants.INDENTATION}movq $8, %rbx");
                    break;
                // 8bit arrays
                case CharArray charArray:
                    bob.AppendLine($"{Constants.INDENTATION}movq $1, %rbx");
                    break;
                default:
                    throw new TypeNotAllowedException(array.Type.ToString(), this.CurrentFile, array.Line, array.Column);
            }

            bob.AppendLine($"{Constants.INDENTATION}imulq %rbx, %rax");
            bob.AppendLine($"{Constants.INDENTATION}movq %rax, %rdi");
            bob.AppendLine($"{Constants.INDENTATION}movq $1, %rsi");

            return bob.ToString();
        }

        private void CreateArrayAsm(AST.Array array)
        {
            // calculate the size of the array
            this.GetAssember(array.Size);

            switch (array)
            {
                // 64bit arrays
                case IntArray intArray:
                case DoubleArray doubleArray:
                case BoolArray boolArray:
                    this.WriteLine("movq $8, %rbx");
                    break;
                // 8bit arrays
                case CharArray charArray:
                    this.WriteLine("movq $1, %rbx");
                    break;
                default:
                    throw new TypeNotAllowedException(array.Type.ToString(), this.CurrentFile, array.Line, array.Column);
            }
            this.WriteLine("imulq %rbx, %rax");

            this.WriteLine("movq %rax, %rdi");
            this.WriteLine("movq $1, %rsi");
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
            switch (assignArray.ArrayIndex.Type)
            {
                case IntType it:
                case DoubleType dt:
                case BooleanType bt:
                    this.WriteLine("movq %rbx, (%rax)");
                    break;
                case CharType ct:
                    this.WriteLine("mov %bl, (%rax)");
                    break;
            }
        }

        private void DestructionStatementAsm(DestructionStatement destruction)
        {
            this.GetAssember(destruction.RefType);
            this.WriteLine("movq %rax, %rdi");

            bool aligned = this.AlignStack();

            this.WriteLine("call destroyHeapObject@PLT");

            if (aligned)
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

        private void GlobalVariableStatementAsm(GlobalVariableStatement globalVariable)
        {
            this.Depth += 1;

            int size = 8;

            if (globalVariable.Value.Type is CharType)
                size = 1;

            this.WriteLine($".globl {globalVariable.Variable.Name}");
            this.WriteLine(".data");

            if (globalVariable.Value.Type is not CharType)
                this.WriteLine(".align 8");

            this.WriteLine($".type {globalVariable.Variable.Name}, @object");
            this.WriteLine($".size {globalVariable.Variable.Name}, {size}");

            this.Depth -= 1;
            this.WriteLine($"{globalVariable.Variable.Name}:");
            this.Depth += 1;

            switch (globalVariable.Value.Type)
            {
                case CharType ct:
                    this.WriteLine($".byte {(byte)(globalVariable.Value as CharLit).Value.Value}");
                    break;
                case IntType it:
                    this.WriteLine($".quad {(globalVariable.Value as IntLit).Value.Value}");
                    break;
                case BooleanType bt:
                    int value = (globalVariable.Value as BoolLit).Value.Value ? 1 : 0;
                    this.WriteLine($".quad {value}");
                    break;
                case DoubleType dt:
                    this.DoubleToAssemblerString(globalVariable.Value as DoubleLit, out string first, out string second);
                    this.WriteLine($".long {first}");
                    this.WriteLine($".long {second}");
                    break;
                case RefType refType:
                    this.WriteLine(".quad 0");
                    if (globalVariable.Value is StringLit sl)
                        GlobalVariableInitializationBuilder.Append(this.CreateStringLitAsm(sl));
                    else
                    {
                        string typeInitialization = null;
                        if (refType is StructType)
                            typeInitialization = this.GlobalVariableStruct((globalVariable.Value as RefTypeCreationStatement).CreatedReftype as Struct);
                        else
                            typeInitialization = this.GlobalVariableArray((globalVariable.Value as RefTypeCreationStatement).CreatedReftype as LL.AST.Array);
                        GlobalVariableInitializationBuilder.Append(typeInitialization);
                        GlobalVariableInitializationBuilder.AppendLine($"{Constants.INDENTATION}call createHeapObject@PLT");
                    }

                    GlobalVariableInitializationBuilder.AppendLine($"{Constants.INDENTATION}movq %rax, {globalVariable.Variable.Name}(%rip)");
                    break;
                default:
                    throw new TypeNotAllowedException(globalVariable.Value.Type.ToString(), this.CurrentFile, globalVariable.Value.Line, globalVariable.Value.Column);
            }

            this.Depth -= 1;
        }
    }
}