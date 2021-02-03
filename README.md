# llCompiler
This is the repository for the compiler that has to be written in the 5th semester at the University of Applied Science RheinMain. The compiler is written in C#

### Master build status
[![Build Status](https://dev.azure.com/larsbehl/larsbehl/_apis/build/status/LarsBehl.llCompiler?branchName=master)](https://dev.azure.com/larsbehl/larsbehl/_build/latest?definitionId=1&branchName=master)

### Development build status
[![Build Status](https://dev.azure.com/larsbehl/larsbehl/_apis/build/status/LarsBehl.llCompiler?branchName=development)](https://dev.azure.com/larsbehl/larsbehl/_build/latest?definitionId=1&branchName=development)


## Datatypes
The llCompiler knows three different primitiv data types. These are
* `int` - 64 Bit integer. Other languages often refer to it as `long`
* `double` - 64 Bit double precision floating point number
* `bool` - Boolean value

In addition LL knows to reference data types
* `structs` - Structures that hold properties with potentially differnt type of data. Comparable with C structs
* `arrays` - A memory section that can hold x times the given data type; Arrays of arrays and arrays of structs are not supported at the moment

## Functions
To prevent repetitive code, LL supports the defintion of functions. These functions can return values of one of the five integrated data types. It is also possible to create `void` returing functions.

If a function returns a reference type, it is also possible to return `null`.

## Compilation
To compile the LL-Compiler you need to have .NET 5.0 installed. To compile the LL-Compiler navigate to `./compiler` and run the batch file `publish.bat`:
```bash
./publish.bat
```

For the Linux users there is a `MAKEFILE` in the `./compiler` folder. You can run this command to compile the LL-Compiler:
```bash
make
```

When you are done with the compilation, navigate to `./bin`. In there you can find the folders `win10`, `unix` and `OSX`. Each of the folders contains one executable for the corresponding operating system.

## Examples
In the demo folder you can find some examples of LL-Code.

## The Future
One of the biggest features planned is to support multiple target languages. Currently the code is compiled to x86 Assembler with AT & T syntax. With further iterations the compiler is going to support the intermediate language. The compiled program could then be run in the .NET Core Runtime. This means that when compiling the user could decide wich platform he wants to target
