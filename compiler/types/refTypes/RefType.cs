namespace ll.type
{
    public class RefType : Type
    {
        public RefType(string type) : base(type)
        {

        }

        public override bool IsPrimitivType()
        {
            return false;
        }
    }
}