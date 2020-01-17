run: testCodeGen
	./testCodeGen

link: runner.o testCodeGenV1.o testCodeGenV1Prog.o compile
	gcc -o testCodeGen runner.o testCodeGenV1.o testCodeGenV1Prog.o

compile: runner.c testCodeGenV1.c testCodeGenV1Prog.S genCode
	gcc -c runner.c testCodeGenV1.c testCodeGenV1Prog.S

genCode: testCodeGenV1Prog.ll llCompiler.dll
	./llCompiler -c testCodeGenV1Prog.ll