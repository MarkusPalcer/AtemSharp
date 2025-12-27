using AtemSharp.Commands.DeviceProfile;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.DeviceProfile;

[TestFixture]
internal class MacroPoolConfigCommandTests : DeserializedCommandTestBase<MacroPoolConfigCommand, MacroPoolConfigCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte MacroCount { get; set; }
    }

    internal override void CompareCommandProperties(MacroPoolConfigCommand actualCommand, CommandData expectedData)
    {
        Assert.That(actualCommand.MacroCount, Is.EqualTo(expectedData.MacroCount));
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        throw new NotImplementedException();
    }

    protected override void CompareStateProperties(IStateHolder stateHolder, CommandData expectedData)
    {
        Assert.That(stateHolder.State.Info.MacroPool.MacroCount, Is.EqualTo(expectedData.MacroCount));
        Assert.That(stateHolder.Macros.Select(x => x.Id), Is.EquivalentTo(Enumerable.Range(0, expectedData.MacroCount)));
    }
}
