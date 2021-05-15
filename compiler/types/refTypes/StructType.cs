namespace LL.Types
{
    public class StructType : RefType
    {
        public string StructName { get; set; }
        
        public StructType(string structName) : base("StructType")
        {
            this.StructName = structName;
        }

        public override string ToString()
        {
            return $"{this.TypeName}: {this.StructName}";
        }

        public override bool Equals(object obj)
        {
            if(obj is StructType st)
                return st.StructName == this.StructName;
            
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