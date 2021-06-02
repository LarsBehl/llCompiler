using LL.Types;
using LL.Exceptions;

namespace LL.AST
{
    public class GlobalVariableStatement : IAST
    {
        private IAST val;
        private string currentFile;
        public VarExpr Variable { get; set; }
        public IAST Value
        {
            get => this.val;
            set
            {
                if (value.Type != Variable.Type)
                    throw new TypeMissmatchException
                    (
                        Variable.Type.ToString(),
                        value.Type.ToString(),
                        this.currentFile,
                        Variable.Line,
                        Variable.Column
                    );

                this.val = value;
            }
        }

        public GlobalVariableStatement(VarExpr variable, string currentFile, int line, int column) : base(new GlobalVariableStatementType(), line, column)
        {
            this.Variable = variable;
            this.Value = null;
            this.currentFile = currentFile;
        }
    }
}