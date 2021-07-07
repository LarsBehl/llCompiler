load glibc;
load util;
load runtime;

global STDIN: int = 0;
global STDOUT: int = 1;
global STDERR: int = 2;

struct File
{
    fd: int;
    stats: FileStat;
}

writeFile(fd: int, buffer: char[], amount: int): int
{
    return write(fd, buffer, amount);
}

readFile(fd: int, buffer: char[], ammount: int): int
{
    bytesRead: int = read(fd, buffer, ammount);
    if(bytesRead < 0)
    {
        print("Could not read from file with descriptor:");
        print(fd);
        exitProgram(-1);
    }
    return bytesRead;
}

openFile(path: char[]): int
{
    fd: int = open(path, 0);

    if(fd == int32MaxValue)
    {
        print("Could not open file:");
        print(path);
        exitProgram(-1);
    }

    return fd;
}

closeFile(fd: int): void
{
    result: int = close(fd);

    if(result == int32MaxValue)
    {
        print("Could not close with with descriptor:");
        print(fd);
        exitProgram(-1);
    }
}

fileStats(fd: int): FileStat
{
    return fstatWrapper(fd);
}