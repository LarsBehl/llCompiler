#include "classData.h"
#include <stdlib.h>
#include <stdio.h>
#include "constants.h"

typedef struct
{
    ClassData **values;
    int count;
    int size;
} ClassDataList;

/**
 * Creates a ClassDataList
 * 
 * @returns pointer to the created ClassDataList
 */
ClassDataList *create_ClassDataList();

/**
 * Destroys a ClassDataList
 * 
 * @param l list that should get destroyed
 */
void destroy_ClassDataList(ClassDataList *l);

/**
 * Adds a value to the list
 * 
 * @param val   value to add
 * @param l  list to add the value to
 * 
 * @returns     true if list does not contain the value, false otherwise
 */
int add_ClassDataList(ClassData *val, ClassDataList *l);

/**
 * Returns the value at the specified index
 * 
 * @param index index of the value
 * @param list  list to retrieve the value from
 * 
 * @returns     value at the specified index, NULL if index not in range
 */
ClassData *get_ClassDataList(int index, ClassDataList *list);

/**
 * Returns the value with the specified id
 * 
 * @param id    id of the ClassData Object
 * @param list  list to retrieve the value from
 * 
 * @returns     value of the specified ClassData Object, NULL if id is unknown
 */
ClassData *getById_ClassDataList(long id, ClassDataList *list);

/**
 * Removes an element by index
 * 
 * @param index index of the element to remove
 * @param list  list to remove from
 */
void removeByIndex_ClassDataList(int index, ClassDataList *list);

/**
 * Removes an element by id
 * 
 * @param id    id of the ClassData Object to remove
 * @param list  list to remove from
 */
void removeById_ClassDataList(long id, ClassDataList *list);

/**
 * Cheks if list contains ClassData object with specified id
 * 
 * @param id    id of ClassData Object
 * @param list  list to search in
 * 
 * @returns     index of id, -1 otherwise
 */
int getIndex_ClassDataList(long id, ClassDataList *list);