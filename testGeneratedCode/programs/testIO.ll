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

readTestIo(fd: int, bytesToRead: int): int
{
    buffer: char[] = new char[bytesToRead + 1];
    bytesRead: int = readFile(fd, buffer, bytesToRead);
    buffer[bytesToRead] = '\0';
    
    print(buffer);
    destroy buffer;

    return bytesRead;
}

closeTestIo(fd: int): void
{
    closeFile(fd);
}