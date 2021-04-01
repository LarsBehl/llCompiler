#include <stdio.h>
#include <stdlib.h>

/**
 * System is out of memory. Clean up application and shutdown with error
 */
void outOfMemory();

/**
 * Given index is out of list range. Clean up application and shutdown with error
 */
void outOfRange(int index, int range);

/**
 * No class with the given ID exists
 */
void unknownObject(int id);