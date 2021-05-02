using System.Collections.Generic;
using LL.Types;

namespace LL.AST
{
    public class Struct : IAST
    {
        public string Name { get; set; }
        public Dictionary<string, IAST> PropValues { get; set; }

        public Struct(string name, Dictionary<string, IAST> propValues, int line, int column) : base(new StructType(name), line, column)
        {
            this.Name = name;
            this.PropValues = propValues;
        }

        public Struct(string name, int line, int column) : this(name, null, line, column)
        {

        }

        public Struct(int line, int column) : this(null, null, line, column)
        {

        }

        public override string ToString()
        {
            string result = this.Name + " {\n";

            foreach (var prop in PropValues)
            {
                result += $"\t{prop.Key}:";

                if (prop.Value == null)
                    result += "null";
                else
                    result += prop.Value.ToString();

                result += ";\n";
            }

            return result + "}";
        }
    }
}