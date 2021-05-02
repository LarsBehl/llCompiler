namespace LL.Types
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

        public override bool Equals(object obj)
        {
            switch(this)
            {
                case IntType it:
                    return it.Equals(obj);
                case DoubleType dt:
                    return dt.Equals(obj);
                case BooleanType bt:
                    return bt.Equals(obj);
                case VoidType vt:
                    return vt.Equals(obj);
                case BoolArrayType boolArrayType:
                    return boolArrayType.Equals(obj);
                case IntArrayType intArrayType:
                    return intArrayType.Equals(obj);
                case DoubleArrayType doubleArrayType:
                    return doubleArrayType.Equals(obj);
                case NullType nullType:
                    return nullType.Equals(obj);
                case StructType structType:
                    return structType.Equals(obj);
                default:
                    return false;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Type t1, Type t2)
        {
            switch(t1)
            {
                case IntType it:
                    return it == t2;
                case DoubleType dt:
                    return dt == t2;
                case BooleanType bt:
                    return bt == t2;
                case VoidType vt:
                    return vt == t2;
                case BoolArrayType boolArrayType:
                    return boolArrayType == t2;
                case IntArrayType intArrayType:
                    return intArrayType == t2;
                case DoubleArrayType doubleArrayType:
                    return doubleArrayType == t2;
                case NullType nullType:
                    return nullType == t2;
                case StructType structType:
                    return structType == t2;
                default:
                    return false;
            }
        }

        public static bool operator !=(Type t1, Type t2)
        {
            switch(t1)
            {
                case IntType it:
                    return it != t2;
                case DoubleType dt:
                    return dt != t2;
                case BooleanType bt:
                    return bt != t2;
                case VoidType vt:
                    return vt != t2;
                case BoolArrayType boolArrayType:
                    return boolArrayType != t2;
                case IntArrayType intArrayType:
                    return intArrayType != t2;
                case DoubleArrayType doubleArrayType:
                    return doubleArrayType != t2;
                case NullType nullType:
                    return nullType != t2;
                case StructType structType:
                    return structType != t2;
                default:
                    return false;
            }
        }
    }
}