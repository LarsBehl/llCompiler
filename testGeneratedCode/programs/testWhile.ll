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