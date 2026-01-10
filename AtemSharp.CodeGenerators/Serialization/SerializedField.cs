namespace AtemSharp.CodeGenerators.Serialization
{
    public class SerializedField
    {
        public string PropertyCode { get; set; } = string.Empty;
        public string SerializationCode { get; set; } = string.Empty;
        public string NamespaceCode { get; set; } = string.Empty;
        public string MergeComparison { get; set; } =  string.Empty;
        public string MergeCode { get; set; } = string.Empty;
    }
}
