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

ll_fstatBuff* fstatWrapper(int fd)
{
    struct stat statbuf;
    int success = fstat(fd, &statbuf);

    if(success < 0)
    {
        printf("Retrieving information about file with descriptor %d failed. Shutting down...\n", fd);
        cleanUpRuntime();
        exit(-1);
    }

    ll_fstatBuff* result = (ll_fstatBuff*) malloc(sizeof(ll_fstatBuff));
    if (!result)
        outOfMemory();
    result->f_lastAcc = (ll_timespec*) malloc(sizeof(ll_timespec));
    if (!result->f_lastAcc)
        outOfMemory();
    result->f_lastMod = (ll_timespec*) malloc(sizeof(ll_timespec));
    if (!result->f_lastMod)
        outOfMemory();
    result->f_lastStatChange = (ll_timespec*) malloc(sizeof(ll_timespec));
    if (!result->f_lastStatChange)
        outOfMemory();
    
    result->f_dev = statbuf.st_dev;
    result->f_inode = statbuf.st_ino;
    result->f_mode = statbuf.st_mode;
    result->f_nlink = statbuf.st_nlink;
    result->f_uid = statbuf.st_uid;
    result->f_gid = statbuf.st_gid;
    result->f_rdev = statbuf.st_rdev;
    result->f_size = statbuf.st_size;
    result->f_blksize = statbuf.st_blksize;
    result->f_blocks = statbuf.st_blocks;
    result->f_lastAcc->tv_sec = statbuf.st_atim.tv_sec;
    result->f_lastAcc->tv_nsec = statbuf.st_atim.tv_nsec;
    result->f_lastMod->tv_sec = statbuf.st_mtim.tv_sec;
    result->f_lastMod->tv_nsec = statbuf.st_mtim.tv_nsec;
    result->f_lastStatChange->tv_sec = statbuf.st_ctim.tv_sec;
    result->f_lastStatChange->tv_nsec = statbuf.st_ctim.tv_nsec;

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