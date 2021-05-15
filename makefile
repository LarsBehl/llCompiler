test: linkTest
	@echo "\n\n\e[0;32mRunning tests...\n\e[0m"
	./testGeneratedCode/bin/testCodeGen

linkTest: compileAssembler
	@echo "\n\n\e[0;32mLinking tests...\n\e[0m"
	cp ./runtime/bin/libLL.a ./testGeneratedCode/bin/
	gcc -o ./testGeneratedCode/bin/testCodeGen ./testGeneratedCode/bin/testCodeGenV1Prog.o ./testGeneratedCode/bin/testBinOps.o -LtestGeneratedCode/bin -lLL

compileAssembler: packageRuntime
	@echo "\n\n\e[0;32mCompiling tests...\n\e[0m"
	gcc -c -g ./testGeneratedCode/bin/testCodeGenV1Prog.S -o ./testGeneratedCode/bin/testCodeGenV1Prog.o
	gcc -c -g ./testGeneratedCode/bin/testBinOps.S -o ./testGeneratedCode/bin/testBinOps.o

packageRuntime: compileRuntime
	@echo "\n\n\e[0;32mPackaging runtime lib...\n\e[0m"
	ar rcs ./runtime/bin/libLL.a ./runtime/bin/runtime.o ./runtime/bin/errors.o ./runtime/bin/addrList.o ./runtime/bin/classData.o ./runtime/bin/classDataList.o

compileRuntime: compileTest
	@echo "\n\n\e[0;32mCompiling runtime...\n\e[0m"
	mkdir -p ./runtime/bin
	gcc -c -g ./runtime/runtime.c -o ./runtime/bin/runtime.o
	gcc -c -g ./runtime/errors.c -o ./runtime/bin/errors.o
	gcc -c -g ./runtime/addrList.c -o ./runtime/bin/addrList.o
	gcc -c -g ./runtime/classData.c -o ./runtime/bin/classData.o
	gcc -c -g ./runtime/classDataList.c -o ./runtime/bin/classDataList.o

compileTest: publishLinux
	@echo "\n\n\e[0;32mCompiling ll code...\n\e[0m"
	cp ./compiler/bin/linux/llCompiler ./testGeneratedCode
	./testGeneratedCode/llCompiler -c ./testGeneratedCode/programs/testCodeGenV1Prog.ll
	mkdir -p ./testGeneratedCode/bin
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
	rm -f ./testGeneratedCode/llCompiler

restore:
	@echo "\n\n\e[0;32mRestoring...\n\e[0m"
	dotnet restore ./compiler/llCompiler.csproj