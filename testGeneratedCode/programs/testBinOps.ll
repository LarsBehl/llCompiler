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

equalCharChar(x:char, y:char): bool
{
    return x == y;
}

equalArrayArray(): bool
{
    x: int[] = new int[5];
    y: int[] = new int[10];
    result: bool = x == y;
    destroy x;
    destroy y;

    return result;
}

equalArrayNull(): bool
{
    x: int[] = new int[5];
    result: bool = x == null;
    destroy x;

    return result;
}

equalNullArrayNull(): bool
{
    x: int[] = null;

    return null == x;
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

notEqualCharChar(x:char, y:char): bool
{
    return x != y;
}

notEqualArrayArray(): bool
{
    x: int[] = new int[5];
    y: int[] = new int[10];
    result: bool = x != y;
    destroy x;
    destroy y;

    return result;
}

notEqualArrayNull(): bool
{
    x: int[] = new int[5];
    result: bool = x != null;
    destroy x;

    return result;
}

notEqualNullArrayNull(): bool
{
    x: int[] = null;

    return null != x;
}