#include <stdlib.h>
#include "addrList.h"
#include "constants.h"
#include "errors.h"

AddrList* create_AddrList()
{
    AddrList* result = (AddrList*) malloc(sizeof(AddrList));
    result->count = 0;
    result->size = INITIAL_SIZE;
    result->references = (void**) malloc(sizeof(void*) * INITIAL_SIZE);
    result->objectAddress = NULL;

    return result;
}

void add_AddrList(void* reference, AddrList* list)
{
    if(list->count + 1 > list->size)
    {
        list->size += INITIAL_SIZE;
        void** newList = (void**) malloc(sizeof(void*) * list->size);

        int i;
        for(i = 0; i < list->count; i++)
        {
            newList[i] = list->references[i];
        }

        free(list->references);
        list->references = newList;
    }

    list->references[list->count++] = reference;
}

void* getByIndex_AddrList(int index, AddrList* list)
{
    if(index > list->count || index < 0)
        outOfRange(index, list->count);
    
    return list->references[index];
}

void removeByValue_AddrList(void* reference, AddrList* list)
{
    int i;
    for(i = 0; i < list->count; i++)
    {
        if(list->references[i] == reference)
            break;
    }

    if(i >= list->count)
        return;

    for(; i < list->count - 1; i++)
    {
        list->references[i] = list->references[i + 1];
    }

    list->count -= 1;
}

void removeByIndex_AddrList(int index, AddrList* list)
{
    if(index < 0 || index > list->count)
        outOfRange(index, list->count);
    
    int i;
    for(i = index; i < list->count - 1; i++)
    {
        list->references[i] = list->references[i + 1];
    }

    list->count -= 1;
}

int indexOf_AddrList(void* reference, AddrList* list)
{
    int i;

    for(i = 0; i < list->count; i++)
    {
        if(list->references[i] == reference)
            break;
    }

    if(i == list->count)
        i = -1;

    return i;
}

void destroy_AddrList(AddrList* list)
{
    free(list->references);
    free(list);
}
