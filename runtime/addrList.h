#define INITIAL_SIZE 5

typedef struct
{
    void **values;
    int count;
    int size;
} AddrList;

/**
 * Creates an AddrList
 * 
 * @returns pointer to the created AddrList
 */
AddrList *create_AddrList();

/**
 * Destorys an AddrList
 * 
 * @param l list that should get destroyed
 */
void destroy_AddrList(AddrList *l);

/**
 * Adds a value to the list
 * 
 * @param val   value to add
 * @param list  list to add the value to
 * 
 * @returns     true if list does not contain the value, false otherwise
 */
int add_AddrList(void *val, AddrList *list);

/**
 * Returns the value at the specified index
 * 
 * @param index index to of the value
 * @param list  list to retrieve the value from
 * 
 * @returns     value at the specified index, NULL if index not in range
 */
void *get_AddrList(int index, AddrList *list);

/**
 * Removes an element by index
 * 
 * @param index index of the element to remove
 * @param list  list to remove from
 * 
 * @returns     true if index in range, false otherwise
 */
int removeByIndex_AddrList(int index, AddrList *list);

/**
 * Removes an element by value
 * 
 * @param val   value to remove
 * @param list  list to remove from
 * 
 * @returns     true if list contains value, false otherwise
 */
int removeByValue_AddrList(void *val, AddrList *list);

/**
 * Checks if list contains val
 * 
 * @param val   value to look for
 * @param list  list to search in
 * 
 * @returns     index of val, -1 otherwise
 */
int getIndex_AddrList(void *val, AddrList *list);