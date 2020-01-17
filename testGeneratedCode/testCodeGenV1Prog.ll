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

equalDoubleDouble(x:double, y:double): bool
{
    return x == y;
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