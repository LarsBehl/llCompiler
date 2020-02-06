using System.Collections.Generic;
using ll.type;

namespace ll.AST
{
    public class StructDefinition : IAST
    {
        public List<StructProperty> properties { get; set; }
        public string name { get; set; }

        public StructDefinition(string name, List<StructProperty> properties) : base(new StructDefinitionType())
        {
            this.name = name;
            this.properties = properties;
        }

        public StructDefinition(string name) : this(name, null)
        {

        }
    }
}