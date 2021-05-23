namespace LL.Helper
{
    public static class Constants
    {
        public static readonly string SOURCE_FILE_ENDING = "ll";
        public static readonly string HEADER_FILE_ENDING = "llh";
        public static readonly string INDENTATION = "    ";
        public static readonly string[] IntegerRegisters = { "%rdi", "%rsi", "%rdx", "%rcx", "%r8", "%r9" };
        public static readonly string[] DoubleRegisters = { "%xmm0", "%xmm1", "%xmm2", "%xmm3", "%xmm4", "%xmm5", "%xmm6", "%xmm7" };

    }
}