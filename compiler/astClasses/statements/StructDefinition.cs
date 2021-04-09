using System.Collections.Generic;
using ll.type;

namespace ll.AST
{
    public class StructDefinition : IAST
    {
        public List<StructProperty> properties { get; set; }
        public string name { get; set; }

        public StructDefinition(string name, List<StructProperty> properties, int line, int column) : base(new StructDefinitionType(), line, column)
        {
            this.name = name;
            this.properties = properties;
        }

        public StructDefinition(string name, int line, int column) : this(name, null, line, column)
        {

        }

        /// <summary>Returns the size of the struct in byte</summary>
        /// <return>Size of an instance of the struct in bytes</return>
        public int GetSize()
        {
            int result = 0;

            foreach (StructProperty structProperty in properties)
                result += 8;

            return result;
        }
    }
}