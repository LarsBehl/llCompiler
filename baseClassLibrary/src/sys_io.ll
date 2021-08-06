load glibc;
load util;
load runtime;

global STDIN: int = 0;
global STDOUT: int = 1;
global STDERR: int = 2;
global LSEEK_SET: int = 0;
global LSEEK_CUR: int = 1;
global LSEEK_END: int = 2;

struct File
{
    fd: int;
    stats: FileStat;
}

writeFile(file: File, buffer: char[], amount: int): int
{
    return write(file.fd, buffer, amount);
}

readFile(file: File, buffer: char[], ammount: int): int
{
    bytesRead: int = read(file.fd, buffer, ammount);
    if(bytesRead < 0)
    {
        print("Could not read from file with descriptor:");
        print(file.fd);
        exitProgram(-1);
    }
    return bytesRead;
}

openFile(path: char[]): File
{
    result: File = new File();
    result.fd = open(path, 0);

    if(result.fd == int32MaxValue)
    {
        print("Could not open file:");
        print(path);
        exitProgram(-1);
    }

    result.stats = fileStats(result.fd);

    return result;
}

closeFile(file: File): void
{
    result: int = close(file.fd);

    if(result == int32MaxValue)
    {
        print("Could not close with with descriptor:");
        print(file.fd);
        exitProgram(-1);
    }

    destroyFile(file);
}

destroyFile(file: File): void
{
    destroy file.stats.lastAcc;
    destroy file.stats.lastMod;
    destroy file.stats.lastStatChange;
    destroy file.stats;
    destroy file;
}

fileStats(fd: int): FileStat
{
    return fstatWrapper(fd);
}

lseekFile(file: File, offset: int, whence: int): int
{
    result: int = lseek(file.fd, offset, whence);

    if(result < 0)
    {
        print("Could not seek in file with fd:");
        print(file.fd);
        exitProgram(-1);
    }

    return result;
}