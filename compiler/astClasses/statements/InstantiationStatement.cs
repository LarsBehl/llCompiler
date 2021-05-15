namespace LL.AST
{
    public class InstantiationStatement : IAST
    {
        public string Name { get; set; }

        public InstantiationStatement(string name, Types.Type type, int line, int column) : base(type, line, column)
        {
            this.Name = name;
        }
    }
}