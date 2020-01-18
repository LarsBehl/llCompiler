run: link
	./testGeneratedCode/testCodeGen

link: compile
	gcc -o ./testGeneratedCode/testCodeGen ./testGeneratedCode/runner.o ./testGeneratedCode/testCodeGenV1.o ./testGeneratedCode/testCodeGenV1Prog.o

compile: genCode
	gcc -c ./testGeneratedCode/runner.c -o ./testGeneratedCode/runner.o
	gcc -c ./testGeneratedCode/testCodeGenV1.c -o ./testGeneratedCode/testCodeGenV1.o
	gcc -c ./testGeneratedCode/testCodeGenV1Prog.S -o ./testGeneratedCode/testCodeGenV1Prog.o

genCode:
	./testGeneratedCode/llCompiler -c ./testGeneratedCode/testCodeGenV1Prog.ll