using System.Collections.Generic;

namespace ll
{
    public class FunctionCall : IAST
    {
        public string name { get; set; }
        public List<IAST> args { get; set; }
    }
}