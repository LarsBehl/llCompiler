load glibc;
load runtime;

exitProgram(status: int): void
{
    cleanUpRuntime();
    exit(status);
}