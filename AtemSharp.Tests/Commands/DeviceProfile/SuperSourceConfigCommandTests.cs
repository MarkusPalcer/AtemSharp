using AtemSharp.Commands.DeviceProfile;
using AtemSharp.State;
using AtemSharp.State.Info;
using AtemSharp.Tests.TestUtilities;

namespace AtemSharp.Tests.Commands.DeviceProfile;

[TestFixture]
public class SuperSourceConfigCommandTests : DeserializedCommandTestBase<SuperSourceConfigCommand,
    SuperSourceConfigCommandTests.CommandData>
{
    [MaxProtocolVersion(ProtocolVersion.V7_5_2)]
    public class CommandData : CommandDataBase
    {
        public int SsrcId { get; set; }
        public byte BoxCount { get; set; }

        // For older protocol versions that may use "Boxes" property in test data
        public byte Boxes
        {
            get => BoxCount;
            set => BoxCount = value;
        }
    }

    protected override void CompareCommandProperties(SuperSourceConfigCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.BoxCount, Is.EqualTo(expectedData.BoxCount));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Info.SuperSources = AtemStateUtil.CreateArray<SuperSourceInfo>(1);
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        Assert.That(state.Info.SuperSources[0].BoxCount, Is.EqualTo(expectedData.BoxCount));
    }
}
