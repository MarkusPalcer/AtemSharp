using AtemSharp.Commands;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands;

[TestFixture]
internal class PowerStatusCommandTests : DeserializedCommandTestBase<PowerStatusCommand, PowerStatusCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public bool Pin1 { get; set; }
        public bool Pin2 { get; set; }
    }

    internal override void CompareCommandProperties(PowerStatusCommand actualCommand, CommandData expectedData)
    {
        Assert.That(actualCommand.PowerSupplies[0], Is.EqualTo(expectedData.Pin1));
        Assert.That(actualCommand.PowerSupplies[1], Is.EqualTo(expectedData.Pin2));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Info.Power = [false, false];
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        Assert.That(state.Info.Power[0], Is.EqualTo(expectedData.Pin1));
        Assert.That(state.Info.Power[1], Is.EqualTo(expectedData.Pin2));
    }
}
