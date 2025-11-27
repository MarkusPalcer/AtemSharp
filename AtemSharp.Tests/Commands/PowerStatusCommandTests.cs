using AtemSharp.Commands;

namespace AtemSharp.Tests.Commands;

[TestFixture]
public class PowerStatusCommandTests : DeserializedCommandTestBase<PowerStatusCommand, PowerStatusCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public bool Pin1 { get; set; }
        public bool Pin2 { get; set; }
    }

    protected override void CompareCommandProperties(PowerStatusCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.PowerSupplies[0], Is.EqualTo(expectedData.Pin1));
        Assert.That(actualCommand.PowerSupplies[1], Is.EqualTo(expectedData.Pin2));
    }
}
