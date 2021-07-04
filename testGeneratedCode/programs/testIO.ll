load sys_io;

writeStdout(message: char[], amount: int): int
{
    return writeFile(STDOUT, message, amount);
}