namespace AtemSharp.Helpers;

[AttributeUsage(AttributeTargets.Field)]
public class ScalingFactorAttribute : Attribute
{
    public ScalingFactorAttribute(double factor)
    {
        Factor = factor;
    }

    public double Factor { get; }
}
