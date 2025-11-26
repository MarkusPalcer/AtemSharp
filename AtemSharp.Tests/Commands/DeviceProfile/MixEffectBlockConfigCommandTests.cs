using AtemSharp.Commands.DeviceProfile;

namespace AtemSharp.Tests.Commands.DeviceProfile;

[TestFixture]
public class MixEffectBlockConfigCommandTests : DeserializedCommandTestBase<MixEffectBlockConfigCommand, MixEffectBlockConfigCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public byte KeyCount { get; set; }
    }

    protected override void CompareCommandProperties(MixEffectBlockConfigCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.Index, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.KeyCount, Is.EqualTo(expectedData.KeyCount));
    }
}
