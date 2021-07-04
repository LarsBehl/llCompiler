$compilerLocation = './compiler'
$testLocation = './testGeneratedCode'
$runtimeLocation = './runtime'
$baseClassLibLocation = './baseClassLibrary'

function printMessage($message) {
    Write-Host "`n`n$($message)...`n" -ForegroundColor DarkGreen
}

function printError($message) {
    Write-Host "`n$($message)" -ForegroundColor DarkRed
}

function printAndRun($command) {
    Write-Host "$($command)"
    Invoke-Expression $command
}

function remove($file) {
    Write-Host "rm -r -f $($file)"
    Remove-Item $file -Recurse -Force -ErrorAction Ignore
}

function clean() {
    printMessage -message "Cleaning"
    printAndRun -command "dotnet clean $($compilerLocation)/llCompiler.csproj"
    remove -file "$($runtimeLocation)/bin"
    remove -file "$($testLocation)/bin"
    remove -file "$($baseClassLibLocation)/bin"
    remove -file "$($baseClassLibLocation)/*.dll"
    remove -file "$($baseClassLibLocation)/*.exe"
    remove -file "$($baseClassLibLocation)/*.pdb"
    remove -file "$($testLocation)/header"
    remove -file "$($testLocation)/*.dll"
    remove -file "$($testLocation)/*.exe"
    remove -file "$($testLocation)/*.pdb"
    if (Test-Path "$($testLocation)/programs/testId.bak") {
        Move-Item "$($testLocation)/programs/testId.bak" -Destination "$($testLocation)/programs/testId.ll"
    }

    if(Test-Path "$($testLocation)/programs/testId.llh") {
        remove -file "$($testLocation)/programs/testId.llh"
    }
}

function restore() {
    printMessage -message "Restoring"
    printAndRun -command "dotnet restore $($compilerLocation)/llCompiler.csproj"
}

function publishWindows() {
    printMessage -message "Publishing Windows"
    printAndRun -command "dotnet publish -c Release --self-contained -r win10-x64 -o $($compilerLocation)/bin/win10 -p:PublishSingleFile=true $($compilerLocation)/llCompiler.csproj"
}

function publishLinux() {
    printMessage -message "Publishing Linux"
    printAndRun -command "dotnet publish -c Release --self-contained -r linux-x64 -o $($compilerLocation)/bin/linux -p:PublishSingleFile=true $($compilerLocation)/llCompiler.csproj"
}

function publishOSX() {
    printMessage -message "Publishing OSX"
    printAndRun -command "dotnet publish -c Release --self-contained -r osx-x64 -o $($compilerLocation)/bin/OSX -p:PublishSingleFile=true $($compilerLocation)/llCompiler.csproj"
}

function publishAll() {
    publishWindows
    publishLinux
    publishOSX
}

function linkTest() {
    printMessage -message "Linking tests"
    Write-Host "cp $($baseClassLibLocation)/bin/libLL.a $($testLocation)/bin/"
    Copy-Item "$($baseClassLibLocation)/bin/libLL.a" -Destination "$($testLocation)/bin/"
    printAndRun -command "wsl gcc -o $($testLocation)/bin/testCodeGen $($testLocation)/bin/testCodeGenV1Prog.o $($testLocation)/bin/testBinOps.o $($testLocation)/bin/testId.o $($testLocation)/bin/testUnary.o $($testLocation)/bin/testAssign.o $($testLocation)/bin/testStructs.o $($testLocation)/bin/testWhile.o $($testLocation)/bin/testIf.o $($testLocation)/bin/testIO.o -LtestGeneratedCode/bin -lLL"
}

