load testBinOps;
load testId;

withoutElse(x:int):int
{
    result:int = 42;

    if(x == 42)
    {
        result = 17;
    }

    return result;
}

withElse(x:int):int
{
    result:int = 0;

    if(x == 42)
    {
        result = 17;
    }
    else
    {
        result = 42;
    }

    return result;
}

withoutElseReturning(x:int):int
{
    if(x == 42)
    {
        return 17;
    }
    
    return 42;
}

withElseReturning(x:int): int
{
    if(x == 42)
    {
        return 17;
    }
    else
    {
        return 42;
    }
}

returningWhile(x:int):int
{
    while(true)
    {
        return x;
    }

    return -1;
}

notReturningWhile(x:int):int
{
    i:int = 0;
    while(i < x)
    {
        i += 1;
    }

    return i;
}

addAssignIntInt(x:int, y:int): int
{
    i: int = x;
    i += y;

    return i;
}

addAssignIntDouble(x:int, y:double): double
{
    i: double = x;
    i += y;

    return i;
}

addAssignDoubleInt(x:double, y:int): double
{
    i: double = x;
    i += y;

    return i;
}

addAssignDoubleDouble(x:double, y:double): double
{
    i: double = x;
    i += y;

    return i;
}

subAssignIntInt(x:int, y:int): int
{
    i: int = x;
    i -= y;

    return i;
}

subAssignIntDouble(x:int, y:double): double
{
    i: double = x;
    i -= y;

    return i;
}

subAssignDoubleInt(x:double, y:int): double
{
    i: double = x;
    i -= y;

    return i;
}

subAssignDoubleDouble(x:double, y:double): double
{
    i: double = x;
    i -= y;

    return i;
}

multAssignIntInt(x:int, y:int): int
{
    i: int = x;
    i *= y;

    return i;
}

multAssignIntDouble(x:int, y:double): double
{
    i: double = x;
    i *= y;

    return i;
}

multAssignDoubleInt(x:double, y:int): double
{
    i: double = x;
    i *= y;

    return i;
}

multAssignDoubleDouble(x:double, y:double): double
{
    i: double = x;
    i *= y;

    return i;
}

divAssignIntInt(x:int, y:int): int
{
    i: int = x;
    i /= y;

    return i;
}

divAssignIntDouble(x:int, y:double): double
{
    i: double = x;
    i /= y;

    return i;
}

divAssignDoubleInt(x:double, y:int): double
{
    i: double = x;
    i /= y;
    
    return i;
}

divAssignDoubleDouble(x:double, y:double): double
{
    i: double = x;
    i /= y;

    return i;
}

incrementPostInt(x:int): int
{
    return x++;
}

incrementPreInt(x:int): int
{
    return ++x;
}

incrementPostDouble(x:double): double
{
    return x++;
}

incrementPreDouble(x:double): double
{
    return ++x;
}

decrementPostInt(x:int): int
{
    return x--;
}

decrementPreInt(x:int): int
{
    return --x;
}

decrementPostDouble(x:double): double
{
    return x--;
}

decrementPreDouble(x:double): double
{
    return --x;
}

notOperator(x:bool): bool
{
    return !x;
}

andOperator(x:bool, y:bool): bool
{
    return x && y;
}

orOperator(x:bool, y:bool): bool
{
    return x || y;
}

notEqualIntInt(x:int, y:int): bool
{
    return x != y;
}

notEqualIntDouble(x:int, y:double): bool
{
    return x != y;
}

notEqualDoubleInt(x:double, y:int): bool
{
    return x != y;
}

notEqualDoubleDouble(x:double, y:double): bool
{
    return x != y;
}

notEqualBoolBool(x:bool, y:bool): bool
{
    return x != y;
}

notEqualArrayArray(): bool
{
    x: int[] = new int[5];
    y: int[] = new int[10];

    return x != y;
}

notEqualArrayNull(): bool
{
    x: int[] = new int[5];

    return x != null;
}

notEqualNullArrayNull(): bool
{
    x: int[] = null;

    return null != x;
}

testPrintBool(x:bool): void
{
    print(x);
}

testPrintInt(x:int): void
{
    print(x);
}

testPrintDouble(x:double): void
{
    print(x);
}

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

incrementPreArray(x: int): int
{
    y: int[] = new int[5];
    y[2] = x;
    result: int = ++y[2];

    destroy y;

    return result;
}

incrementPostArray(x: int): int
{
    y: int[] = new int[5];
    y[2] = x;
    result: int = y[2]++;

    destroy y;

    return result;
}

