#include "errors.h"

void outOfMemory()
{
    printf("Out of memory; Shutting down..\n");
    // TODO clean up runtime
    exit(EXIT_FAILURE);
}

void outOfRange(int index, int range)
{
    printf("Index %d out of range %d; Shutting down...\n", index, range);
    // TODO clean up runtime
    exit(EXIT_FAILURE);
}

void unknownObject(int id)
{
    printf("Unknown Class ID \"%d\"; Shutting down...\n", id);
    // TODO clean up runtime
    exit(EXIT_FAILURE);
}