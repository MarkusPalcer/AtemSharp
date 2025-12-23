using AtemSharp.Commands.Audio.Fairlight;
using AtemSharp.State;
using AtemSharp.State.Audio.Fairlight;
using AtemSharp.State.Ports;

namespace AtemSharp.Tests.Commands.Fairlight;

internal class FairlightMixerInputUpdateV811CommandTests : DeserializedCommandTestBase<FairlightMixerInputUpdateV811Command, FairlightMixerInputUpdateV811CommandTests.CommandData>
{
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

    internal override void CompareCommandProperties(FairlightMixerInputUpdateV811Command actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.Id, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.InputType, Is.EqualTo(expectedData.InputType));
        Assert.That(actualCommand.ExternalPortType, Is.EqualTo(expectedData.ExternalPortType));
        Assert.That(actualCommand.SupportedConfigurations.CombineComponents(), Is.EqualTo(expectedData.SupportedConfigurations));
        Assert.That(actualCommand.ActiveConfiguration, Is.EqualTo(expectedData.ActiveConfiguration));
        Assert.That(actualCommand.SupportedInputLevels.CombineComponents(), Is.EqualTo(expectedData.SupportedInputLevels));
        Assert.That(actualCommand.ActiveInputLevel, Is.EqualTo(expectedData.ActiveInputLevel));
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
        Assert.That(target.SupportedConfigurations.CombineComponents(), Is.EqualTo(expectedData.SupportedConfigurations));
        Assert.That(target.ActiveConfiguration, Is.EqualTo(expectedData.ActiveConfiguration));
        Assert.That(target.SupportedInputLevels.CombineComponents(), Is.EqualTo(expectedData.SupportedInputLevels));
        Assert.That(target.ActiveInputLevel, Is.EqualTo(expectedData.ActiveInputLevel));
    }
}
