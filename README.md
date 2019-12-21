# llCompiler
This is the repository for the compiler that has to be written in the 5th semester at the University of Applied Science RheinMain. The compiler is written in C#

[![Build Status](https://dev.azure.com/larsbehl/larsbehl/_apis/build/status/LarsBehl.llCompiler?branchName=master)](https://dev.azure.com/larsbehl/larsbehl/_build/latest?definitionId=1&branchName=master)

## Datatypes
The llCompiler knows three different primitiv data types. These are
* int - 64 Bit integer. Other languages often refer to it as "long"
* double - 64 Bit double precision floating point number
* bool - Boolean value

It is also possible to mark a function as void returning. In this case, the function does not return any value.