using System.Collections.Generic;

namespace ll
{
    public class Sequenz : IAST
    {
        public List<IAST> body { get; set; }

        public Sequenz(List<IAST> body)
        {
            this.body = body;
        }
    }
}