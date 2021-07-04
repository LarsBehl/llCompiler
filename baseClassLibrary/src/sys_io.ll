load glibc;

global STDIN: int = 0;
global STDOUT: int = 1;
global STDERR: int = 2;

writeFile(fd: int, buffer: char[], amount: int): int
{
    return write(fd, buffer, amount);
}