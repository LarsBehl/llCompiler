namespace LL.Types
{
    public abstract class PrimitivType : LL.Types.Type
    {
        public PrimitivType(string typeName) : base(typeName)
        {

        }

        public override bool IsPrimitivType()
        {
            return true;
        }
    }
}