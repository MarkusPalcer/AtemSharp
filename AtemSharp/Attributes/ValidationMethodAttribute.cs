namespace AtemSharp.Helpers;

[AttributeUsage(AttributeTargets.Field)]
public class ValidationMethodAttribute : Attribute
{
    public ValidationMethodAttribute(string methodName) {}
}
