#include "addrList.h"

typedef struct
{
    AddrList** values;
    int size;
    int count;
} AddrMap;

/**
 * Creates a AddrMap struct
*/
AddrMap* create_AddrMap();

/**
 * Adds an object and a reference to it to the map.
 * If the object is already present in the map, only the reference will be added
 * 
 * @param object    the object to add the reference for
 * @param reference the reference to add for the object
 * @param map       the map to add the values to
*/
void add_AddrMap(void* object, void* reference, AddrMap* map);

/**
 * Remove an object and all its references from the map
 * 
 * @param object    the object to remove from the map
 * @param map       the map to remove from
*/
void removeByObject_AddrMap(void* object, AddrMap* map);

/**
 * Remove an object and all its references by index from the map
 * 
 * @param index index of the object
 * @param map   map to remove from
*/
void removeByIndex_AddrMap(int index, AddrMap* map);

/**
 * Remove a single reference from the object
 * 
 * @param object    object to remove the reference from
 * @param reference the reference to remove
 * @param map       map to remove from
*/
void removeReferenceByValue_AddrMap(void* object, void* reference, AddrMap* map);

/**
 * Remove a single refrence by index from the object
 * 
 * @param object    object to remove the reference from
 * @param int       index of the reference to remove
 * @param map       map to remove from
*/
void removeReferenceByIndex_AddrMap(void* object, int index, AddrMap* map);

/**
 * Get the index of the given object
 * 
 * @param object    object to get the index for
 * @param map       map the get the index from
 * 
 * @returns the index of the object if present, -1 otherwise
*/
int indexOfObject_AddrMap(void* object, AddrMap* map);

/**
 * Get the index of the given reference for the given object
 * 
 * @param object    object to get the index of the reference from
 * @param reference reference to get the index for
 * @param map       map the get the index from
 * 
 * @returns the index of the reference for the object, -1 otherwise
*/
int indexOfReference_AddrMap(void* object, void* reference, AddrMap* map);

/**
 * Destructor function for AddrMap
 * 
 * @param map   struct to destroy
*/
void destroy_AddrMap(AddrMap* map);