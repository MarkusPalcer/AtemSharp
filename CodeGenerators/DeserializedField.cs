namespace CodeGenerators
{
    public class DeserializedField
    {
        public DeserializedField(string name, string propertyName, string type, uint offset, string documentationComment, string? deserializeExpression)
        {
            Name = name;
            Type = type;
            Offset = offset;
            DocumentationComment = documentationComment;
            PropertyName = propertyName;
            DeserializeExpression = deserializeExpression;
        }

        /// <summary>
        ///  The name of the field
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The name of the property associated with this field
        /// </summary>
        /// <example>
        /// When the field name is "public byte videoMode;" or "public byte _videoMode;", the property name would be "VideoMode"
        /// </example>
        public string PropertyName { get; set; }

        /// <summary>
        /// The full type name of the field's type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The offset value from the DeserializedFieldAttribute
        /// </summary>
        public uint Offset { get; set; }

        /// <summary>
        /// The C# expression to use for deserializing this field
        /// </summary>
        public string? DeserializeExpression { get; set; }

        public string DocumentationComment { get; set; }
    }
}
