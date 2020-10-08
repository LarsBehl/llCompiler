run: link
	./testGeneratedCode/testCodeGen

link: compile
	gcc -o ./testGeneratedCode/testCodeGen ./testGeneratedCode/runner.o ./testGeneratedCode/testCodeGenV1.o ./testGeneratedCode/testCodeGenV1Prog.o ./testGeneratedCode/runtime.o ./testGeneratedCode/errors.o ./testGeneratedCode/addrList.o ./testGeneratedCode/classData.o ./testGeneratedCode/classDataList.o

compile: buildRuntime
	gcc -c ./testGeneratedCode/runner.c -o ./testGeneratedCode/runner.o
	gcc -c ./testGeneratedCode/testCodeGenV1.c -o ./testGeneratedCode/testCodeGenV1.o
	gcc -c ./testGeneratedCode/testCodeGenV1Prog.S -o ./testGeneratedCode/testCodeGenV1Prog.o

buildRuntime: genCode
	gcc -c ./runtime/runtime.c -o ./testGeneratedCode/runtime.o
	gcc -c ./runtime/errors.c -o ./testGeneratedCode/errors.o
	gcc -c ./runtime/addrList.c -o ./testGeneratedCode/addrList.o
	gcc -c ./runtime/classData.c -o ./testGeneratedCode/classData.o
	gcc -c ./runtime/classDataList.c -o ./testGeneratedCode/classDataList.o

genCode:
	./testGeneratedCode/llCompiler -c ./testGeneratedCode/testCodeGenV1Prog.ll