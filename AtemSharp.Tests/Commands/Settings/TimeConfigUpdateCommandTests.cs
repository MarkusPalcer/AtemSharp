using AtemSharp.Commands.Settings;
using AtemSharp.State;
using AtemSharp.State.Settings;

namespace AtemSharp.Tests.Commands.Settings;

[TestFixture]
internal class TimeConfigUpdateCommandTests : DeserializedCommandTestBase<TimeConfigUpdateCommand, TimeConfigUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public TimeMode Mode { get; set; }
    }

    internal override void CompareCommandProperties(TimeConfigUpdateCommand actualCommand, CommandData expectedData)
    {
        Assert.That(actualCommand.Mode, Is.EqualTo(expectedData.Mode));
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        Assert.That(state.Settings.TimeMode, Is.EqualTo(expectedData.Mode));
    }
}
