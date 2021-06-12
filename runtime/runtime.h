#include <stdlib.h>
#include <stdio.h>
#include <stdbool.h>

/**
 * Creates a new heap object
 * 
 * @param size      size of the object in byte, or id of the struct
 * @param isArray   true if array should be created
 * @returns         base address of the object
 */
void *createHeapObject(long size, bool isArray);

/**
 * Creates a char[] from the given constant char[]
 *
 * @param literal   literal to create the heap object for
 * @param length    length of the string
 * @returns         base address of the char[]
 */
char* createStringFromLiteral(char* literal, long length);

/**
 * Frees up the memory used by the object
 * 
 * @param obj    address of the object
 */
void destroyHeapObject(void *obj);

/**
 * Registers a class and its associated size in bytes
 * 
 * @param id    id of the class to register
 * @param size  size of an instance of the class
 */
void registerClass(long id, long size);

/**
 * Initializes all needed data structures needed by the runtime
 */
void initializeRuntime();

/**
 * Frees up all memory still allocated for the runtime
 */
void cleanUpRuntime();