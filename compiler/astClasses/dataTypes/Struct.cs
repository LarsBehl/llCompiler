using System.Collections.Generic;
using ll.type;

namespace ll.AST
{
    public class Struct : IAST
    {
        public string name { get; set; }
        public List<StructProperty> propValues { get; set; }

        public Struct(string name, List<StructProperty> propValues) : base(new StructType(name))
        {
            this.name = name;
            this.propValues = propValues;
        }

        public Struct(string name) : this(name, null)
        {

        }

        public Struct() : this(null, null)
        {

        }
    }
}