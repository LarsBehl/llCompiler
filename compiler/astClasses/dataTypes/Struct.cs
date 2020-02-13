using System.Collections.Generic;
using ll.type;

namespace ll.AST
{
    public class Struct : IAST
    {
        public string name { get; set; }
        public Dictionary<string, IAST> propValues { get; set; }

        public Struct(string name, Dictionary<string, IAST> propValues, int line, int column) : base(new StructType(name), line, column)
        {
            this.name = name;
            this.propValues = propValues;
        }

        public Struct(string name, int line, int column) : this(name, null, line, column)
        {

        }

        public Struct(int line, int column) : this(null, null, line, column)
        {

        }

        public override string ToString()
        {
            string result = this.name + " {\n";

            foreach (var prop in propValues)
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