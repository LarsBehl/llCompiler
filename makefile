test: linkTest
	@echo "\n\n\e[0;32mRunning tests...\n\e[0m"
	./testGeneratedCode/bin/testCodeGen
ifeq ($(shell test $$? -gt 0; echo $$?), 0)
	@echo "\n\n\e[0;31mTests not successfull.\t$$? tests failed\n\e[0m"
else
	@echo "\n\n\e[0;32mAll tests successfull\n\e[0m"
endif

linkTest: compileAssembler
	@echo "\n\n\e[0;32mLinking tests...\n\e[0m"
	cp ./baseClassLibrary/bin/libLL.a ./testGeneratedCode/bin/
	gcc -o ./testGeneratedCode/bin/testCodeGen ./testGeneratedCode/bin/testCodeGenV1Prog.o ./testGeneratedCode/bin/testBinOps.o ./testGeneratedCode/bin/testId.o ./testGeneratedCode/bin/testUnary.o ./testGeneratedCode/bin/testAssign.o ./testGeneratedCode/bin/testStructs.o ./testGeneratedCode/bin/testWhile.o ./testGeneratedCode/bin/testIf.o ./testGeneratedCode/bin/testIO.o -LtestGeneratedCode/bin -lLL

compileAssembler: compileTest
	@echo "\n\n\e[0;32mCompiling tests...\n\e[0m"
	gcc -c -g ./testGeneratedCode/bin/testCodeGenV1Prog.S -o ./testGeneratedCode/bin/testCodeGenV1Prog.o
	gcc -c -g ./testGeneratedCode/bin/testBinOps.S -o ./testGeneratedCode/bin/testBinOps.o
	gcc -c -g ./testGeneratedCode/bin/testId.S -o ./testGeneratedCode/bin/testId.o
	gcc -c -g ./testGeneratedCode/bin/testUnary.S -o ./testGeneratedCode/bin/testUnary.o
	gcc -c -g ./testGeneratedCode/bin/testAssign.S -o ./testGeneratedCode/bin/testAssign.o
	gcc -c -g ./testGeneratedCode/bin/testStructs.S -o ./testGeneratedCode/bin/testStructs.o
	gcc -c -g ./testGeneratedCode/bin/testWhile.S -o ./testGeneratedCode/bin/testWhile.o
	gcc -c -g ./testGeneratedCode/bin/testIf.S -o ./testGeneratedCode/bin/testIf.o
	gcc -c -g ./testGeneratedCode/bin/testIO.S -o ./testGeneratedCode/bin/testIO.o