function compileAssember() {
    printMessage -message "Compiling tests"
    printAndRun -command "wsl gcc -c -g $($testLocation)/bin/testCodeGenV1Prog.S -o $($testLocation)/bin/testCodeGenV1Prog.o"
    printAndRun -command "wsl gcc -c -g $($testLocation)/bin/testBinOps.S -o $($testLocation)/bin/testBinOps.o"
    printAndRun -command "wsl gcc -c -g $($testLocation)/bin/testId.S -o $($testLocation)/bin/testId.o"
    printAndRun -command "wsl gcc -c -g $($testLocation)/bin/testUnary.S -o $($testLocation)/bin/testUnary.o"
    printAndRun -command "wsl gcc -c -g $($testLocation)/bin/testAssign.S -o $($testLocation)/bin/testAssign.o"
    printAndRun -command "wsl gcc -c -g $($testLocation)/bin/testStructs.S -o $($testLocation)/bin/testStructs.o"
    printAndRun -command "wsl gcc -c -g $($testLocation)/bin/testWhile.S -o $($testLocation)/bin/testWhile.o"
    printAndRun -command "wsl gcc -c -g $($testLocation)/bin/testIf.S -o $($testLocation)/bin/testIf.o"
    printAndRun -command "wsl gcc -c -g $($testLocation)/bin/testIO.S -o $($testLocation)/bin/testIO.o"
}

function packageRuntime() {
    printMessage -message "Packaging runtime lib"
    Write-Host "cp $($runtimeLocation)/bin/* $($baseClassLibLocation)/bin/"
    Copy-Item "$($runtimeLocation)/bin/*" -Destination "$($baseClassLibLocation)/bin/" -Force
    printAndRun -command "wsl ar rcs $($baseClassLibLocation)/bin/libLL.a $($baseClassLibLocation)/bin/runtime.o $($baseClassLibLocation)/bin/errors.o $($baseClassLibLocation)/bin/addrList.o $($baseClassLibLocation)/bin/classData.o $($baseClassLibLocation)/bin/classDataList.o $($baseClassLibLocation)/bin/util.o $($baseClassLibLocation)/bin/sys_io.o"
}

function compileRuntime() {
    printMessage -message "Compiling runtime"
    if (!(Test-Path "$($runtimeLocation)/bin")) {
        New-Item -ItemType Directory -Force -Path "$($runtimeLocation)/bin"
    }
    printAndRun -command "wsl gcc -c -g $($runtimeLocation)/runtime.c -o $($runtimeLocation)/bin/runtime.o"
    printAndRun -command "wsl gcc -c -g $($runtimeLocation)/errors.c -o $($runtimeLocation)/bin/errors.o"
    printAndRun -command "wsl gcc -c -g $($runtimeLocation)/addrList.c -o $($runtimeLocation)/bin/addrList.o"
    printAndRun -command "wsl gcc -c -g $($runtimeLocation)/classData.c -o $($runtimeLocation)/bin/classData.o"
    printAndRun -command "wsl gcc -c -g $($runtimeLocation)/classDataList.c -o $($runtimeLocation)/bin/classDataList.o"
}

function compileBaseClassLibrary() {
    printMessage -message "Compiling Base class library"
    if (!(Test-Path "$($baseClassLibLocation)/bin")) {
        New-Item -ItemType Directory -Force -Path "$($baseClassLibLocation)/bin"
    }
    # Copy compiler
    Write-Host "cp $($compilerLocation)/bin/win10/* $($baseClassLibLocation)"
    Copy-Item "$($compilerLocation)/bin/win10/*" -Destination "$($baseClassLibLocation)/"
    # Compile library
    printAndRun -command "$($baseClassLibLocation)/llCompiler.exe -c $($baseClassLibLocation)/src/util.ll"
    printAndRun -command "$($baseClassLibLocation)/llCompiler.exe -c $($baseClassLibLocation)/src/sys_io.ll"
    Write-Host "mv $($baseClassLibLocation)/src/*.S $($baseClassLibLocation)/bin/"
    Move-Item "$($baseClassLibLocation)/src/*.S" -Destination "$($baseClassLibLocation)/bin/" -Force
    printAndRun "wsl gcc -c -g $($baseClassLibLocation)/bin/util.S -o $($baseClassLibLocation)/bin/util.o"
    printAndRun "wsl gcc -c -g $($baseClassLibLocation)/bin/sys_io.S -o $($baseClassLibLocation)/bin/sys_io.o"
    # Generate header files
    printAndRun -command "$($baseClassLibLocation)/llCompiler.exe -h $($baseClassLibLocation)/src/util.ll"
    printAndRun -command "$($baseClassLibLocation)/llCompiler.exe -h $($baseClassLibLocation)/src/sys_io.ll"
    Write-Host "mv $($baseClassLibLocation)/src/*.llh $($baseClassLibLocation)/bin/"
    Move-Item "$($baseClassLibLocation)/src/*.llh" -Destination "$($baseClassLibLocation)/bin/" -Force
}

