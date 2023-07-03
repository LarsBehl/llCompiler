#include "addrMap.h"
#include "constants.h"
#include "errors.h"

AddrMap* create_AddrMap()
{
    AddrMap* result = (AddrMap*) malloc(sizeof(AddrMap));
    result->count = 0;
    result->size = INITIAL_SIZE;
    result->values = (AddrList**) malloc(sizeof(AddrList*) * INITIAL_SIZE);

    return result;
}

void add_AddrMap(void* object, void* reference, AddrMap* map)
{
    int i;
    for(i = 0; i < map->count; i++)
    {
        if(map->values[i]->objectAddress == object)
        {
            add_AddrList(reference, map->values[i]);
            return;
        }
    }

    if(map->count + 1 > map->size)
    {
        map->size += INITIAL_SIZE;
        AddrList** newValues = (AddrList**) malloc(sizeof(AddrList*) * map->size);
        for(i = 0; i < map->count; i++)
        {
            newValues = map->values[i];
        }

        free(map->values);
        map->values = newValues;
    }

    AddrList* newList = create_AddrList();
    newList->objectAddress = object;
    add_AddrList(reference, newList);
    map->values[map->count++] = newList;
}

void removeByObject_AddrMap(void* object, AddrMap* map)
{
    int i;
    for(i = 0; i < map->count; i++)
    {
        if(map->values[i]->objectAddress == object)
        {
            destroy_AddrList(map->values[i]);
            break;
        }
    }

    if(i >= map->count)
        return;
    
    for(; i < map->count - 1; i++)
    {
        map->values[i] = map->values[i + 1];
    }

    map->count -= 1;
}

void removeByIndex_AddrMap(int index, AddrMap* map)
{
    if(index < 0 || index >= map->count)
        outOfRange(index, map->count);
    
    destroy_AddrList(map->values[index]);
    int i;
    for(i = index; i < map->count - 1; i++)
    {
        map->values[i] = map->values[i + 1];
    }

    map->count -1;
}

void removeReferenceByValue_AddrMap(void* object, void* reference, AddrMap* map)
{
    int i;
    for(i = 0; i < map->count; i++)
    {
        if(map->values[i]->objectAddress == object)
            break;
    }

    if(i == map->count)
        return;
    
    removeByValue_AddrList(reference, map->values[i]);
}

void removeReferenceByIndex_AddrMap(void* object, int index, AddrMap* map)
{
    int i;
    for(i = 0; i < map->count; i++)
    {
        if(map->values[i]->objectAddress == object)
            break;
    }

    if(i == map->count)
        return;
    
    removeByIndex_AddrList(index, map->values[i]);
}

int indexOfObject_AddrMap(void* object, AddrMap* map)
{
    int i;
    for(i = 0; i < map->count; i++)
    {
        if(map->values[i]->objectAddress == object)
            break;
    }

    if(i == map->count)
        i = -1;
    
    return i;
}

int indexOfReference_AddrMap(void* object, void* reference, AddrMap* map)
{
    int i = indexOfObject_AddrMap(object, map);
    if(i < 0)
        return i;
    
    i = indexOf_AddrList(reference, map->values[i]);

    return i;
}

void destroy_AddrMap(AddrMap* map)
{
    int i;
    for(i = 0; i < map->count; i++)
    {
        destroy_AddrList(map->values[i]);
    }

    free(map->values);
    free(map);
}
