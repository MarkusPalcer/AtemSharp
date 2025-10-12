using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

[TestFixture]
public class TransitionPropertiesUpdateCommandTests : DeserializedCommandTestBase<TransitionPropertiesUpdateCommand,
    TransitionPropertiesUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public byte Style { get; set; }
        public byte Selection { get; set; }
        public byte NextStyle { get; set; }
        public byte NextSelection { get; set; }
    }

    protected override void CompareCommandProperties(TransitionPropertiesUpdateCommand actualCommand, 
        CommandData expectedData, TestCaseData testCase)
    {
        var failures = new List<string>();

        // Compare all properties
        if (actualCommand.MixEffectId != expectedData.Index)
        {
            failures.Add($"MixEffectId: expected {expectedData.Index}, actual {actualCommand.MixEffectId}");
        }

        if ((byte)actualCommand.Style != expectedData.Style)
        {
            failures.Add($"Style: expected {expectedData.Style}, actual {(byte)actualCommand.Style}");
        }

        if ((byte)actualCommand.Selection != expectedData.Selection)
        {
            failures.Add($"Selection: expected {expectedData.Selection}, actual {(byte)actualCommand.Selection}");
        }

        if ((byte)actualCommand.NextStyle != expectedData.NextStyle)
        {
            failures.Add($"NextStyle: expected {expectedData.NextStyle}, actual {(byte)actualCommand.NextStyle}");
        }

        if ((byte)actualCommand.NextSelection != expectedData.NextSelection)
        {
            failures.Add($"NextSelection: expected {expectedData.NextSelection}, actual {(byte)actualCommand.NextSelection}");
        }

        // Assert results
        if (failures.Count > 0)
        {
            Assert.Fail($"Command property comparison failed:\n{string.Join("\n", failures)}");
        }
    }

    [Test]
    public void Deserialize_ValidData_ParsesCorrectly()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 }; // mixEffect=1, style=2, selection=3, nextStyle=4, nextSelection=5
        using var stream = new MemoryStream(data);

        // Act
        var command = TransitionPropertiesUpdateCommand.Deserialize(stream, ProtocolVersion.V8_0);

        // Assert
        Assert.That(command.MixEffectId, Is.EqualTo(1));
        Assert.That(command.Style, Is.EqualTo((TransitionStyle)2));
        Assert.That(command.Selection, Is.EqualTo((TransitionSelection)3));
        Assert.That(command.NextStyle, Is.EqualTo((TransitionStyle)4));
        Assert.That(command.NextSelection, Is.EqualTo((TransitionSelection)5));
    }

    [Test]
    public void ApplyToState_ValidMixEffect_UpdatesTransitionProperties()
    {
        // Arrange
        var command = new TransitionPropertiesUpdateCommand
        {
            MixEffectId = 1,
            Style = TransitionStyle.Wipe,
            Selection = TransitionSelection.Background | TransitionSelection.Key1,
            NextStyle = TransitionStyle.DVE,
            NextSelection = TransitionSelection.Key2 | TransitionSelection.Key3
        };

        var state = CreateMinimalState();

        // Act
        var changedPaths = command.ApplyToState(state);

        // Assert
        Assert.That(changedPaths, Is.EqualTo(new[] { "video.mixEffects.1.transitionProperties" }));
        
        var mixEffect = state.Video.MixEffects[1];
        Assert.That(mixEffect.TransitionProperties, Is.Not.Null);
        Assert.That(mixEffect.TransitionProperties!.Style, Is.EqualTo(TransitionStyle.Wipe));
        Assert.That(mixEffect.TransitionProperties.Selection, Is.EqualTo(TransitionSelection.Background | TransitionSelection.Key1));
        Assert.That(mixEffect.TransitionProperties.NextStyle, Is.EqualTo(TransitionStyle.DVE));
        Assert.That(mixEffect.TransitionProperties.NextSelection, Is.EqualTo(TransitionSelection.Key2 | TransitionSelection.Key3));
    }

    [Test]
    public void ApplyToState_InvalidMixEffect_ThrowsInvalidIdError()
    {
        // Arrange
        var command = new TransitionPropertiesUpdateCommand
        {
            MixEffectId = 5, // Beyond capabilities
            Style = TransitionStyle.Mix,
            Selection = TransitionSelection.Background,
            NextStyle = TransitionStyle.Mix,
            NextSelection = TransitionSelection.Background
        };

        var state = CreateMinimalState(); // Only has 2 mix effects

        // Act & Assert
        var ex = Assert.Throws<InvalidIdError>(() => command.ApplyToState(state));
        Assert.That(ex!.Message, Does.Contain("MixEffect"));
        Assert.That(ex.Message, Does.Contain("5"));
    }

    [Test]
    public void ApplyToState_ExistingTransitionProperties_UpdatesInPlace()
    {
        // Arrange
        var command = new TransitionPropertiesUpdateCommand
        {
            MixEffectId = 0,
            Style = TransitionStyle.Sting,
            Selection = TransitionSelection.Key4,
            NextStyle = TransitionStyle.Dip,
            NextSelection = TransitionSelection.Background
        };

        var state = CreateMinimalState();
        // Pre-populate with existing transition properties
        state.Video.MixEffects[0].TransitionProperties = new TransitionProperties
        {
            Style = TransitionStyle.Mix,
            Selection = TransitionSelection.Background,
            NextStyle = TransitionStyle.Mix,
            NextSelection = TransitionSelection.Background
        };

        // Act
        var changedPaths = command.ApplyToState(state);

        // Assert
        Assert.That(changedPaths, Is.EqualTo(new[] { "video.mixEffects.0.transitionProperties" }));
        
        var mixEffect = state.Video.MixEffects[0];
        Assert.That(mixEffect.TransitionProperties, Is.Not.Null);
        Assert.That(mixEffect.TransitionProperties!.Style, Is.EqualTo(TransitionStyle.Sting));
        Assert.That(mixEffect.TransitionProperties.Selection, Is.EqualTo(TransitionSelection.Key4));
        Assert.That(mixEffect.TransitionProperties.NextStyle, Is.EqualTo(TransitionStyle.Dip));
        Assert.That(mixEffect.TransitionProperties.NextSelection, Is.EqualTo(TransitionSelection.Background));
    }

    private static AtemState CreateMinimalState()
    {
        return new AtemState
        {
            Info = new DeviceInfo
            {
                Capabilities = new AtemCapabilities
                {
                    MixEffects = 2
                }
            },
            Video = new VideoState
            {
                MixEffects = new Dictionary<int, MixEffect>
                {
                    [0] = new MixEffect { Index = 0 },
                    [1] = new MixEffect { Index = 1 }
                }
            }
        };
    }
}