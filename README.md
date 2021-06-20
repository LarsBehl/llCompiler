# llCompiler
This is the repository for the compiler was written in the 5th semester at the University of Applied Science RheinMain as part of the lecture *compiler construction*. The compiler is written in `C#`.

Work on the compiler is currently continued as part of a project during the masters degree in computer science.

### Master build status
[![Build Status](https://dev.azure.com/larsbehl/larsbehl/_apis/build/status/LarsBehl.llCompiler?branchName=master)](https://dev.azure.com/larsbehl/larsbehl/_build/latest?definitionId=1&branchName=master)

### Development build status
[![Build Status](https://dev.azure.com/larsbehl/larsbehl/_apis/build/status/LarsBehl.llCompiler?branchName=development)](https://dev.azure.com/larsbehl/larsbehl/_build/latest?definitionId=1&branchName=development)

### Feature in progess:
*OS interop and base class library*


## Datatypes
There are for different primitiv data types supported by the compiler
* `int` - 64 Bit integer. Other languages often refer to it as `long`
* `double` - 64 Bit double precision floating point number
* `bool` - Boolean value
* `char` - 8 Bit ascii value.

In addition there are the reference data types
* `structs` - Structures that hold properties with potentially differnt type of data. Comparable with C structs
* `arrays` - A memory section that can hold x times the given data type; Arrays of arrays and arrays of structs are not supported at the moment

To easily intialize `char[]` string literals were added. Please note that for each string literal a heap object will be created on runtime. This could introduce memory leaks if a string literal is used as a return value or passed to a function call. With further iterations of the compiler, the garbage collection will remove these potential memory leaks.

The only exception of the heap object creation for string literals is currently the `print()` function. When a string literal is passed to the `print()` function, no heap object is created. As soon as the base class library is implemented, this exception will no longer be existent.

## Functions
To prevent repetitive code, LL supports the defintion of functions. These functions can return values of one of the five integrated data types. It is also possible to create `void` returing functions.

If a function returns a reference type, it is also possible to return `null`.

## Global variables
With the addition of global variables a user can now define constants that may be used by multiple functions. This is usefull when, for example, implementing interaction with the operating system like reading and writing files. The file descriptors 0, 1 and 2 are always opened up for an application as `stdin`, `stdout` and `stderr`. Those can now be defined as global variables and used when writing or reading.

## Module system
With the latest changes the compiler now supports splitting up the code into multiple source code files. This enables the users to clean up the code and support a more modular structure. It is important to note, that for each source code document used in an application, a seperate assembler file will be generated.

In addition, header-like files were added. These files are needed when interfacing with binaries. For LL-Files it is possible to automatically generate the header files from the source code by running the compiler in header generation mode:
```bash
llCompiler -h [sourceFile]
```
Like the compilation of source code, the header generation is performed for each dependency recursively (if `file1.ll` depends on `file2.ll` and `file2.ll` on `file3.ll` headers will be generated for all three files). This is especially usefull when implementing a library which will be distributed in binary format.

## Interfacing with C libraries
The previously mentioned header files enables the user to interface with binaries written in or developed for `C`. To use the library, one simply has to add a header file containing the function prototypes and struct definitions of the library that will be in use. Load the header file at the appropriate position in the code and the compiler will treat any calls to functions like the library is present as source code. Compile the assembler code with the help of gcc and link the libraries in use. The created binary *should* work as expected.

## Compilation
To compile the LL-Compiler you need to have .NET 5.0 installed. To compile the LL-Compiler run the following command:
```bash
./make.ps1 publishWindows
```
Make sure that powershell script execution is set correctly.

For the Linux users there is a `MAKEFILE`. You can run this command to compile the LL-Compiler:
```bash
make publishWindows
```

When you are done with the compilation, navigate to `./bin`. In there you can find the folders `win10`, `linux` or `OSX`. Each of the folders contains an executable for the corresponding operating system and all needed dependencies.

## Running compiled LL-Code
The by the LL-Compiler generated assembler code uses AT & T syntax. It is recommended to use a linux based operting system when trying to compile the assembler code. For windows useres the easiest way is to use [WSL](https://docs.microsoft.com/en-us/windows/wsl/about) or [WSL2](https://docs.microsoft.com/en-us/windows/wsl/about#what-is-wsl-2). It is also possible to compile the code using mingw, but it has **not** been tested.

## Examples
In the demo folder you can find some examples of LL-Code.

## The Future
It is currently planned to support multiple target platforms like x86 assembler and the intermediate language.

Additionally a runtime environment with support for garbage collection is in work.

It is also planned to completely rework the language to be object oriented.