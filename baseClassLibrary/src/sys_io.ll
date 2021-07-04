load glibc;
load util;

global STDIN: int = 0;
global STDOUT: int = 1;
global STDERR: int = 2;

writeFile(fd: int, buffer: char[], amount: int): int
{
    return write(fd, buffer, amount);
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