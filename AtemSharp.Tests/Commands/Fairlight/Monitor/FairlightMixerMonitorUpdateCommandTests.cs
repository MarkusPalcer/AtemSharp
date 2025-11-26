using FairlightMixerMonitorUpdateCommand = AtemSharp.Commands.Audio.Fairlight.Monitor.FairlightMixerMonitorUpdateCommand;

namespace AtemSharp.Tests.Commands.Fairlight.Monitor;

public class FairlightMixerMonitorUpdateCommandTests : DeserializedCommandTestBase<FairlightMixerMonitorUpdateCommand, FairlightMixerMonitorUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public double Gain { get; set; }
        public double InputMasterGain { get; set; }
        public bool InputMasterEnabled { get; set; }
        public double InputTalkbackGain { get; set; }
        public double InputSidetoneGain { get; set; }
    }

    protected override void CompareCommandProperties(FairlightMixerMonitorUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.Gain, Is.EqualTo(expectedData.Gain).Within(0.01));
        Assert.That(actualCommand.InputMasterGain, Is.EqualTo(expectedData.InputMasterGain).Within(0.01));
        Assert.That(actualCommand.InputMasterMuted, Is.EqualTo(expectedData.InputMasterEnabled));
        Assert.That(actualCommand.InputTalkbackGain, Is.EqualTo(expectedData.InputTalkbackGain).Within(0.01));
        Assert.That(actualCommand.InputSidetoneGain, Is.EqualTo(expectedData.InputSidetoneGain).Within(0.01));
    }
}
