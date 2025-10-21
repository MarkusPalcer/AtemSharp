namespace AtemSharp.Helpers;

[AttributeUsage(AttributeTargets.Field)]
public class SerializedFieldAttribute : Attribute
{
    public SerializedFieldAttribute(int offset, byte flag)
    {
    }

    public SerializedFieldAttribute(int offset)
    {
    }
}
