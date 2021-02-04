namespace ll.type
{
    public class BoolArrayType : ArrayType
    {
        public BoolArrayType() : base("boolArrayType")
        {

        }

        public override bool Equals(object obj)
        {
            return obj is BoolArrayType;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(BoolArrayType boolArrayType, Type t)
        {
            return t is BoolArrayType || t is NullType;
        }

        public static bool operator !=(BoolArrayType boolArrayType, Type t)
        {
            return t is not BoolArrayType && t is not NullType;
        }
    }
}