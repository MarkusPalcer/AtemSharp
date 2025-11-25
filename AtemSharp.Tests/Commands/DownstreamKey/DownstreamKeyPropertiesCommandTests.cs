using AtemSharp.Commands.DownstreamKey;
using AtemSharp.State;
using AtemSharp.State.Video;
using AtemSharp.State.Video.DownstreamKeyer;

namespace AtemSharp.Tests.Commands.DownstreamKey;

[TestFixture]
public class DownstreamKeyPropertiesCommandTests : DeserializedCommandTestBase<DownstreamKeyPropertiesCommand,
    DownstreamKeyPropertiesCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int Index { get; set; }
        public bool Tie { get; set; }
        public int Rate { get; set; }
        public bool PreMultipliedKey { get; set; }
        public double Clip { get; set; }
        public double Gain { get; set; }
        public bool Invert { get; set; }
        public bool MaskEnabled { get; set; }
        public double MaskTop { get; set; }
        public double MaskBottom { get; set; }
        public double MaskLeft { get; set; }
        public double MaskRight { get; set; }
    }

    protected override void CompareCommandProperties(DownstreamKeyPropertiesCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.DownstreamKeyerId, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.Tie, Is.EqualTo(expectedData.Tie));
        Assert.That(actualCommand.Rate, Is.EqualTo(expectedData.Rate));
        Assert.That(actualCommand.PreMultiply, Is.EqualTo(expectedData.PreMultipliedKey));
        Assert.That(actualCommand.Clip, Is.EqualTo(expectedData.Clip).Within(0.1));
        Assert.That(actualCommand.Gain, Is.EqualTo(expectedData.Gain).Within(0.1));
        Assert.That(actualCommand.Invert, Is.EqualTo(expectedData.Invert));
        Assert.That(actualCommand.MaskEnabled, Is.EqualTo(expectedData.MaskEnabled));
        Assert.That(actualCommand.MaskTop, Is.EqualTo(expectedData.MaskTop).Within(0.001));
        Assert.That(actualCommand.MaskBottom, Is.EqualTo(expectedData.MaskBottom).Within(0.001));
        Assert.That(actualCommand.MaskLeft, Is.EqualTo(expectedData.MaskLeft).Within(0.001));
        Assert.That(actualCommand.MaskRight, Is.EqualTo(expectedData.MaskRight).Within(0.001));
    }

    [Test]
    public void ApplyToState_WithValidData_ShouldUpdateDownstreamKeyerProperties()
    {
        // Arrange
        var state = new AtemState();
        state.Video.DownstreamKeyers = AtemStateUtil.CreateArray<DownstreamKeyer>(2);

        var command = new DownstreamKeyPropertiesCommand
        {
            DownstreamKeyerId = 0,
            Tie = true,
            Rate = 25,
            PreMultiply = false,
            Clip = 50.0,
            Gain = 75.0,
            Invert = true,
            MaskEnabled = true,
            MaskTop = -5.0,
            MaskBottom = 5.0,
            MaskLeft = -10.0,
            MaskRight = 10.0
        };

        // Act
        command.ApplyToState(state);

        // Assert
        Assert.That(state.Video, Is.Not.Null);
        Assert.That(state.Video.DownstreamKeyers.Count, Is.GreaterThan(0));
        Assert.That(state.Video.DownstreamKeyers[0], Is.Not.Null);
        Assert.That(state.Video.DownstreamKeyers[0].Properties, Is.Not.Null);

        var properties = state.Video.DownstreamKeyers[0].Properties;
        Assert.That(properties.Tie, Is.EqualTo(true));
        Assert.That(properties.Rate, Is.EqualTo(25));
        Assert.That(properties.PreMultiply, Is.EqualTo(false));
        Assert.That(properties.Clip, Is.EqualTo(50.0));
        Assert.That(properties.Gain, Is.EqualTo(75.0));
        Assert.That(properties.Invert, Is.EqualTo(true));

        Assert.That(properties.Mask.Enabled, Is.EqualTo(true));
        Assert.That(properties.Mask.Top, Is.EqualTo(-5.0));
        Assert.That(properties.Mask.Bottom, Is.EqualTo(5.0));
        Assert.That(properties.Mask.Left, Is.EqualTo(-10.0));
        Assert.That(properties.Mask.Right, Is.EqualTo(10.0));
    }

    [Test]
    public void ApplyToState_WithDifferentDownstreamKeyerId_ShouldUpdateCorrectIndex()
    {
        // Arrange
        var state = new AtemState();
        state.Video.DownstreamKeyers = AtemStateUtil.CreateArray<DownstreamKeyer>(4);

        var command = new DownstreamKeyPropertiesCommand
        {
            DownstreamKeyerId = 2,
                Tie = false,
                Rate = 50,
                PreMultiply = true,
                Clip = 25.0,
                Gain = 30.0,
                Invert = false,
                    MaskEnabled = false,
                    MaskTop = 0.0,
                    MaskBottom = 0.0,
                    MaskLeft = 0.0,
                    MaskRight = 0.0
        };

        // Act
        command.ApplyToState(state);

        // Assert
        Assert.That(state.Video, Is.Not.Null);
        Assert.That(state.Video.DownstreamKeyers[2], Is.Not.Null);
        Assert.That(state.Video.DownstreamKeyers[2].Properties, Is.Not.Null);

        var properties = state.Video.DownstreamKeyers[2].Properties;
        Assert.That(properties.Tie, Is.EqualTo(false));
        Assert.That(properties.Rate, Is.EqualTo(50));
        Assert.That(properties.PreMultiply, Is.EqualTo(true));
        Assert.That(properties.Clip, Is.EqualTo(25.0));
        Assert.That(properties.Gain, Is.EqualTo(30.0));
        Assert.That(properties.Invert, Is.EqualTo(false));
        Assert.That(properties.Mask.Enabled, Is.EqualTo(false));
    }

    [Test]
    public void ApplyToState_WithInvalidDownstreamKeyerId_ShouldThrowInvalidIdError()
    {
        // Arrange
        var state = new AtemState();
        state.Video.DownstreamKeyers = AtemStateUtil.CreateArray<DownstreamKeyer>(2);

        var command = new DownstreamKeyPropertiesCommand
        {
            DownstreamKeyerId = 5, // Invalid - beyond available keyers
        };

        // Act & Assert
        Assert.Throws<IndexOutOfRangeException>(() => command.ApplyToState(state));
    }

    [Test]
    public void ApplyToState_WithExistingDownstreamKeyer_ShouldOverwriteProperties()
    {
        // Arrange
        var state = new AtemState();

        state.Video.DownstreamKeyers = AtemStateUtil.CreateArray<DownstreamKeyer>(2);
        state.Video.DownstreamKeyers[0] = new DownstreamKeyer
        {
            Properties = new DownstreamKeyerProperties
            {
                Tie = false,
                Rate = 100,
                PreMultiply = false,
                Clip = 10.0,
                Gain = 20.0,
                Invert = false,
                Mask = new MaskProperties { Enabled = false }
            }
        };

        var command = new DownstreamKeyPropertiesCommand
        {
            DownstreamKeyerId = 0,
                Tie = true,
                Rate = 30,
                PreMultiply = true,
                Clip = 80.0,
                Gain = 90.0,
                Invert = true,

                MaskEnabled = true,
                MaskTop = -1.0,
                MaskBottom = 1.0,
                MaskLeft = -2.0,
                MaskRight = 2.0
        };

        // Act
        command.ApplyToState(state);

        // Assert
        var properties = state.Video.DownstreamKeyers[0].Properties;
        Assert.That(properties.Tie, Is.EqualTo(true));
        Assert.That(properties.Rate, Is.EqualTo(30));
        Assert.That(properties.PreMultiply, Is.EqualTo(true));
        Assert.That(properties.Clip, Is.EqualTo(80.0));
        Assert.That(properties.Gain, Is.EqualTo(90.0));
        Assert.That(properties.Invert, Is.EqualTo(true));
        Assert.That(properties.Mask.Enabled, Is.EqualTo(true));
        Assert.That(properties.Mask.Top, Is.EqualTo(-1.0));
        Assert.That(properties.Mask.Bottom, Is.EqualTo(1.0));
        Assert.That(properties.Mask.Left, Is.EqualTo(-2.0));
        Assert.That(properties.Mask.Right, Is.EqualTo(2.0));
    }
}
