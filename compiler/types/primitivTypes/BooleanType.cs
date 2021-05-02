namespace LL.Types
{
    public class BooleanType : PrimitivType
    {
        public BooleanType() : base("bool")
        {
            
        }

        public override bool Equals(object obj)
        {
            return obj is BooleanType;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(BooleanType bt, Type t)
        {
            return t is BooleanType;
        }

        public static bool operator !=(BooleanType bt, Type t)
        {
            return t is not BooleanType;
        }
    }
}