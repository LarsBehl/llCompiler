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

        public static bool operator ==(LoadStatement l1, LoadStatement l2)
        {
            return l1.Program == l2.Program;
        }

        public static bool operator !=(LoadStatement l1, LoadStatement l2)
        {
            return l1.Program != l2.Program;
        }

        public override bool Equals(object obj)
        {
            if(obj is LoadStatement load)
            {
                return load == this;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}