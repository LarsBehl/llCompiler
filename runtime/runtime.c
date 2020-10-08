#include "runtime.h"

void *createHeapObject(long size)
{
    void *tmp = malloc(size);

    if (!tmp)
        outOfMemory();

    return tmp;
}

void destroyHeapObject(void *obj)
{
    free(obj);
}