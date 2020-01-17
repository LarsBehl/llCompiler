run: link
	./testCodeGen

link: compile
	gcc -o testCodeGen runner.o testCodeGenV1.o testCodeGenV1Prog.o

compile: genCode
	gcc -c runner.c testCodeGenV1.c testCodeGenV1Prog.S

genCode:
	./llCompiler -c testCodeGenV1Prog.ll