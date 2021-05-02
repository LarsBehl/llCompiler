using System.Collections.Generic;

namespace LL.CodeGeneration
{
    public class FunctionAsm
    {
        public string name { get; set; }
        public Dictionary<string, int> variableMap { get; set; }
        public int usedDoubleRegisters { get; set; }
        public int usedIntegerRegisters { get; set; }

        public FunctionAsm(string name)
        {
            this.name = name;
            this.variableMap = new Dictionary<string, int>();
            this.usedDoubleRegisters = 0;
            this.usedIntegerRegisters = 0;
        }
    }
}