using AtemSharp.Commands.Audio.Fairlight;
using AtemSharp.State.Audio.Fairlight;
using AtemSharp.State.Info;
using AtemSharp.State.Ports;

namespace AtemSharp.Tests.Commands.Fairlight;

public class FairlightMixerInputUpdateCommandTests : DeserializedCommandTestBase<FairlightMixerInputUpdateCommand, FairlightMixerInputUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int Index { get; set; }
        public FairlightInputType InputType { get; set; }
        public ExternalPortType ExternalPortType { get; set; }
        public FairlightInputConfiguration SupportedConfigurations { get; set; }
        public FairlightInputConfiguration ActiveConfiguration { get; set; }

        // Unknown how to test
        public bool SupportsRcaToXlr { get; set; }
        public bool RcaToXlrEnabled { get; set; }

        public FairlightAnalogInputLevel SupportedInputLevels { get; set; }
        public FairlightAnalogInputLevel ActiveInputLevel { get; set; }
    }

    protected override void CompareCommandProperties(FairlightMixerInputUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.Id, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.InputType, Is.EqualTo(expectedData.InputType));
        Assert.That(actualCommand.ExternalPortType, Is.EqualTo(expectedData.ExternalPortType));
        Assert.That(CommandTestUtilities.CombineComponents(actualCommand.SupportedConfigurations), Is.EqualTo(expectedData.SupportedConfigurations));
        Assert.That(actualCommand.ActiveConfiguration, Is.EqualTo(expectedData.ActiveConfiguration));

        if (testCase.FirstVersion >= ProtocolVersion.V8_1_1)
        {
            Assert.That(CommandTestUtilities.CombineComponents(actualCommand.SupportedInputLevels), Is.EqualTo(expectedData.SupportedInputLevels));
            Assert.That(actualCommand.ActiveInputLevel, Is.EqualTo(expectedData.ActiveInputLevel));
        }
    }
}
