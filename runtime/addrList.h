typedef struct
{
    void* objectAddress;
    void** references;
    int size;
    int count;
} AddrList;

/**
 * Creates a AddrList struct
 * 
 * @returns pointer to AddrList
*/
AddrList* create_AddrList();

/**
 * Adds the given reference to the address list.
 * 
 * @param reference     the reference to add to the reference list
 * @param list          the list to add the value to
*/
void add_AddrList(void* reference, AddrList* list);

/**
 * Retrieve the value at the given index
 * 
 * @param index index of the value to retrieve
 * @param list  list to retrieve the value from
 * 
 * @returns     the value at the given index
*/
void* getByIndex_AddrList(int index, AddrList* list);

/**
 * Remove the value from the given list
 * 
 * @param reference value to remove from the reference list
 * @param list      list to remove the value from
*/
void removeByValue_AddrList(void* reference, AddrList* list);

/**
 * Remove the value at the given index from the list
 * 
 * @param index index of the value to remove
 * @param list  list to remove the value from
*/
void removeByIndex_AddrList(int index, AddrList* list);

/**
 * Retrieve the index of the given reference
 * 
 * @param reference value to retrieve the index for
 * @param list      listg to retrieve the index from
 * 
 * @returns the index if found, -1 otherwise
*/
int indexOf_AddrList(void* reference, AddrList* list);

/**
 * Destructor function for AddrList
 * 
 * @param list  struct to destroy
*/
void destroy_AddrList(AddrList* list);
