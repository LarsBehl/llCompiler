namespace LL.Types
{
    public class CharArrayType : ArrayType
    {
        public CharArrayType() : base("char")
        {

        }

        public override bool Equals(object obj)
        {
            return obj is CharArrayType;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(CharArrayType charArrayType, Type t) => t is CharArrayType || t is NullType;
        public static bool operator !=(CharArrayType charArrayType, Type t) => t is not CharArrayType && t is not NullType;
    }
}