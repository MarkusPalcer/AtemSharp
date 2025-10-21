using System.Xml.Schema;

namespace AtemSharp.Helpers;

[AttributeUsage(AttributeTargets.Field)]
public class CustomScalingAttribute : Attribute
{
    public CustomScalingAttribute(string scalingMethod) {}
}
