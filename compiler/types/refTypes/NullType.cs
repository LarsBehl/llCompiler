namespace LL.Types
{
    public class NullType : RefType
    {
        public NullType() : base("NullType")
        {

        }

        public override bool Equals(object obj)
        {
            return obj is NullType;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(NullType nt, Type t)
        {
            return t is RefType;
        }

        public static bool operator !=(NullType nt, Type t)
        {
            return t is not RefType;
        }
    }
}