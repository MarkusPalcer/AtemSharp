using AtemSharp.Commands.Audio.Fairlight;
using AtemSharp.State;
using AtemSharp.State.Audio.Fairlight;
using AtemSharp.State.Info;
using AtemSharp.State.Ports;
using AtemSharp.Tests.TestUtilities;

namespace AtemSharp.Tests.Commands.Fairlight;

internal class FairlightMixerInputUpdateCommandTests : DeserializedCommandTestBase<FairlightMixerInputUpdateCommand,
    FairlightMixerInputUpdateCommandTests.CommandData>
{
    [MaxProtocolVersion(ProtocolVersion.V8_0_1)]
    public class CommandData : CommandDataBase
    {
        public ushort Index { get; set; }
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

    internal override void CompareCommandProperties(FairlightMixerInputUpdateCommand actualCommand, CommandData expectedData, TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        Assert.That(actualCommand.Id, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.InputType, Is.EqualTo(expectedData.InputType));
        Assert.That(actualCommand.ExternalPortType, Is.EqualTo(expectedData.ExternalPortType));
        Assert.That(actualCommand.SupportedConfigurations.CombineComponents(),
                    Is.EqualTo(expectedData.SupportedConfigurations));
        Assert.That(actualCommand.ActiveConfiguration, Is.EqualTo(expectedData.ActiveConfiguration));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Audio = new FairlightAudioState();
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        var target = state.GetFairlight().Inputs[expectedData.Index];
        Assert.That(target.Id, Is.EqualTo(expectedData.Index));
        Assert.That(target.InputType, Is.EqualTo(expectedData.InputType));
        Assert.That(target.ExternalPortType, Is.EqualTo(expectedData.ExternalPortType));
        Assert.That(target.SupportedConfigurations.CombineComponents(),
                    Is.EqualTo(expectedData.SupportedConfigurations));
        Assert.That(target.ActiveConfiguration, Is.EqualTo(expectedData.ActiveConfiguration));
        Assert.That(target.SupportedInputLevels, Is.Empty);
        Assert.That(target.ActiveInputLevel, Is.EqualTo(FairlightAnalogInputLevel.None));
    }
}
