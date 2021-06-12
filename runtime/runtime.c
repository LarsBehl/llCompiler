#include "runtime.h"
#include "errors.h"
#include "classDataList.h"

ClassDataList* classDataList;

void *createHeapObject(long size, bool isArray)
{
    if(!isArray)
    {
        size = getById_ClassDataList(size, classDataList)->size;
    }
    
    void *tmp = malloc(size);

    if (!tmp)
        outOfMemory();

    return tmp;
}

char* createStringFromLiteral(char* literal, long length)
{
    char* result = createHeapObject(length + 1, true);

    int i;
    for(i = 0; i < length; i++)
    {
        result[i] = literal[i];
    }

    result[length] = '\0';

    return result;
}

void destroyHeapObject(void *obj)
{
    free(obj);
}

void registerClass(long id, long size)
{
    ClassData* classData = create_ClassData(id, size);
    add_ClassDataList(classData, classDataList);
}

void initializeRuntime()
{
    classDataList = create_ClassDataList();
}

void cleanUpRuntime()
{
    destroy_ClassDataList(classDataList);
}