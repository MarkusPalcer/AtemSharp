using AtemSharp.Commands.DeviceProfile;
using AtemSharp.State;
using AtemSharp.State.Info;

namespace AtemSharp.Tests.Commands.DeviceProfile;

[TestFixture]
public class VersionCommandTests : DeserializedCommandTestBase<VersionCommand, VersionCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ProtocolVersion ProtocolVersion { get; set; }
    }

    protected override void CompareCommandProperties(VersionCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.Version, Is.EqualTo(expectedData.ProtocolVersion));
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        Assert.That(state.Info.ApiVersion, Is.EqualTo(expectedData.ProtocolVersion));
    }
}