decrementPreArray(x: int): int
{
    y: int[] = new int[5];
    y[2] = x;
    result: int = --y[2];

    destroy y;

    return result;
}

decrementPostArray(x: int): int
{
    y: int[] = new int[5];
    y[2] = x;
    result: int = y[2]--;

    destroy y;

    return result;
}

struct Point
{
    x: int;
    y: int;
}

incrementPreStruct(x: int): int
{
    p: Point = new Point();
    p.x = x;
    result: int = ++p.x;
    destroy p;
    return result;
}

incrementPostStruct(x:int): int
{
    p: Point = new Point();
    p.x = x;
    result: int = p.x++;
    destroy p;
    return result;
}

decrementPreStruct(x:int): int
{
    p: Point = new Point();
    p.x = x;
    result: int = --p.x;
    destroy p;
    return result;
}

decrementPostStruct(x:int): int
{
    p: Point = new Point();
    p.x = x;
    result: int = p.x--;
    destroy p;
    return result;
}

struct Line 
{
    p1: Point;
    p2: Point;
}

accessFirstInnerStruct(x:int): int
{
    l: Line = new Line();
    l.p1 = new Point();
    l.p1.x = 42;

    return l.p1.x;
}

accessSecondInnerStruct(x:int): int
{
    l: Line = new Line();
    l.p2 = new Point();
    l.p2.y = 42;

    return l.p2.y;
}

struct iA
{
    array: int[];
}

accessInnerArray(x:int): int
{
    a: iA = new iA();
    a.array = new int[10];
    a.array[5] = 42;

    return a.array[5];
}

assignNullStructProp(): int
{
    a: iA = new iA();
    a.array = null;

    return 42;
}

struct doubleStruct
{
    d: double;
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

    return y[0];
}

assignIntDoubleStruct(x: int): double
{
    y: doubleStruct = new doubleStruct();
    y.d = x;
    result: double = y.d;
    destroy y;

    return result;
}

modExpr(x: int, y: int): int
{
    return x % y;
}

struct TestData
{
    testCount: int;
    successCount: int;
}

evalInt(expected: int, val: int, testData: TestData): void
{
    testData.testCount++;
    result: bool = expected == val;

    if(result)
    {
        testData.successCount++;
    }

    print(result);
}

evalDouble(expected: double, val: double, testData: TestData): void
{
    testData.testCount++;
    result: bool = expected == val;

    if(result)
    {
        testData.successCount++;
    }

    print(result);
}