packageRuntime: compileBaseClassLibrary
	@echo "\n\n\e[0;32mPackaging runtime lib...\n\e[0m"
	cp ./runtime/bin/* ./baseClassLibrary/bin/
	ar rcs ./baseClassLibrary/bin/libLL.a ./baseClassLibrary/bin/runtime.o ./baseClassLibrary/bin/errors.o ./baseClassLibrary/bin/addrList.o ./baseClassLibrary/bin/classData.o ./baseClassLibrary/bin/classDataList.o ./baseClassLibrary/bin/util.o ./baseClassLibrary/bin/sys_io.o

compileRuntime: publishLinux
	@echo "\n\n\e[0;32mCompiling runtime...\n\e[0m"
	mkdir -p ./runtime/bin
	gcc -c -g ./runtime/runtime.c -o ./runtime/bin/runtime.o
	gcc -c -g ./runtime/errors.c -o ./runtime/bin/errors.o
	gcc -c -g ./runtime/addrList.c -o ./runtime/bin/addrList.o
	gcc -c -g ./runtime/classData.c -o ./runtime/bin/classData.o
	gcc -c -g ./runtime/classDataList.c -o ./runtime/bin/classDataList.o

compileBaseClassLibrary: compileRuntime
	@echo "\n\n\e[0;32mCompiling Base class library...\n\e[0m"
ifeq (,$(wildcard ./baseClassLibrary/bin))
	mkdir -p ./baseClassLibrary/bin
endif
	cp ./compiler/bin/linux/llCompiler ./baseClassLibrary
	./baseClassLibrary/llCompiler -c ./baseClassLibrary/src/sys_io.ll
	mv ./baseClassLibrary/src/*.S ./baseClassLibrary/bin/
	./baseClassLibrary/llCompiler -h ./baseClassLibrary/src/sys_io.ll
	mv ./baseClassLibrary/src/*.llh ./baseClassLibrary/bin/
	gcc -c -g ./baseClassLibrary/bin/util.S -o ./baseClassLibrary/bin/util.o
	gcc -c -g ./baseClassLibrary/bin/sys_io.S -o ./baseClassLibrary/bin/sys_io.o

compileTest: packageRuntime
	@echo "\n\n\e[0;32mCompiling ll code...\n\e[0m"
	cp ./compiler/bin/linux/llCompiler ./testGeneratedCode
ifeq (,$(wildcard ./testGeneratedCode/bin))
	mkdir -p ./testGeneratedCode/bin
endif
ifeq (,$(wildcard ./testGeneratedCode/header))
	mkdir -p ./testGeneratedCode/header
endif
	mv ./baseClassLibrary/bin/util.llh ./testGeneratedCode/header/
	cp ./baseClassLibrary/header/* ./testGeneratedCode/header/
ifeq (,$(wildcard ./testGeneratedCode/bin/testId.o))
	./testGeneratedCode/llCompiler -c ./testGeneratedCode/programs/testId.ll
	mv ./testGeneratedCode/programs/testId.S ./testGeneratedCode/bin/testId.S
	./testGeneratedCode/llCompiler -h ./testGeneratedCode/programs/testId.ll
	mv ./testGeneratedCode/programs/testId.ll ./testGeneratedCode/programs/testId.bak
endif
	./testGeneratedCode/llCompiler -c ./testGeneratedCode/programs/testCodeGenV1Prog.ll
	mv ./testGeneratedCode/programs/*.S ./testGeneratedCode/bin/

generateCode:
	@echo "\n\n\e[0;32mGenerating code from grammar...\n\e[0m"
	cd ./compiler \
	java -jar ../deps/antlr-4.9.1-complete.jar -Dlanguage=CSharp ll.g4 -no-listener -visitor  -package LL

publishWindows:
	@echo "\n\n\e[0;32mPublishing Windows...\n\e[0m"
	dotnet publish -c Release --self-contained -r win10-x64 -o ./compiler/bin/win10 -p:PublishSingleFile=true ./compiler/llCompiler.csproj

publishLinux:
	@echo "\n\n\e[0;32mPublishing Linux...\n\e[0m"
	dotnet publish -c Release --self-contained -r linux-x64 -o ./compiler/bin/linux -p:PublishSingleFile=true ./compiler/llCompiler.csproj

publishOSX:
	@echo "\n\n\e[0;32mPublishing OSX...\n\e[0m"
	dotnet publish -c Release --self-contained -r osx-x64 -o ./compiler/bin/OSX -p:PublishSingleFile=true ./compiler/llCompiler.csproj

publishAll: publishWindows publishLinux publishOSX

clean:
	@echo "\n\n\e[0;32mCleaning...\n\e[0m"
	dotnet clean ./compiler/llCompiler.csproj
	rm -r -f ./runtime/bin
	rm -r -f ./testGeneratedCode/bin
	rm -r -f ./testGeneratedCode/header
	rm -r -f ./baseClassLibrary/bin
	rm -f ./baseClassLibrary/llCompiler
	rm -f ./testGeneratedCode/llCompiler
ifneq (,$(wildcard ./testGeneratedCode/programs/testId.bak))
	mv ./testGeneratedCode/programs/testId.bak ./testGeneratedCode/programs/testId.ll
endif

ifneq (,$(wildcard ./testGeneratedCode/programs/testId.llh))
	rm -f ./testGeneratedCode/programs/testId.llh
endif

restore:
	@echo "\n\n\e[0;32mRestoring...\n\e[0m"
	dotnet restore ./compiler/llCompiler.csproj