namespace LL.Types
{
    public class VoidType : PrimitivType
    {
        public VoidType(): base("VoidType")
        {
            
        }

        public override bool Equals(object obj)
        {
            return obj is VoidType;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(VoidType vt, Type t)
        {
            return t is VoidType;
        }

        public static bool operator !=(VoidType vt, Type t)
        {
            return t is not VoidType;
        }
    }
}