function compileTest() {
    printMessage -message "Compiling ll code"
    Write-Host "cp $($compilerLocation)/bin/win10/* $($testLocation)/"
    Copy-Item "$($compilerLocation)/bin/win10/*" -Destination "$($testLocation)/"

    if (!(Test-Path "$($testLocation)/bin")) {
        New-Item -ItemType Directory -Force -Path "$($testLocation)/bin"
    }

    if (!(Test-Path "$($testLocation)/header")) {
        New-Item -ItemType Directory -Force -Path "$($testLocation)/header"
    }

    Write-Host "mv $($baseClassLibLocation)/bin/*.llh $($testLocation)/header/"
    Copy-Item "$($baseClassLibLocation)/bin/*.llh" -Destination "$($testLocation)/header/" -Force
    Write-Host "mv $($baseClassLibLocation)/header/* $($testLocation)/header/"
    Copy-Item "$($baseClassLibLocation)/header/*" -Destination "$($testLocation)/header/" -Force

    if (!(Test-Path "$($testLocation)/bin/testId.o")) {
        printAndRun -command "$($testLocation)/llCompiler.exe -c $($testLocation)/programs/testId.ll"
        Move-Item "$($testLocation)/programs/testId.S" -Destination "$($testLocation)/bin/" -Force
        printAndRun -command "$($testLocation)/llCompiler.exe -h $($testLocation)/programs/testId.ll"
        Move-Item "$($testLocation)/programs/testId.ll" -Destination "$($testLocation)/programs/testId.bak"
    }

    printAndRun -command "$($testLocation)/llCompiler.exe -c $($testLocation)/programs/testCodeGenV1Prog.ll"
    Move-Item "$($testLocation)/programs/*.S" -Destination "$($testLocation)/bin/" -Force
}

function test() {
    publishWindows
    compileRuntime
    compileBaseClassLibrary
    packageRuntime
    compileTest
    compileAssember
    linkTest
    printMessage -message "Running tests"
    printAndRun -command "wsl $($testLocation)/bin/testCodeGen"

    if($LASTEXITCODE -gt 0) {
        printError -message "Tests not successfull. `t$($LASTEXITCODE) tests failed"
    } else {
        printMessage -message "All tests successfull"
    }
}

function generateCode() {
    printMessage -message "Generating code from grammar"
    Write-Host "cd $($compilerLocation)"
    Set-Location $compilerLocation
    printAndRun -command "java -jar ../deps/antlr-4.9.1-complete.jar -Dlanguage=CSharp ll.g4 -no-listener -visitor  -package LL"
    Write-Host "cd ../"
    Set-Location "../"
}

function rtfm() {
    Write-Host "Usage: make.ps1 [argument]"
    Write-Host "Allowed arguments:"
    Write-Host "`tclean`t`t- cleans up the repository"
    Write-Host "`trestore`t`t- restores dotnet rependencies"
    Write-Host "`tpublishWindows`t- publishes the compiler for windows"
    Write-Host "`tpublishLinux`t- publishes the compiler for linux"
    Write-Host "`tpublishOSX`t- publishes the compiler for OSX"
    Write-Host "`tpublishAll`t- publishes the compiler for all previously specified plattforms"
    Write-Host "`ttest`t`t- runs the tests"
    Write-Host "`tgenerateCode`t- generates code from antlr grammar"
}

if ($args.Count -le 0 -or $args.Count -gt 1) {
    Write-Host "Missing argument" -ForegroundColor Red
    rtfm
    Return
}

switch ($args[0]) {
    "clean" {
        clean
    }
    "restore" {
        restore
    }
    "publishWindows" {
        publishWindows
    }
    "publishLinux" {
        publishLinux
    }
    "publishOSX" {
        publishOSX
    }
    "publishAll" {
        publishAll
    }
    "test" {
        test
    }
    "generateCode" {
        generateCode
    }
    Default {
        Write-Host "Unknown argument: " -ForegroundColor Red
        Write-Host "`t$($args[0])`n"
        rtfm
    }
}
