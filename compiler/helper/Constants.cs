using System.Collections.Generic;

namespace LL.Helper
{
    public static class Constants
    {
        public static readonly string SOURCE_FILE_ENDING = "ll";
        public static readonly string HEADER_FILE_ENDING = "llh";
        public static readonly string INDENTATION = "    ";
        public static readonly string[] INTEGER_REGISTERS = { "%rdi", "%rsi", "%rdx", "%rcx", "%r8", "%r9" };
        public static readonly string[] DOUBLE_REGISTERS = { "%xmm0", "%xmm1", "%xmm2", "%xmm3", "%xmm4", "%xmm5", "%xmm6", "%xmm7" };
        public static readonly string INT_PRINT_STRING = "%ld\\n";
        public static readonly string BOOL_PRINT_STRING = INT_PRINT_STRING;
        public static readonly string DOUBLE_PRINT_STRING = "%f\\n";
        public static readonly string CHAR_PRINT_STRING = "%c\\n";
        public static readonly string STRING_PRINT_STRING = "%s\\n";
        public static readonly Dictionary<string, char> ESCAPED_CHARS = new Dictionary<string, char>()
        {
            {
                @"\n", '\n'
            },
            {
                @"\t", '\t'
            },
            {
                @"\""", '\"'
            },
            {
                @"\'", '\''
            },
            {
                @"\r", '\r'
            },
            {
                @"\0", '\0'
            },
            {
                @"\\", '\\'
            }
        };
    }
}