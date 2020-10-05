#include <stdlib.h>
#include <stdio.h>

/**
 * Creates a new heap object
 * 
 * @param size  size of the object in byte
 * @returns     base address of the object
 */
void *createHeapObject(long size);

/**
 * Frees up the memory used by the object
 * 
 * @param obj    address of the object
 */
void destroyHeapObject(void *obj);