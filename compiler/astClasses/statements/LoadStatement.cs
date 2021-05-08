using LL.Types;

namespace LL.AST
{
    public class LoadStatement : IAST
    {
        public string FileName { get; set; }
        public string Location { get; set; }
        public ProgramNode Program { get; set; }
        
        public LoadStatement(string fileName, string location, int line, int column) : base(new LoadStatementType(), line, column)
        {
            this.FileName = fileName;
            this.Location = location;
        }
    }
}