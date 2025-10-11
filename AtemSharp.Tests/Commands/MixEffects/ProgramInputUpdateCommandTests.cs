using AtemSharp.Commands;
using AtemSharp.Commands.MixEffects;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.MixEffects;

[TestFixture]
public class ProgramInputUpdateCommandTests : DeserializedCommandTestBase<ProgramInputUpdateCommand,
    ProgramInputUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int Index { get; set; }
        public int Source { get; set; }
    }

    protected override void CompareCommandProperties(ProgramInputUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.MixEffectId, Is.EqualTo(expectedData.Index), 
                   $"MixEffectId should match expected value for test case {testCase.Name}");
        Assert.That(actualCommand.Source, Is.EqualTo(expectedData.Source), 
                   $"Source should match expected value for test case {testCase.Name}");
    }

    [Test]
    public void ApplyToState_WithValidMixEffect_UpdatesProgramInput()
    {
        // Arrange
        const int mixEffectId = 1;
        const int newSource = 2000;
        
        var state = CreateStateWithMixEffect(mixEffectId, 1000); // Initial source
        var command = new ProgramInputUpdateCommand
        {
            MixEffectId = mixEffectId,
            Source = newSource
        };

        // Act
        var changedPaths = command.ApplyToState(state);

        // Assert
        Assert.That(state.Video!.MixEffects[mixEffectId]!.ProgramInput, Is.EqualTo(newSource));
        Assert.That(changedPaths, Has.Length.EqualTo(1));
        Assert.That(changedPaths[0], Is.EqualTo($"video.mixEffects.{mixEffectId}.programInput"));
    }

    [Test]
    public void ApplyToState_WithoutMixEffect_CreatesAndUpdates()
    {
        // Arrange
        const int mixEffectId = 1;
        const int newSource = 2000;
        
        var state = new AtemState
        {
            Info = new DeviceInfo
            {
                Capabilities = new AtemCapabilities
                {
                    MixEffects = 4 // Allow mix effect 1
                }
            }
        };
        
        var command = new ProgramInputUpdateCommand
        {
            MixEffectId = mixEffectId,
            Source = newSource
        };

        // Act
        var changedPaths = command.ApplyToState(state);

        // Assert
        Assert.That(state.Video, Is.Not.Null);
        Assert.That(state.Video!.MixEffects, Has.Length.GreaterThan(mixEffectId));
        Assert.That(state.Video.MixEffects[mixEffectId], Is.Not.Null);
        Assert.That(state.Video.MixEffects[mixEffectId]!.ProgramInput, Is.EqualTo(newSource));
        Assert.That(changedPaths, Has.Length.EqualTo(1));
        Assert.That(changedPaths[0], Is.EqualTo($"video.mixEffects.{mixEffectId}.programInput"));
    }

    [Test]
    public void ApplyToState_WithInvalidMixEffectId_ThrowsInvalidIdError()
    {
        // Arrange
        const int invalidMixEffectId = 10;
        
        var state = new AtemState
        {
            Info = new DeviceInfo
            {
                Capabilities = new AtemCapabilities
                {
                    MixEffects = 2 // Only 2 mix effects available (0 and 1)
                }
            }
        };
        
        var command = new ProgramInputUpdateCommand
        {
            MixEffectId = invalidMixEffectId,
            Source = 1000
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidIdError>(() => command.ApplyToState(state));
        Assert.That(exception!.Message, Does.Contain("MixEffect"));
        Assert.That(exception.Message, Does.Contain(invalidMixEffectId.ToString()));
    }

    [Test]
    public void ApplyToState_WithNullCapabilities_ThrowsInvalidIdError()
    {
        // Arrange
        var state = new AtemState(); // No capabilities set
        
        var command = new ProgramInputUpdateCommand
        {
            MixEffectId = 0,
            Source = 1000
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidIdError>(() => command.ApplyToState(state));
        Assert.That(exception!.Message, Does.Contain("MixEffect"));
    }

    /// <summary>
    /// Creates an AtemState with a valid mix effect at the specified index
    /// </summary>
    private static AtemState CreateStateWithMixEffect(int mixEffectId, int programInput = 0)
    {
        var mixEffects = new MixEffect?[Math.Max(mixEffectId + 1, 2)];
        mixEffects[mixEffectId] = new MixEffect
        {
            Index = mixEffectId,
            ProgramInput = programInput,
            PreviewInput = 1000,
            TransitionPreview = false,
            TransitionPosition = new TransitionPosition
            {
                InTransition = false,
                HandlePosition = 0,
                RemainingFrames = 0
            },
            TransitionProperties = new TransitionProperties(),
            TransitionSettings = new TransitionSettings(),
            UpstreamKeyers = []
        };

        return new AtemState
        {
            Info = new DeviceInfo
            {
                Capabilities = new AtemCapabilities
                {
                    MixEffects = Math.Max(mixEffectId + 1, 2)
                }
            },
            Video = new VideoState
            {
                MixEffects = mixEffects
            }
        };
    }
}