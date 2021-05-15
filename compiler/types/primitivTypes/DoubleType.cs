namespace LL.Types
{
    public class DoubleType : PrimitivType
    {
        public DoubleType() : base("double")
        {
            
        }

        public override bool Equals(object obj)
        {
            return obj is DoubleType;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(DoubleType dt, Type t)
        {
            return t is DoubleType;
        }

        public static bool operator !=(DoubleType dt, Type t)
        {
            return t is not DoubleType;
        }
    }
}