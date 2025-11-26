using AtemSharp.Commands.Settings;
using AtemSharp.State.Settings;

namespace AtemSharp.Tests.Commands.Settings;

[TestFixture]
public class TimeConfigUpdateCommandTests : DeserializedCommandTestBase<TimeConfigUpdateCommand, TimeConfigUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public TimeMode Mode { get; set; }
    }

    protected override void CompareCommandProperties(TimeConfigUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.Mode, Is.EqualTo(expectedData.Mode));
    }
}
