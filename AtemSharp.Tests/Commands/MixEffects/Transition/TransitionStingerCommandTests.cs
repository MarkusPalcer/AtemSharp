using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.State.Video.MixEffect;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

[TestFixture]
public class TransitionStingerCommandTests : SerializedCommandTestBase<TransitionStingerCommand,
    TransitionStingerCommandTests.CommandData>
{
    /// <inheritdoc />
    protected override Range[] GetFloatingPointByteRanges()
    {
        return
        [
            6..8, // Clip
            8..10 // Gain
        ];
    }

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

    protected override TransitionStingerCommand CreateSut(TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        return new TransitionStingerCommand(new MixEffect
        {
            Id = testCase.Command.Index,
            TransitionSettings =
            {
                Stinger =
                {
                    Source = testCase.Command.Source,
                    PreMultipliedKey =
                    {
                        Enabled = testCase.Command.PreMultipliedKey,
                        Clip = testCase.Command.Clip,
                        Gain = testCase.Command.Gain,
                        Inverted = testCase.Command.Invert,
                    },
                    Preroll = testCase.Command.Preroll,
                    ClipDuration = testCase.Command.ClipDuration,
                    TriggerPoint = testCase.Command.TriggerPoint,
                    MixRate = testCase.Command.MixRate
                }
            }
        });
    }

    [Test]
    public void PropertyChanges_SetCorrectFlags()
    {
        // Arrange
        var state = new MixEffect();
        var command = new TransitionStingerCommand(state);

        // Act & Assert
        command.Source = 5;
        Assert.That(command.Flag & (1 << 0), Is.Not.EqualTo(0), "Source flag should be set");

        command.PreMultipliedKey = true;
        Assert.That(command.Flag & (1 << 1), Is.Not.EqualTo(0), "PreMultipliedKey flag should be set");

        command.Clip = 50.0;
        Assert.That(command.Flag & (1 << 2), Is.Not.EqualTo(0), "Clip flag should be set");

        command.Gain = 75.0;
        Assert.That(command.Flag & (1 << 3), Is.Not.EqualTo(0), "Gain flag should be set");

        command.Invert = true;
        Assert.That(command.Flag & (1 << 4), Is.Not.EqualTo(0), "Invert flag should be set");

        command.Preroll = 1000;
        Assert.That(command.Flag & (1 << 5), Is.Not.EqualTo(0), "Preroll flag should be set");

        command.ClipDuration = 2000;
        Assert.That(command.Flag & (1 << 6), Is.Not.EqualTo(0), "ClipDuration flag should be set");

        command.TriggerPoint = 1500;
        Assert.That(command.Flag & (1 << 7), Is.Not.EqualTo(0), "TriggerPoint flag should be set");

        command.MixRate = 30;
        Assert.That(command.Flag & (1 << 8), Is.Not.EqualTo(0), "MixRate flag should be set");
    }
}
