intId(x:int): int
{
    return x;
}

doubleId(x:double): double
{
    return x;
}

boolId(x:bool): bool
{
    return x;
}

addIntInt(x:int, y:int): int
{
    return x+y;
}

addIntDouble(x:int, y:double): double
{
    return x+y;
}

addDoubleInt(x:double, y:int): double
{
    return x+y;
}

addDoubleDouble(x:double, y:double): double
{
    return x+y;
}

subIntInt(x:int, y:int): int
{
    return x-y;
}

subIntDouble(x:int, y:double): double
{
    return x-y;
}

subDoubleInt(x:double, y:int): double
{
    return x - y;
}

subDoubleDouble(x:double, y:double): double
{
    return x-y;
}

multIntInt(x:int, y:int): int
{
    return x*y;
}

multIntDouble(x:int, y:double): double
{
    return x*y;
}

multDoubleInt(x:double, y:int): double
{
    return x*y;
}

multDoubleDouble(x:double, y:double): double
{
    return x*y;
}

divIntInt(x:int, y:int): int
{
    return x/y;
}

divIntDouble(x:int, y:double): double
{
    return x/y;
}

divDoubleInt(x:double, y:int): double
{
    return x/y;
}

divDoubleDouble(x:double, y:double): double
{
    return x/y;
}

greaterIntInt(x:int, y:int): bool
{
    return x > y;
}

greaterIntDouble(x:int, y:double): bool
{
    return x > y;
}

greaterDoubleInt(x:double, y:int): bool
{
    return x > y;
}

greaterDoubleDouble(x:double, y:double): bool
{
    return x > y;
}

lessIntInt(x:int, y:int): bool
{
    return x < y;
}

lessIntDouble(x:int, y:double): bool
{
    return x < y;
}

lessDoubleInt(x:double, y:int): bool
{
    return x < y;
}

lessDoubleDouble(x:double, y:double): bool
{
    return x < y;
}

equalIntInt(x:int, y:int): bool
{
    return x == y;
}

equalIntDouble(x:int, y:double): bool
{
    return x == y;
}

equalDoubleInt(x:double, y:int): bool
{
    return x == y;
}

equalDoubleDouble(x:double, y:double): bool
{
    return x == y;
}

equalArrayArray(): bool
{
    x: int[] = new int[5];
    y: int[] = new int[10];

    return x == y;
}

equalArrayNull(): bool
{
    x: int[] = new int[5];

    return x == null;
}

equalNullArrayNull(): bool
{
    x: int[] = null;

    return null == x;
}

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