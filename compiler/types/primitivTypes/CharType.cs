namespace LL.Types
{
    public class CharType : PrimitivType
    {
        public CharType() : base("char")
        {

        }

        public override bool Equals(object obj)
        {
            return obj is CharType;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(CharType ct, Type t) => t is CharType;

        public static bool operator !=(CharType ct, Type t) => t is not CharType;
    }
}