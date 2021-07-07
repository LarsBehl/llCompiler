load testBinOps;
load testId;
load testWhile;
load testIf;
load testAssign;
load testUnary;
load testStructs;
load util;
load testIO;

global testGlobalVariableInt: int = 42;
global testGlobalVariableDouble: double = 18.7;
global testGlobalVariableChar: char = 'c';
global testGlobalVariableBool: bool = true;
global testGlobalArray: int[] = new int[5];
global testGlobalString: char[] = "Hallo Welt\n";

testIntArray(x:int): int
{
    y: int[] = new int[17];
    y[12] = x;
    z: int = y[12];
    destroy y;

    return z;
}

testDoubleArray(x:double): double
{
    y: double[] = new double[17];
    y[12] = x;
    z: double = y[12];
    destroy y;

    return z;
}

testBoolArray(x:bool): bool
{
    y: bool[] = new bool[17];
    y[12] = x;
    z: bool = y[12];
    destroy y;

    return z;
}

testCharArray(x:char): char
{
    y: char[] = new char[42];
    y[21] = x;
    z: char = y[21];
    destroy y;

    return z;
}

factorial(x:int): int
{
    if(x > 1)
    {
        return x * factorial(x-1);
    }
    return 1;
}

callMultipleOthers(x:int):int
{
    i:int = multIntInt(x, 2);
    i = addIntInt(i, 5);

    return i;
}

overFlowOnlyInt(x:int, y:int, z:int, a:int, b:int, c:int, d:int, e:int, f:int): int
{
    return x+y+z+a+b+c+d+e+f;
}

overFlowOnlyDouble(x:double, y:double, z:double, a:double, b:double, c:double, d:double, e:double, f:double): double
{
    return x+y+z+a+b+c+d+e+f;
}

overFlowIntMixed(x:int, y:double, z:int, a:double, b:int, c:int, d:int, e:int, f:int, g:int): double
{
    return x+y+z+a+b+c+d+e+f+g;
}

overFlowDoubleMixed(x:double, y:int, z:double, a:int, b:double, c:double, d:double, e:double, f:double): double
{
    return x+y+z+a+b+c+d+e+f;
}

 overFlowBoth(x:double, y:double, z:double, a:double, b:double, c:double, d:double, e:double, f:int, g:int, h:int, i:int, j:int, k:int, l:int, m:int, n:int): double
{
    return x+y+z+a+b+c+d+e+f+g+h+i+j+k+l+m+n;
}

callIntDouble(x: int): double
{
    return doubleFunctionInt(x);
}

doubleFunctionInt(x: double): double
{
    y: double = x;

    return y;
}

assignIntDoubleArray(x: int): double
{
    y: double[] = new double[5];
    y[0] = x;
    result: double = y[0];
    destroy y;

    return result;
}

evalInt(expected: int, val: int): void
{
    testData.testCount++;
    result: bool = expected == val;

    if(result)
    {
        testData.successCount++;
    }
    else
    {
        print("Expected:");
        print(expected);
        print("Actual:");
        print(val);
    }

    print(result);
}

evalDouble(expected: double, val: double): void
{
    testData.testCount++;
    result: bool = expected == val;

    if(result)
    {
        testData.successCount++;
    }

    print(result);
}

evalBool(expected: bool, val: bool): void
{
    testData.testCount++;
    result: bool = expected == val;

    if(result)
    {
        testData.successCount++;
    }

    print(result);
}

evalChar(expected: char, val: char): void
{
    testData.testCount++;
    result: bool = expected == val;

    if(result)
    {
        testData.successCount++;
    }

    print(result);
}

