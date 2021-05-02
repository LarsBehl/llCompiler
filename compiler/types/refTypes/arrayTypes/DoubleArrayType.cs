namespace LL.Types
{
    public class DoubleArrayType : ArrayType
    {
        public DoubleArrayType() : base("doubleArrayType")
        {

        }

        public override bool Equals(object obj)
        {
            return obj is DoubleArrayType;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(DoubleArrayType doubleArrayType, Type t)
        {
            return t is DoubleArrayType || t is NullType;
        }

        public static bool operator !=(DoubleArrayType doubleArrayType, Type t)
        {
            return t is not DoubleArrayType && t is not NullType;
        }
    }
}