evalBool(expected: bool, val: bool, testData: TestData): void
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
    testData: TestData = new TestData();
    intResult: int;
    doubleResult: double;
    boolResult: bool;

    intResult = intId(42);
    evalInt(42, intResult, testData);
    doubleResult = doubleId(42.0);
    evalDouble(42.0, doubleResult, testData);
    boolResult = boolId(true);
    evalBool(true, boolResult, testData);

    intResult = addIntInt(17, 25);
    evalInt(42, intResult, testData);
    doubleResult = addIntDouble(17, 25.5);
    evalDouble(42.5, doubleResult, testData);
    doubleResult = addDoubleInt(17.5, 17);
    evalDouble(34.5, doubleResult, testData);
    doubleResult = addDoubleDouble(13.2, 28.8);
    evalDouble(42.0, doubleResult, testData);

    intResult = subIntInt(27, 15);
    evalInt(12, intResult, testData);
    doubleResult = subIntDouble(17, 3.2);
    evalDouble(13.8, doubleResult, testData);
    doubleResult = subDoubleInt(17.5, 2);
    evalDouble(15.5, doubleResult, testData);
    doubleResult = subDoubleDouble(7.7, 0.5);
    evalDouble(7.2, doubleResult, testData);

    intResult = multIntInt(3, 15);
    evalInt(45, intResult, testData);
    doubleResult = multIntDouble(2, 5.5);
    evalDouble(11.0, doubleResult, testData);
    doubleResult = multDoubleInt(2.5, 2);
    evalDouble(5.0, doubleResult, testData);
    doubleResult = multDoubleDouble(0.5, 2.5);
    evalDouble(1.25, doubleResult, testData);

    intResult = divIntInt(3, 2);
    evalInt(1, intResult, testData);
    doubleResult = divIntDouble(2, 0.5);
    evalDouble(4, doubleResult, testData);
    doubleResult = divDoubleInt(5.0, 2);
    evalDouble(2.5, doubleResult, testData);
    doubleResult = divDoubleDouble(0.5, 0.5);
    evalDouble(1.0, doubleResult, testData);

    boolResult = greaterIntInt(5, 4);
    evalBool(true, boolResult, testData);
    boolResult = greaterIntDouble(5, 4.2);
    evalBool(true, boolResult, testData);
    boolResult = greaterDoubleInt(5.0, 2);
    evalBool(true, boolResult, testData);
    boolResult = greaterDoubleDouble(7.2, 7.199999999);
    evalBool(true, boolResult, testData);

    boolResult = lessIntInt(2, 3);
    evalBool(true, boolResult, testData);
    boolResult = lessIntDouble(2, 3.5);
    evalBool(true, boolResult, testData);
    boolResult = lessDoubleInt(2.0, 5);
    evalBool(true, boolResult, testData);
    boolResult = lessDoubleDouble(2.1999999, 2.2);
    evalBool(true, boolResult, testData);

    boolResult = equalIntInt(3, 3);
    evalBool(true, boolResult, testData);
    boolResult = equalIntDouble(3, 3.0);
    evalBool(true, boolResult, testData);
    boolResult = equalDoubleInt(2.0, 2);
    evalBool(true, boolResult, testData);
    boolResult = equalDoubleDouble(3.0, 3.0);
    evalBool(true, boolResult, testData);
    boolResult = equalArrayArray();
    evalBool(false, boolResult, testData);
    boolResult = equalArrayNull();
    evalBool(false, boolResult, testData);
    boolResult = equalNullArrayNull();
    evalBool(true, boolResult, testData);

    intResult = withoutElse(42);
    evalInt(17, intResult, testData);
    intResult = withoutElse(17);
    evalInt(42, intResult, testData);
    intResult = withElse(42);
    evalInt(17, intResult, testData);
    intResult = withElse(17);
    evalInt(42, intResult, testData);
    intResult = withoutElseReturning(42);
    evalInt(17, intResult, testData);
    intResult = withoutElseReturning(17);
    evalInt(42, intResult, testData);
    intResult = withElseReturning(42);
    evalInt(17, intResult, testData);
    intResult = withElseReturning(17);
    evalInt(42, intResult, testData);

    intResult = returningWhile(42);
    evalInt(42, intResult, testData);
    intResult = returningWhile(17);
    evalInt(17, intResult, testData);
    intResult = notReturningWhile(17);
    evalInt(17, intResult, testData);

    intResult = addAssignIntInt(17, 25);
    evalInt(42, intResult, testData);
    doubleResult = addAssignIntDouble(17, 25.5);
    evalDouble(42.5, doubleResult, testData);
    doubleResult = addAssignDoubleInt(3.5, 7);
    evalDouble(10.5, doubleResult, testData);
    doubleResult = addAssignDoubleDouble(16.5, 25.5);
    evalDouble(42.0, doubleResult, testData);

    intResult = subAssignIntInt(17, 15);
    evalInt(2, intResult, testData);
    doubleResult = subAssignIntDouble(17, 15.5);
    evalDouble(1.5, doubleResult, testData);
    doubleResult = subAssignDoubleInt(2.5, 2);
    evalDouble(0.5, doubleResult, testData);
    doubleResult = subAssignDoubleDouble(17.2, 0.2);
    evalDouble(17.0, doubleResult, testData);

    intResult = multAssignIntInt(3, 5);
    evalInt(15, intResult, testData);
    doubleResult = multAssignIntDouble(3, 5.5);
    evalDouble(16.5, doubleResult, testData);
    doubleResult = multAssignDoubleInt(2.6, 2);
    evalDouble(5.2, doubleResult, testData);
    doubleResult = multAssignDoubleDouble(0.5, 1.5);
    evalDouble(0.75, doubleResult, testData);

    intResult = divAssignIntInt(3, 2);
    evalInt(1, intResult, testData);
    doubleResult = divAssignIntDouble(3, 0.5);
    evalDouble(6.0, doubleResult, testData);
    doubleResult = divAssignDoubleInt(6.2, 2);
    evalDouble(3.1, doubleResult, testData);
    doubleResult = divAssignDoubleDouble(0.5, 0.5);
    evalDouble(1.0, doubleResult, testData);

    intResult = incrementPostInt(4);
    evalInt(4, intResult, testData);
    intResult = incrementPreInt(4);
    evalInt(5, intResult, testData);
    doubleResult = incrementPostDouble(2.5);
    evalDouble(2.5, doubleResult, testData);
    doubleResult = incrementPreDouble(2.5);
    evalDouble(3.5, doubleResult, testData);

    intResult = decrementPostInt(4);
    evalInt(4, intResult, testData);
    intResult = decrementPreInt(4);
    evalInt(3, intResult, testData);
    doubleResult = decrementPostDouble(2.5);
    evalDouble(2.5, doubleResult, testData);
    doubleResult = decrementPreDouble(2.5);
    evalDouble(1.5, doubleResult, testData);

    boolResult = notOperator(true);
    evalBool(false, boolResult, testData);
    boolResult = notOperator(false);
    evalBool(true, boolResult, testData);

    boolResult = andOperator(true, true);
    evalBool(true, boolResult, testData);
    boolResult = andOperator(false, true);
    evalBool(false, boolResult, testData);
    boolResult = andOperator(true, false);
    evalBool(false, boolResult, testData);
    boolResult = andOperator(false, false);
    evalBool(false, boolResult, testData);

    boolResult = orOperator(true, true);
    evalBool(true, boolResult, testData);
    boolResult = orOperator(true, false);
    evalBool(true, boolResult, testData);
    boolResult = orOperator(false, true);
    evalBool(true, boolResult, testData);
    boolResult = orOperator(false, false);
    evalBool(false, boolResult, testData);

    boolResult = notEqualIntInt(1, 2);
    evalBool(true, boolResult, testData);
    boolResult = notEqualIntDouble(1, 1.0);
    evalBool(false, boolResult, testData);
    boolResult = notEqualDoubleInt(1.0, 2);
    evalBool(true, boolResult, testData);
    boolResult = notEqualDoubleDouble(1.0, 1.0);
    evalBool(false, boolResult, testData);
    boolResult = notEqualBoolBool(true, false);
    evalBool(true, boolResult, testData);

    boolResult = notEqualArrayArray();
    evalBool(true, boolResult, testData);
    boolResult = notEqualArrayNull();
    evalBool(true, boolResult, testData);
    boolResult = notEqualNullArrayNull();
    evalBool(false, boolResult, testData);

    intResult = testIntArray(42);
    evalInt(42, intResult, testData);
    doubleResult = testDoubleArray(27.3);
    evalDouble(27.3, doubleResult, testData);
    boolResult = testBoolArray(false);
    evalBool(false, boolResult, testData);

    intResult = overFlowOnlyInt(1, 1, 1, 1, 1, 1, 1, 1, 1);
    evalInt(9, intResult, testData);
    doubleResult = overFlowOnlyDouble(1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
    evalDouble(9.0, doubleResult, testData);
    doubleResult = overFlowIntMixed(1, 1.0, 1, 1.0, 1, 1, 1, 1, 1, 1);
    evalDouble(10.0, doubleResult, testData);
    doubleResult = overFlowDoubleMixed(1.0, 1, 1.0, 1, 1.0, 1.0, 1.0, 1.0, 1.0);
    evalDouble(9.0, doubleResult, testData);
    doubleResult = overFlowBoth(1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1, 1, 1, 1, 1, 1, 1, 1, 1);
    evalDouble(17.0, doubleResult, testData);

    intResult = factorial(4);
    evalInt(24, intResult, testData);

    intResult = callMultipleOthers(7);
    evalInt(19, intResult, testData);

    intResult = incrementPreArray(41);
    evalInt(42, intResult, testData);
    intResult = incrementPostArray(42);
    evalInt(42, intResult, testData);
    intResult = decrementPreArray(43);
    evalInt(42, intResult, testData);
    intResult = decrementPostArray(42);
    evalInt(42, intResult, testData);

    intResult = incrementPreStruct(41);
    evalInt(42, intResult, testData);
    intResult = incrementPostStruct(42);
    evalInt(42, intResult, testData);
    intResult = decrementPreStruct(43);
    evalInt(42, intResult, testData);
    intResult = decrementPostStruct(42);
    evalInt(42, intResult, testData);

    intResult = accessFirstInnerStruct(42);
    evalInt(42, intResult, testData);
    intResult = accessSecondInnerStruct(42);
    evalInt(42, intResult, testData);
    intResult = accessInnerArray(42);
    evalInt(42, intResult, testData);
    intResult = assignNullStructProp();
    evalInt(42, intResult, testData);

    intResult = modExpr(5, 2);
    evalInt(1, intResult, testData);
    intResult = modExpr(-1, 3);
    evalInt(-1, intResult, testData);
    intResult = modExpr(4, 2);
    evalInt(0, intResult, testData);

    doubleResult = callIntDouble(42);
    evalDouble(42.0, doubleResult, testData);
    doubleResult = assignIntDoubleArray(17);
    evalDouble(17.0, doubleResult, testData);
    doubleResult = assignIntDoubleStruct(3);
    evalDouble(3.0, doubleResult, testData);

    print(testData.testCount);
    print(testData.successCount);

    destroy testData;
}