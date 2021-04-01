#include "classDataList.h"

ClassDataList *create_ClassDataList()
{
    ClassDataList *result = (ClassDataList *)malloc(sizeof(ClassDataList));

    if (!result)
        outOfMemory();

    result->values = (ClassData **)malloc(sizeof(ClassData *) * INITIAL_SIZE);

    if (!result->values)
        outOfMemory();

    result->size = INITIAL_SIZE;
    result->count = 0;

    return result;
}

void destroy_ClassDataList(ClassDataList *l)
{
    int i;
    for (i = 0; i < l->count; i++)
        destroy_ClassData(l->values[i]);

    free(l->values);
    free(l);
}

int add_ClassDataList(ClassData *val, ClassDataList *l)
{
    int index = getIndex_ClassDataList(val->id, l);

    if (index >= 0)
        return 0;

    if (l->count + 1 > l->size)
    {
        l->size += 5;
        ClassData **tmp = (ClassData **)malloc(sizeof(ClassData *) * l->size);

        int i;
        for (i = 0; i < l->count; i++)
            tmp[i] = l->values[i];

        free(l->values);
        l->values = tmp;
    }

    l->values[l->count++] = val;

    return 1;
}

ClassData *get_ClassDatList(int index, ClassDataList *list)
{
    if (index >= list->count || index < 0)
        outOfRange(index, list->count);

    return list->values[index];
}

ClassData *getById_ClassDataList(long id, ClassDataList *list)
{
    int i;
    for (i = 0; i < list->count; i++)
    {
        if (list->values[i]->id == id)
            return list->values[i];
    }

    return NULL;
}

void removeByIndex_ClassDataList(int index, ClassDataList *list)
{
    if (index >= list->count || index < 0)
        outOfRange(index, list->count);

    int i;
    for (i = index; i < list->count - 1; i++)
        list->values[i] = list->values[i + 1];

    list->values[--list->count] = NULL;
}

void removeById_ClassDataList(long id, ClassDataList *list)
{
    int index = getIndex_ClassDataList(id, list);

    if (index < 0)
        unknownObject(id);

    int i;
    for (i = index; i < list->count - 1; i++)
        list->values[i] = list->values[i + 1];

    list->values[--list->count] = NULL;
}

int getIndex_ClassDataList(long id, ClassDataList *list)
{
    int i;
    for (i = 0; i < list->count; i++)
    {
        if (list->values[i]->id == id)
            return i;
    }

    return -1;
}
