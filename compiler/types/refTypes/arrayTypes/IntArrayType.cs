namespace LL.Types
{
    public class IntArrayType : ArrayType
    {
        public IntArrayType() : base("IntArrayType")
        {

        }

        public override bool Equals(object obj)
        {
            return obj is IntArrayType;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(IntArrayType intArrayType, Type t)
        {
            return t is IntArrayType || t is NullType;
        }

        public static bool operator !=(IntArrayType intArrayType, Type t)
        {
            return t is not IntArrayType && t is not NullType;
        }
    }
}