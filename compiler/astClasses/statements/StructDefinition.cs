using System.Collections.Generic;
using LL.Types;

namespace LL.AST
{
    public class StructDefinition : IAST
    {
        public List<StructProperty> Properties { get; set; }
        public string Name { get; set; }

        public StructDefinition(string name, List<StructProperty> properties, int line, int column) : base(new StructDefinitionType(), line, column)
        {
            this.Name = name;
            this.Properties = properties;
        }

        public StructDefinition(string name, int line, int column) : this(name, null, line, column)
        {

        }

        /// <summary>Returns the size of the struct in byte</summary>
        /// <return>Size of an instance of the struct in bytes</return>
        public int GetSize()
        {
            int result = 0;

            foreach (StructProperty structProperty in Properties)
                result += 8;

            return result;
        }
    }
}