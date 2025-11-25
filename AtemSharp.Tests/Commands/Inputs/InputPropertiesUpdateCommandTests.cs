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

        // TODO: Figure out the relationship between AvailableExternalPorts in test data and ExternalPorts property on the command
        // The test data contains AvailableExternalPorts (ushort) but the command has ExternalPorts (ExternalPortType[]?)
        // Need to understand how these relate and if one should be derived from the other
        public ushort AvailableExternalPorts { get; set; }

        public ExternalPortType ExternalPortType { get; set; }
        public InternalPortType InternalPortType { get; set; }
        public SourceAvailability SourceAvailability { get; set; }
        public MeAvailability MeAvailability { get; set; }
    }

    protected override void CompareCommandProperties(InputPropertiesUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        var failures = new List<string>();

        // Compare all properties
        if (!actualCommand.InputId.Equals(expectedData.Id))
        {
            failures.Add($"InputId: expected {expectedData.Id}, actual {actualCommand.InputId}");
        }

        if (!actualCommand.LongName.Equals(expectedData.LongName))
        {
            failures.Add($"LongName: expected '{expectedData.LongName}', actual '{actualCommand.LongName}'");
        }

        if (!actualCommand.ShortName.Equals(expectedData.ShortName))
        {
            failures.Add($"ShortName: expected '{expectedData.ShortName}', actual '{actualCommand.ShortName}'");
        }

        if (!actualCommand.AreNamesDefault.Equals(expectedData.AreNamesDefault))
        {
            failures.Add($"AreNamesDefault: expected {expectedData.AreNamesDefault}, actual {actualCommand.AreNamesDefault}");
        }

        // TODO: Skip ExternalPorts comparison - there's a mismatch between test data and command structure
        // Test data contains "AvailableExternalPorts" (ushort) but command has "ExternalPorts" (ExternalPortType[]?)
        // Need to understand the relationship between these fields and how they should be used

        if (!actualCommand.ExternalPortType.Equals(expectedData.ExternalPortType))
        {
            failures.Add($"ExternalPortType: expected {expectedData.ExternalPortType}, actual {actualCommand.ExternalPortType}");
        }

        if (!actualCommand.InternalPortType.Equals(expectedData.InternalPortType))
        {
            failures.Add($"InternalPortType: expected {expectedData.InternalPortType}, actual {actualCommand.InternalPortType}");
        }

        if (!actualCommand.SourceAvailability.Equals(expectedData.SourceAvailability))
        {
            failures.Add($"SourceAvailability: expected {expectedData.SourceAvailability}, actual {actualCommand.SourceAvailability}");
        }

        if (!actualCommand.MeAvailability.Equals(expectedData.MeAvailability))
        {
            failures.Add($"MeAvailability: expected {expectedData.MeAvailability}, actual {actualCommand.MeAvailability}");
        }

        // Assert results
        if (failures.Count > 0)
        {
            Assert.Fail($"Command deserialization property mismatch for version {testCase.FirstVersion}:\n" +
                       string.Join("\n", failures));
        }
    }
}
