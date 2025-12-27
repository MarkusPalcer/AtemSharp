using AtemSharp.Commands.DeviceProfile;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.DeviceProfile;

[TestFixture]
internal class SuperSourceConfigCommandV8Tests : DeserializedCommandTestBase<SuperSourceConfigCommandV8,
    SuperSourceConfigCommandV8Tests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte SsrcId { get; set; }
        public byte BoxCount { get; set; }

        // For older protocol versions that may use "Boxes" property in test data
        public byte Boxes
        {
            get => BoxCount;
            set => BoxCount = value;
        }
    }

    internal override void CompareCommandProperties(SuperSourceConfigCommandV8 actualCommand, CommandData expectedData)
    {
        Assert.That(actualCommand.SuperSourceId, Is.EqualTo(expectedData.SsrcId));
        Assert.That(actualCommand.BoxCount, Is.EqualTo(expectedData.BoxCount));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Info.SuperSources.GetOrCreate(expectedData.SsrcId);
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        Assert.That(state.Info.SuperSources[expectedData.SsrcId].BoxCount, Is.EqualTo(expectedData.BoxCount));
    }
}
