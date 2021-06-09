load testStructs;

global testData: TestData = new TestData();

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

testPrintChar(x:char): void
{
    print(x);
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

modExpr(x: int, y: int): int
{
    return x % y;
}