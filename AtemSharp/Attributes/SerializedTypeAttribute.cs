namespace AtemSharp.Helpers;

[AttributeUsage(AttributeTargets.Field)]
public class SerializedTypeAttribute : Attribute
{
    public SerializedTypeAttribute(Type type) {}
}
