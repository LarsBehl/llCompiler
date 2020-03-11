id(x: int): int
{
    return x;
}

square(x: int): int
{
    return x * x;
}

aCallsB(x: int): int
{
    return b(x);
}

b(x: int): int
{
    return x + 2;
}

half(x: double): double
{
    return x / 2;
}

fourtyTwo(): int
{
    y: int = square(5);
    return y + 17;
}

plusSeventeen(x: int): int
{
    return x + 17;
}

fac(x: int): int
{
    if(x > 1)
    {
        return x * fac(x-1);
    }

    return x;
}

facIter(x: int): int
{
    x2: int = x;

    while(x > 1)
    {
        x2 = x2 * --x;
    }

    return x2;
}

voidFunction(): void
{
    x: int = 1;

    return;
}

equalsArray(): int
{
    x: int[] = new int[5];
    y: int[] = x;
    if(x == y)
    {
        return 42;
    }

    return 17;
}

equalsArrayNull(): int
{
    x: int[] = new int[5];

    if(null == x)
    {
        return 17;
    }

    return 42;
}

equalsNullArrayNull(): int
{
    x: int[] = null;

    if(x == null)
    {
        return 42;
    }

    return 17;
}

notEqualsArray(): int
{
    x: int[] = new int[5];
    y: int[] = new int[10];

    if(x != y)
    {
        return 42;
    }

    return 17;
}

notEqualsArrayNull(): int
{
    x: int[] = new int[42];

    if(x != null)
    {
        return 42;
    }

    return 17;
}

notEqualsNullArrayNull(): int
{
    x: int[] = null;

    if(null != x)
    {
        return 17;
    }

    return 42;
}