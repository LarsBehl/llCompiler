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