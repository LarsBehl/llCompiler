using System.Collections.Generic;

namespace LL.CodeGeneration
{
    public class FunctionAsm
    {
        public string Name { get; set; }
        public Dictionary<string, int> VariableMap { get; set; }
        public int UsedDoubleRegisters { get; set; }
        public int UsedIntegerRegisters { get; set; }

        public FunctionAsm(string name)
        {
            this.Name = name;
            this.VariableMap = new Dictionary<string, int>();
            this.UsedDoubleRegisters = 0;
            this.UsedIntegerRegisters = 0;
        }
    }
}