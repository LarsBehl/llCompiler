#include "runtime.h"

void *createHeapObject(long size)
{
    void *tmp = malloc(size);

    if (!tmp)
    {
        printf("Out of memory; Shutting down...\n");
        // TODO clean up runtime
        exit(EXIT_FAILURE);
    }

    return tmp;
}

void destroyHeapObject(void *obj)
{
    free(obj);
}