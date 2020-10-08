#include "addrList.h"

AddrList *create_AddrList()
{
    AddrList *result = (AddrList *)malloc(sizeof(AddrList));

    if (!result)
        outOfMemory();

    result->size = INITIAL_SIZE;
    result->values = (void **)malloc((sizeof(void *) * result->size));

    if (!result->values)
        outOfMemory();

    result->values = (void **)memset(result->values, 0, result->size);
    result->count = 0;

    return result;
}

void destory_AddrList(AddrList *l)
{
    free(l->values);
    free(l);
}

int add_AddrList(void *val, AddrList *list)
{
    if (getIndex_AddrList(val, list) < 0)
        return 0;

    if (list->count + 1 > list->size)
    {
        list->size += INITIAL_SIZE;
        void **tmp = (void **)malloc(sizeof(void *) * list->size);

        if (!tmp)
            outOfMemory();

        for (int i = 0; i < list->count; i++)
        {
            tmp[i] = list->values[i];
        }
    }

    list->values[list->count++] = val;

    return 1;
}

void *get_AddrList(int index, AddrList *list)
{
    if (index >= list->count)
        return NULL;

    return list->values[index];
}

int removeByIndex_AddrList(int index, AddrList *list)
{
    if (index >= list->count)
        return 0;

    if (index < list->count - 1)
    {
        for (int i = index; i < list->count - 1; i++)
        {
            list->values[i] = list->values[i + 1];
        }
    }

    list->values[list->count--] = NULL;
    return 1;
}

int removeByValue_AddrList(void *val, AddrList *list)
{
    int index = getIndex_AddrList(val, list);

    if (index < 0)
        return 0;

    return removeByIndex_AddrList(index, list);
}

int getIndex_AddrList(void *val, AddrList *list)
{
    for (int i = 0; i < list->count; i++)
    {
        if (list->values[i] == val)
            return i;
    }

    return -1;
}