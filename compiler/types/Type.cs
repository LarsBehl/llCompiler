namespace ll.type
{
    public abstract class Type
    {
        public string typeName { get; set; }

        public Type(string typeName)
        {
            this.typeName = typeName;
        }

        public virtual bool IsPrimitivType()
        {
            return false;
        }
    }
}