using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.State;
using AtemSharp.State.Video;
using AtemSharp.State.Video.MixEffect;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

[TestFixture]
public class TransitionStingerUpdateCommandTests : DeserializedCommandTestBase<TransitionStingerUpdateCommand,
    TransitionStingerUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public byte Source { get; set; }
        public bool PreMultipliedKey { get; set; }
        public double Clip { get; set; }
        public double Gain { get; set; }
        public bool Invert { get; set; }
        public ushort Preroll { get; set; }
        public ushort ClipDuration { get; set; }
        public ushort TriggerPoint { get; set; }
        public ushort MixRate { get; set; }
    }

    protected override void CompareCommandProperties(TransitionStingerUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.MixEffectId, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.Source, Is.EqualTo(expectedData.Source));
        Assert.That(actualCommand.PreMultipliedKey, Is.EqualTo(expectedData.PreMultipliedKey));
        Assert.That(actualCommand.Clip, Is.EqualTo(expectedData.Clip).Within(0.1));
        Assert.That(actualCommand.Gain, Is.EqualTo(expectedData.Gain).Within(0.1));
        Assert.That(actualCommand.Invert, Is.EqualTo(expectedData.Invert));
        Assert.That(actualCommand.Preroll, Is.EqualTo(expectedData.Preroll));
        Assert.That(actualCommand.ClipDuration, Is.EqualTo(expectedData.ClipDuration));
        Assert.That(actualCommand.TriggerPoint, Is.EqualTo(expectedData.TriggerPoint));
        Assert.That(actualCommand.MixRate, Is.EqualTo(expectedData.MixRate));
    }

    [Test]
    public void ApplyToState_ValidState_UpdatesCorrectly()
    {
        // Arrange
        var state = new AtemState
        {
            Video = new VideoState
            {
                MixEffects = [new MixEffect { Id = 0 }, new MixEffect { Id = 1 }]
            }
        };

        var command = new TransitionStingerUpdateCommand
        {
            MixEffectId = 1,
            Source = 5,
            PreMultipliedKey = true,
            Clip = 25.5,
            Gain = 75.0,
            Invert = true,
            Preroll = 1000,
            ClipDuration = 2000,
            TriggerPoint = 1500,
            MixRate = 30
        };

        // Act
        command.ApplyToState(state);

        // Assert
        var stingerSettings = state.Video.MixEffects[1].TransitionSettings.Stinger;
        Assert.That(stingerSettings, Is.Not.Null);
        Assert.That(stingerSettings.Source, Is.EqualTo(5));
        Assert.That(stingerSettings.PreMultipliedKey, Is.EqualTo(true));
        Assert.That(stingerSettings.Clip, Is.EqualTo(25.5));
        Assert.That(stingerSettings.Gain, Is.EqualTo(75.0));
        Assert.That(stingerSettings.Invert, Is.EqualTo(true));
        Assert.That(stingerSettings.Preroll, Is.EqualTo(1000));
        Assert.That(stingerSettings.ClipDuration, Is.EqualTo(2000));
        Assert.That(stingerSettings.TriggerPoint, Is.EqualTo(1500));
        Assert.That(stingerSettings.MixRate, Is.EqualTo(30));
    }
}
