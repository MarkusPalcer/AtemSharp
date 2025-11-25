using AtemSharp.State.Info;

namespace AtemSharp.Tests.TestUtilities;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class MaxProtocolVersionAttribute : Attribute
{
    public MaxProtocolVersionAttribute(ProtocolVersion maxVersion)
    {
        MaxVersion = maxVersion;
    }

    public ProtocolVersion MaxVersion { get; }

}
