using AtemSharp.Commands.Macro;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.Macro;

internal class MacroRunStatusUpdateCommandTests : DeserializedCommandTestBase<MacroRunStatusUpdateCommand,
    MacroRunStatusUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public bool IsRunning { get; set; }
        public bool Loop { get; set; }
        public bool IsWaiting { get; set; }
        public ushort Index { get; set; }
    }

    internal override void CompareCommandProperties(MacroRunStatusUpdateCommand actualCommand, CommandData expectedData)
    {
        Assert.That(actualCommand.IsRunning, Is.EqualTo(expectedData.IsRunning));
        Assert.That(actualCommand.Loop, Is.EqualTo(expectedData.Loop));
        Assert.That(actualCommand.IsWaiting, Is.EqualTo(expectedData.IsWaiting));
        Assert.That(actualCommand.MacroIndex, Is.EqualTo(expectedData.Index));
    }

    protected override void PrepareState(IStateHolder stateHolder, CommandData testData)
    {
        stateHolder.Macros.GetOrCreate(testData.Index);
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        throw new NotImplementedException();
    }

    protected override void CompareStateProperties(IStateHolder state, CommandData expectedData)
    {
        if (expectedData.IsRunning)
        {
            Assert.That(state.Macros.Player.CurrentlyPlaying, Is.SameAs(state.Macros[expectedData.Index]));
        }
        else
        {
            Assert.That(state.Macros.Player.CurrentlyPlaying, Is.Null);
        }

        Assert.That(state.Macros.Player.PlayLooped, Is.EqualTo(expectedData.Loop));
        Assert.That(state.Macros.Player.PlaybackIsWaitingForUserAction, Is.EqualTo(expectedData.IsWaiting));
    }
}
