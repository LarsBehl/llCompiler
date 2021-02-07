$compilerLocation = './compiler'
$testLocation = './testGeneratedCode'
$runtimeLocation = './runtime'

function printMessage($message) {
    Write-Host "`n`n$($message)...`n" -ForegroundColor DarkGreen
}

function printAndRun($command) {
    Write-Host "$($command)"
    $result = Invoke-Expression $command
    Write-Host "$($result)"
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
    remove -file "$($testLocation)/*.dll"
    remove -file "$($testLocation)/*.exe"
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
    Write-Host "cp $($runtimeLocation)/bin/libLL.a $($testLocation)/bin/"
    Copy-Item "$($runtimeLocation)/bin/libLL.a" -Destination "$($testLocation)/bin/"
    printAndRun -command "wsl gcc -o $($testLocation)/bin/testCodeGen $($testLocation)/bin/runner.o $($testLocation)/bin/testCodeGenV1.o $($testLocation)/bin/testCodeGenV1Prog.o -LtestGeneratedCode/bin -lLL"
}

function compileAssember() {
    printMessage -message "Compiling tests"
    printAndRun -command "wsl gcc -c -g $($testLocation)/util/runner.c -o $($testLocation)/bin/runner.o"
    printAndRun -command "wsl gcc -c -g $($testLocation)/util/testCodeGenV1.c -o $($testLocation)/bin/testCodeGenV1.o"
    printAndRun -command "wsl gcc -c -g $($testLocation)/bin/testCodeGenV1Prog.S -o $($testLocation)/bin/testCodeGenV1Prog.o"
}

function packageRuntime() {
    printMessage -message "Packaging runtime lib"
    printAndRun -command "wsl ar rcs $($runtimeLocation)/bin/libLL.a $($runtimeLocation)/bin/runtime.o $($runtimeLocation)/bin/errors.o $($runtimeLocation)/bin/addrList.o $($runtimeLocation)/bin/classData.o $($runtimeLocation)/bin/classDataList.o"
}

function compileRuntime() {
    printMessage -message "Compiling runtime"
    if(!(Test-Path "$($runtimeLocation)/bin")) {
        New-Item -ItemType Directory -Force -Path "$($runtimeLocation)/bin"
    }
    printAndRun -command "wsl gcc -c -g $($runtimeLocation)/runtime.c -o $($runtimeLocation)/bin/runtime.o"
    printAndRun -command "wsl gcc -c -g $($runtimeLocation)/errors.c -o $($runtimeLocation)/bin/errors.o"
    printAndRun -command "wsl gcc -c -g $($runtimeLocation)/addrList.c -o $($runtimeLocation)/bin/addrList.o"
    printAndRun -command "wsl gcc -c -g $($runtimeLocation)/classData.c -o $($runtimeLocation)/bin/classData.o"
    printAndRun -command "wsl gcc -c -g $($runtimeLocation)/classDataList.c -o $($runtimeLocation)/bin/classDataList.o"
}

function compileTest() {
    printMessage -message "Compiling ll code"
    Write-Host "cp $($compilerLocation)/bin/win10/* $($testLocation)/"
    Copy-Item "$($compilerLocation)/bin/win10/*" -Destination "$($testLocation)/"

    if(!(Test-Path "$($testLocation)/bin")) {
        New-Item -ItemType Directory -Force -Path "$($testLocation)/bin"
    }

    printAndRun -command "$($testLocation)/llCompiler.exe -c $($testLocation)/programs/testCodeGenV1Prog.ll"
    Move-Item "$($testLocation)/programs/testCodeGenV1Prog.S" -Destination "$($testLocation)/bin/testCodeGenV1Prog.S" -Force
}

function test() {
    publishWindows
    compileTest
    compileRuntime
    packageRuntime
    compileAssember
    linkTest
    printMessage -message "Running tests"
    printAndRun -command "wsl $($testLocation)/bin/testCodeGen"
}

function generateCode() {
    printMessage -message "Generating code from grammar"
    Write-Host "cd $($compilerLocation)"
    Set-Location $compilerLocation
    printAndRun -command "java -jar ../deps/antlr-4.9.1-complete.jar -Dlanguage=CSharp ll.g4 -no-listener -visitor  -package ll"
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
    Write-Host "`tgenerateCode`t`t- generates code from antlr grammar"
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

    }
    Default {
        Write-Host "Unknown argument: " -ForegroundColor Red
        Write-Host "`t$($args[0])`n"
        rtfm
    }
}
