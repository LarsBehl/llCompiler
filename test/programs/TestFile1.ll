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