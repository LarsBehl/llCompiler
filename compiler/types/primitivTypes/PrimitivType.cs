namespace ll.type
{
    public class PrimitivType : ll.type.Type
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