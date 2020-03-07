struct ArrayList
{
    length: int;
    size: int;
    content: int[];
}

newArrayList(): ArrayList
{
    result: ArrayList = new ArrayList();
    result.length = 0;
    result.size = 5;
    result.content = new int[5];

    return result;
}

get(index: int, al: ArrayList): int
{
    if(index >= al.length)
    {
        return -42;
    }

    return al.content[index];
}

add(item: int, al: ArrayList): void
{
    print(item);
    if(al.length + 1 > al.size)
    {
        al.size = al.length + 5;
        tmp: int[] = new int[al.size];
        i: int = 0;

        while(i < al.length)
        {
            tmp[i] = al.content[i];
            i+=1;
        }
        destroy al.content;

        al.content = tmp;
    }

    al.content[al.length++] = item;
}

remove(index: int, al: ArrayList): bool
{
    if(index >= al.length)
    {
        return false;
    }
    else
    {
        while(index + 1 < al.length)
        {
            al.content[index] = al.content[index+1];
            index += 1;
        }

        al.length--;
        al.content[al.length] = 0;

        return true;
    }
}

indexOf(item: int, al: ArrayList): int
{
    i: int = 0;

    while(i < al.length)
    {
        if(get(i, al) == item)
        {
            return i;
        }

        i++;
    }

    return -1;
}

insert(item: int, index: int, al: ArrayList): bool
{
    if(index >= al.length)
    {
        return false;
    }

    add(item, al);

    i: int = al.length - 1;

    while(i > index)
    {
        al.content[i] = al.content[--i];
    }

    al.content[index] = item;

    return true;
}

update(index: int, item: int, al:ArrayList): bool
{
    if(index >= al.length)
    {
        return false;
    }

    al.content[index] = item;
    return true;
}

main(): void
{
    al: ArrayList = newArrayList();

    i: int = 0;

    while(i < 20)
    {
        add(i, al);
        i += 1;
    }

    
}