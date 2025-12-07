using AtemSharp.Commands.Inputs;
using AtemSharp.State;
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

    protected override void CompareCommandProperties(InputPropertiesUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.InputId, Is.EqualTo(expectedData.Id));
        Assert.That(actualCommand.LongName, Is.EqualTo(expectedData.LongName));
        Assert.That(actualCommand.ShortName, Is.EqualTo(expectedData.ShortName));
        Assert.That(actualCommand.AreNamesDefault, Is.EqualTo(expectedData.AreNamesDefault));
        Assert.That(actualCommand.ExternalPortType, Is.EqualTo(expectedData.ExternalPortType));
        Assert.That(actualCommand.InternalPortType, Is.EqualTo(expectedData.InternalPortType));
        Assert.That(actualCommand.SourceAvailability, Is.EqualTo(expectedData.SourceAvailability));
        Assert.That(actualCommand.MeAvailability, Is.EqualTo(expectedData.MeAvailability));
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        var actualCommand = state.Video.Inputs[expectedData.Id];
        Assert.That(actualCommand.InputId, Is.EqualTo(expectedData.Id));
        Assert.That(actualCommand.LongName, Is.EqualTo(expectedData.LongName));
        Assert.That(actualCommand.ShortName, Is.EqualTo(expectedData.ShortName));
        Assert.That(actualCommand.AreNamesDefault, Is.EqualTo(expectedData.AreNamesDefault));
        Assert.That(actualCommand.ExternalPortType, Is.EqualTo(expectedData.ExternalPortType));
        Assert.That(actualCommand.InternalPortType, Is.EqualTo(expectedData.InternalPortType));
        Assert.That(actualCommand.SourceAvailability, Is.EqualTo(expectedData.SourceAvailability));
        Assert.That(actualCommand.MeAvailability, Is.EqualTo(expectedData.MeAvailability));
    }
}
