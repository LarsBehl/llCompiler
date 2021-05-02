namespace LL.Types
{
    public class StructType : RefType
    {
        public string structName { get; set; }
        public StructType(string structName) : base("StructType")
        {
            this.structName = structName;
        }

        public override bool Equals(object obj)
        {
            if(obj is StructType st)
                return st.structName == this.structName;
            
            return obj is NullType;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(StructType structType, Type t)
        {
            return structType.Equals(t);
        }

        public static bool operator !=(StructType structType, Type t)
        {
            return !structType.Equals(t);
        }
    }
}