namespace ll.type
{
    public class StructType : RefType
    {
        public string structName { get; set; }
        public StructType(string structName) : base("StructType")
        {
            this.structName = structName;
        }
    }
}