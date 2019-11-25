f(x: int): int
{
    return g(x);
}

g(x: int): int
{
    return x + 2;
}

f(3)