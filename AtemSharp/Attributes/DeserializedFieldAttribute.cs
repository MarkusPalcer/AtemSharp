namespace AtemSharp.Helpers;

[AttributeUsage(AttributeTargets.Field)]
public class DeserializedFieldAttribute : Attribute
{
    public DeserializedFieldAttribute(int offset)
    {
        Offset = offset;
    }

    public int Offset { get; set; }
}
