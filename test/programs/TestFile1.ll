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
    result: int = x;

    while(x > 1)
    {
        result = result * --x;
    }

    return result;
}

test(x: int): int
{
    if(x > 2)
    {
        if(x==3)
        {
            return x * plusSeventeen(x);
        }

        return 1337;
    }
    else
    {
        return 42;
    }
}

testNeu(): int
{
    i: int = 0;

    while(i < 100)
    {
        if(i == 42)
        {
            return i;
        }

        i++;
    }

    return 1337;
}

testNeuNeu(): int
{
    while(true)
    {
        return 1337;
    }
}