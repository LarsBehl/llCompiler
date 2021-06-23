using System.Text;
using System.Collections.Generic;

using LL.AST;

namespace LL.Exceptions
{
    public class CircularDependencyException : BaseCompilerException
    {
        private static readonly string MESSAGE = "Detected circular dependency";

        public List<ProgramNode> CircularDependencies { get; set; }


        public CircularDependencyException(List<ProgramNode> nodes, string fileLocation) : base($"{MESSAGE}: {FormatList(nodes)}", fileLocation, -1, -1)
        {
            this.CircularDependencies = nodes;
        }

        private static string FormatList(List<ProgramNode> nodes)
        {
            StringBuilder bob = new StringBuilder();

            for(int i = 0; i < nodes.Count; i++)
            {
                bob.Append(nodes[i].FileName);

                if(i < nodes.Count - 1)
                    bob.Append("->");
            }

            return bob.ToString();
        }
    }
}