main(): void
{
    print("Running assemlber Tests");
    intResult: int;
    doubleResult: double;
    boolResult: bool;
    charResult: char;

    intResult = intId(42);
    evalInt(42, intResult);
    doubleResult = doubleId(42.0);
    evalDouble(42.0, doubleResult);
    boolResult = boolId(true);
    evalBool(true, boolResult);
    charResult = charId('c');
    evalChar('c', charResult);

    intResult = addIntInt(17, 25);
    evalInt(42, intResult);
    doubleResult = addIntDouble(17, 25.5);
    evalDouble(42.5, doubleResult);
    doubleResult = addDoubleInt(17.5, 17);
    evalDouble(34.5, doubleResult);
    doubleResult = addDoubleDouble(13.2, 28.8);
    evalDouble(42.0, doubleResult);

    intResult = subIntInt(27, 15);
    evalInt(12, intResult);
    doubleResult = subIntDouble(17, 3.2);
    evalDouble(13.8, doubleResult);
    doubleResult = subDoubleInt(17.5, 2);
    evalDouble(15.5, doubleResult);
    doubleResult = subDoubleDouble(7.7, 0.5);
    evalDouble(7.2, doubleResult);

    intResult = multIntInt(3, 15);
    evalInt(45, intResult);
    doubleResult = multIntDouble(2, 5.5);
    evalDouble(11.0, doubleResult);
    doubleResult = multDoubleInt(2.5, 2);
    evalDouble(5.0, doubleResult);
    doubleResult = multDoubleDouble(0.5, 2.5);
    evalDouble(1.25, doubleResult);

    intResult = divIntInt(3, 2);
    evalInt(1, intResult);
    doubleResult = divIntDouble(2, 0.5);
    evalDouble(4, doubleResult);
    doubleResult = divDoubleInt(5.0, 2);
    evalDouble(2.5, doubleResult);
    doubleResult = divDoubleDouble(0.5, 0.5);
    evalDouble(1.0, doubleResult);

    boolResult = greaterIntInt(5, 4);
    evalBool(true, boolResult);
    boolResult = greaterIntDouble(5, 4.2);
    evalBool(true, boolResult);
    boolResult = greaterDoubleInt(5.0, 2);
    evalBool(true, boolResult);
    boolResult = greaterDoubleDouble(7.2, 7.199999999);
    evalBool(true, boolResult);

    boolResult = lessIntInt(2, 3);
    evalBool(true, boolResult);
    boolResult = lessIntDouble(2, 3.5);
    evalBool(true, boolResult);
    boolResult = lessDoubleInt(2.0, 5);
    evalBool(true, boolResult);
    boolResult = lessDoubleDouble(2.1999999, 2.2);
    evalBool(true, boolResult);

    boolResult = equalIntInt(3, 3);
    evalBool(true, boolResult);
    boolResult = equalIntDouble(3, 3.0);
    evalBool(true, boolResult);
    boolResult = equalDoubleInt(2.0, 2);
    evalBool(true, boolResult);
    boolResult = equalDoubleDouble(3.0, 3.0);
    evalBool(true, boolResult);
    boolResult = equalArrayArray();
    evalBool(false, boolResult);
    boolResult = equalArrayNull();
    evalBool(false, boolResult);
    boolResult = equalNullArrayNull();
    evalBool(true, boolResult);
    boolResult = equalCharChar('x', 'x');
    evalBool(true, boolResult);
    boolResult = equalCharChar('x', 'c');
    evalBool(false, boolResult);

    intResult = withoutElse(42);
    evalInt(17, intResult);
    intResult = withoutElse(17);
    evalInt(42, intResult);
    intResult = withElse(42);
    evalInt(17, intResult);
    intResult = withElse(17);
    evalInt(42, intResult);
    intResult = withoutElseReturning(42);
    evalInt(17, intResult);
    intResult = withoutElseReturning(17);
    evalInt(42, intResult);
    intResult = withElseReturning(42);
    evalInt(17, intResult);
    intResult = withElseReturning(17);
    evalInt(42, intResult);

    intResult = returningWhile(42);
    evalInt(42, intResult);
    intResult = returningWhile(17);
    evalInt(17, intResult);
    intResult = notReturningWhile(17);
    evalInt(17, intResult);

    intResult = addAssignIntInt(17, 25);
    evalInt(42, intResult);
    doubleResult = addAssignIntDouble(17, 25.5);
    evalDouble(42.5, doubleResult);
    doubleResult = addAssignDoubleInt(3.5, 7);
    evalDouble(10.5, doubleResult);
    doubleResult = addAssignDoubleDouble(16.5, 25.5);
    evalDouble(42.0, doubleResult);

    intResult = subAssignIntInt(17, 15);
    evalInt(2, intResult);
    doubleResult = subAssignIntDouble(17, 15.5);
    evalDouble(1.5, doubleResult);
    doubleResult = subAssignDoubleInt(2.5, 2);
    evalDouble(0.5, doubleResult);
    doubleResult = subAssignDoubleDouble(17.2, 0.2);
    evalDouble(17.0, doubleResult);

    intResult = multAssignIntInt(3, 5);
    evalInt(15, intResult);
    doubleResult = multAssignIntDouble(3, 5.5);
    evalDouble(16.5, doubleResult);
    doubleResult = multAssignDoubleInt(2.6, 2);
    evalDouble(5.2, doubleResult);
    doubleResult = multAssignDoubleDouble(0.5, 1.5);
    evalDouble(0.75, doubleResult);

    intResult = divAssignIntInt(3, 2);
    evalInt(1, intResult);
    doubleResult = divAssignIntDouble(3, 0.5);
    evalDouble(6.0, doubleResult);
    doubleResult = divAssignDoubleInt(6.2, 2);
    evalDouble(3.1, doubleResult);
    doubleResult = divAssignDoubleDouble(0.5, 0.5);
    evalDouble(1.0, doubleResult);

    intResult = incrementPostInt(4);
    evalInt(4, intResult);
    intResult = incrementPreInt(4);
    evalInt(5, intResult);
    doubleResult = incrementPostDouble(2.5);
    evalDouble(2.5, doubleResult);
    doubleResult = incrementPreDouble(2.5);
    evalDouble(3.5, doubleResult);

    intResult = decrementPostInt(4);
    evalInt(4, intResult);
    intResult = decrementPreInt(4);
    evalInt(3, intResult);
    doubleResult = decrementPostDouble(2.5);
    evalDouble(2.5, doubleResult);
    doubleResult = decrementPreDouble(2.5);
    evalDouble(1.5, doubleResult);

    boolResult = notOperator(true);
    evalBool(false, boolResult);
    boolResult = notOperator(false);
    evalBool(true, boolResult);

    boolResult = andOperator(true, true);
    evalBool(true, boolResult);
    boolResult = andOperator(false, true);
    evalBool(false, boolResult);
    boolResult = andOperator(true, false);
    evalBool(false, boolResult);
    boolResult = andOperator(false, false);
    evalBool(false, boolResult);

    boolResult = orOperator(true, true);
    evalBool(true, boolResult);
    boolResult = orOperator(true, false);
    evalBool(true, boolResult);
    boolResult = orOperator(false, true);
    evalBool(true, boolResult);
    boolResult = orOperator(false, false);
    evalBool(false, boolResult);

    boolResult = notEqualIntInt(1, 2);
    evalBool(true, boolResult);
    boolResult = notEqualIntDouble(1, 1.0);
    evalBool(false, boolResult);
    boolResult = notEqualDoubleInt(1.0, 2);
    evalBool(true, boolResult);
    boolResult = notEqualDoubleDouble(1.0, 1.0);
    evalBool(false, boolResult);
    boolResult = notEqualBoolBool(true, false);
    evalBool(true, boolResult);
    boolResult = notEqualCharChar('x', 'c');
    evalBool(true, boolResult);
    boolResult = notEqualCharChar('x', 'x');
    evalBool(false, boolResult);

    boolResult = notEqualArrayArray();
    evalBool(true, boolResult);
    boolResult = notEqualArrayNull();
    evalBool(true, boolResult);
    boolResult = notEqualNullArrayNull();
    evalBool(false, boolResult);

    intResult = testIntArray(42);
    evalInt(42, intResult);
    doubleResult = testDoubleArray(27.3);
    evalDouble(27.3, doubleResult);
    boolResult = testBoolArray(false);
    evalBool(false, boolResult);
    charResult = testCharArray('x');
    evalChar('x', charResult);

    intResult = overFlowOnlyInt(1, 1, 1, 1, 1, 1, 1, 1, 1);
    evalInt(9, intResult);
    doubleResult = overFlowOnlyDouble(1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
    evalDouble(9.0, doubleResult);
    doubleResult = overFlowIntMixed(1, 1.0, 1, 1.0, 1, 1, 1, 1, 1, 1);
    evalDouble(10.0, doubleResult);
    doubleResult = overFlowDoubleMixed(1.0, 1, 1.0, 1, 1.0, 1.0, 1.0, 1.0, 1.0);
    evalDouble(9.0, doubleResult);
    doubleResult = overFlowBoth(1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1, 1, 1, 1, 1, 1, 1, 1, 1);
    evalDouble(17.0, doubleResult);

    intResult = factorial(4);
    evalInt(24, intResult);

    intResult = callMultipleOthers(7);
    evalInt(19, intResult);

    intResult = incrementPreArray(41);
    evalInt(42, intResult);
    intResult = incrementPostArray(42);
    evalInt(42, intResult);
    intResult = decrementPreArray(43);
    evalInt(42, intResult);
    intResult = decrementPostArray(42);
    evalInt(42, intResult);

    intResult = incrementPreStruct(41);
    evalInt(42, intResult);
    intResult = incrementPostStruct(42);
    evalInt(42, intResult);
    intResult = decrementPreStruct(43);
    evalInt(42, intResult);
    intResult = decrementPostStruct(42);
    evalInt(42, intResult);

    intResult = accessFirstInnerStruct(42);
    evalInt(42, intResult);
    intResult = accessSecondInnerStruct(42);
    evalInt(42, intResult);
    intResult = accessInnerArray(42);
    evalInt(42, intResult);
    intResult = assignNullStructProp();
    evalInt(42, intResult);

    intResult = modExpr(5, 2);
    evalInt(1, intResult);
    intResult = modExpr(-1, 3);
    evalInt(-1, intResult);
    intResult = modExpr(4, 2);
    evalInt(0, intResult);

    doubleResult = callIntDouble(42);
    evalDouble(42.0, doubleResult);
    doubleResult = assignIntDoubleArray(17);
    evalDouble(17.0, doubleResult);
    doubleResult = assignIntDoubleStruct(3);
    evalDouble(3.0, doubleResult);

    intResult = writeStdout(testGlobalString, 11);
    evalInt(11, intResult);
    intResult = openTestIo();
    fd: int = intResult;
    evalInt(3, intResult);
    intResult = readTestIo(fd, 16);
    testData.testCount++;
    if(intResult <= 16 && intResult >= 0)
    {
        testData.successCount++;
        print(true);
    }
    else
    {
        print(false);
    }
    fstatTestIo(fd);
    print(true);
    testData.testCount++;
    testData.successCount++;
    closeTestIo(fd);
    print(true);
    testData.testCount++;
    testData.successCount++;

    print("Tests:");
    print(testData.testCount);
    print("Successfull tests:");
    print(testData.successCount);

    tests: int = testData.testCount;
    success: int = testData.successCount;
    destroy testGlobalString;
    destroy testGlobalArray;
    destroy testData;

    exitProgram(tests-success);
}