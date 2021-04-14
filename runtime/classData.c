#include "classData.h"

ClassData *create_ClassData(long id, long size)
{
    ClassData *result = (ClassData *)malloc(sizeof(ClassData));

    if (!result)
        outOfMemory();

    result->id = id;
    result->size = size;

    return result;
}

void destroy_ClassData(ClassData *data)
{
    free(data);
}