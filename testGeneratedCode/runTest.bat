dotnet build -c Release ..\compiler\llCompiler.csproj
del .\llCompiler.*
del .\Antlr4.Runtime.Standard.dll
copy ..\compiler\bin\Release\net5.0\* .
.\llCompiler.exe -c testCodeGenV1Prog.ll
wsl gcc -c runner.c testCodeGenV1.c testCodeGenV1Prog.S
wsl gcc -o testCodeGen runner.o testCodeGenV1.o testCodeGenV1Prog.o
wsl ./testCodeGen