load sys_io;
load util;

writeStdout(message: char[], amount: int): int
{
    f: File = new File();
    f.fd = STDOUT;
    return writeFile(f, message, amount);
}

openTestIo(): File
{
    path: char[] = "./testGeneratedCode/programs/testIO.ll";
    file: File = openFile(path);
    destroy path;

    return file;
}

readTestIo(file: File, bytesToRead: int): int
{
    buffer: char[] = new char[bytesToRead + 1];
    bytesRead: int = readFile(file, buffer, bytesToRead);
    buffer[bytesToRead] = '\0';
    
    print(buffer);
    destroy buffer;

    return bytesRead;
}

lseekTestIo(file: File): bool
{
    result: int = lseekFile(file, 0, LSEEK_SET);
    
    return result == 0;
}

closeTestIo(file: File): void
{
    closeFile(file);
}