using System;

namespace ll
{
    public class IntLit : IAST
    {
        public int n { get; set; }

        public IntLit(int n)
        {
            this.n = n;
        }
    }
}