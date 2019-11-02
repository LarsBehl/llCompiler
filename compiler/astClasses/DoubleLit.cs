using System;

namespace ll
{
    public class DoubleLit : IAST
    {
        public double n { get; set; }

        public DoubleLit(double n)
        {
            this.n = n;
        }
    }
}