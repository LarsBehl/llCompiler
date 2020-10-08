#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include "errors.h"

typedef struct
{
    long id;
    long size;
} ClassData;

/**
 * Creates a ClassData struct
 * 
 * @param id    id of the class, calculated on compilation
 * @param size  amount of bytes needed for given object
 * @returns     pointer to ClassData
 */
ClassData *create_ClassData(long id, long size);

/**
 * Destructor function for ClassData
 * 
 * @param data  struct to destroy
 */
void destroy_ClassData(ClassData *data);