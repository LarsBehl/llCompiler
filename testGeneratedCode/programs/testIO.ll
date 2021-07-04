load sys_io;

writeStdout(message: char[], amount: int): int
{
    return writeFile(STDOUT, message, amount);
}

openTestIo(): int
{
    path: char[] = "./testGeneratedCode/programs/testIO.ll";
    fd: int = openFile(path);
    destroy path;

    return fd;
}