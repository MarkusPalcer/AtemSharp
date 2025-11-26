using AtemSharp.Commands.Inputs;
using AtemSharp.State.Ports;
using AtemSharp.State.Video.InputChannel;

namespace AtemSharp.Tests.Commands.Inputs;

[TestFixture]
public class InputPropertiesUpdateCommandTests : DeserializedCommandTestBase<InputPropertiesUpdateCommand,
    InputPropertiesUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort Id { get; set; }
        public string LongName { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
        public bool AreNamesDefault { get; set; }

        public ushort AvailableExternalPorts { get; set; }

        public ExternalPortType ExternalPortType { get; set; }
        public InternalPortType InternalPortType { get; set; }
        public SourceAvailability SourceAvailability { get; set; }
        public MeAvailability MeAvailability { get; set; }
    }

    protected override void CompareCommandProperties(InputPropertiesUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.InputId, Is.EqualTo(testCase.Command.Id));
        Assert.That(actualCommand.LongName, Is.EqualTo(testCase.Command.LongName));
        Assert.That(actualCommand.ShortName, Is.EqualTo(testCase.Command.ShortName));
        Assert.That(actualCommand.AreNamesDefault, Is.EqualTo(testCase.Command.AreNamesDefault));
        Assert.That(actualCommand.ExternalPortType, Is.EqualTo(testCase.Command.ExternalPortType));
        Assert.That(actualCommand.InternalPortType, Is.EqualTo(testCase.Command.InternalPortType));
        Assert.That(actualCommand.SourceAvailability, Is.EqualTo(testCase.Command.SourceAvailability));
        Assert.That(actualCommand.MeAvailability, Is.EqualTo(testCase.Command.MeAvailability));
    }
}
