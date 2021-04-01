#include "errors.h"
#include "runtime.h"

void outOfMemory()
{
    printf("Out of memory; Shutting down..\n");
    cleanUpRuntime();

    exit(EXIT_FAILURE);
}

void outOfRange(int index, int range)
{
    printf("Index %d out of range %d; Shutting down...\n", index, range);
    cleanUpRuntime();

    exit(EXIT_FAILURE);
}

void unknownObject(int id)
{
    printf("Unknown Class ID \"%d\"; Shutting down...\n", id);
    cleanUpRuntime();

    exit(EXIT_FAILURE);
}