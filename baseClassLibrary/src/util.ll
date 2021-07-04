load glibc;
load runtime;

global int32MaxValue: int = 4294967295;

exitProgram(status: int): void
{
    cleanUpRuntime();
    exit(status);
}