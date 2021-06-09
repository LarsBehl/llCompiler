struct Point
{
    x: int;
    y: int;
}

struct Line 
{
    p1: Point;
    p2: Point;
}

struct iA
{
    array: int[];
}

struct doubleStruct
{
    d: double;
}

struct TestData
{
    testCount: int;
    successCount: int;
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

assignIntDoubleStruct(x: int): double
{
    y: doubleStruct = new doubleStruct();
    y.d = x;
    result: double = y.d;
    destroy y;

    return result;
}