namespace ll.type
{
    public class IntType : PrimitivType
    {
        public IntType() : base("int")
        {
            
        }

        public override bool Equals(object o)
        {
            return o is IntType;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(IntType it, Type t)
        {
            return t is IntType;
        }

        public static bool operator !=(IntType it, Type t)
        {
            return t is not IntType;
        }
    }
}