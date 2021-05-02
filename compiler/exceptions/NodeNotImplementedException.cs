namespace LL.Exceptions
{
    public class NodeNotImplementedException : BaseCompilerException
    {
        private static readonly string MESSAGE = "Node not implemented";
        public string NodeName { get; set; }

        public NodeNotImplementedException(string nodeName, string fileName, int line, int column)
        : base($"{MESSAGE}: {nodeName}", fileName, line, column)
            => this.NodeName = nodeName;
    }
}