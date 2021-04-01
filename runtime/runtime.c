#include "runtime.h"
#include "errors.h"
#include "classDataList.h"

ClassDataList* classDataList;

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