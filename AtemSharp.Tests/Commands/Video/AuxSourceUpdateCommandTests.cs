using AtemSharp.Commands.Video;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.Video;

[TestFixture]
public class AuxSourceUpdateCommandTests : DeserializedCommandTestBase<AuxSourceUpdateCommand,
    AuxSourceUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int Id { get; set; }
        public int Source { get; set; }
    }

    protected override void CompareCommandProperties(AuxSourceUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.AuxBus, Is.EqualTo(expectedData.Id),
                   $"AuxBus should match expected value for test case {testCase.Name}");
        Assert.That(actualCommand.Source, Is.EqualTo(expectedData.Source),
                   $"Source should match expected value for test case {testCase.Name}");
    }

    [Test]
    public void ApplyToState_WithValidAuxiliary_UpdatesSource()
    {
        // Arrange
        const int auxId = 1;
        const int newSource = 2000;

        var state = CreateStateWithAuxiliary(auxId, 1000); // Initial source
        var command = new AuxSourceUpdateCommand
        {
            AuxBus = auxId,
            Source = newSource
        };

        // Act
        command.ApplyToState(state);

        // Assert
        Assert.That(state.Video.Auxiliaries[auxId], Is.EqualTo(newSource));
    }

    [Test]
    public void ApplyToState_WithoutAuxiliary_CreatesAndUpdates()
    {
        // Arrange
        const int auxId = 3;
        const int newSource = 1500;

        var state = new AtemState
        {
            Info = new DeviceInfo
            {
                Capabilities = new AtemCapabilities
                {
                    Auxiliaries = 5 // More than auxId
                }
            }
        };

        var command = new AuxSourceUpdateCommand
        {
            AuxBus = auxId,
            Source = newSource
        };

        // Act
        command.ApplyToState(state);

        // Assert
        Assert.That(state.Video, Is.Not.Null, "Video state should be created");
        Assert.That(state.Video.Auxiliaries[auxId], Is.EqualTo(newSource));
    }

    [Test]
    public void ApplyToState_WithInvalidAuxId_ThrowsInvalidIdError()
    {
        // Arrange
        const int auxId = 10;
        var state = new AtemState
        {
            Info = new DeviceInfo
            {
                Capabilities = new AtemCapabilities
                {
                    Auxiliaries = 5 // Less than auxId
                }
            }
        };

        var command = new AuxSourceUpdateCommand
        {
            AuxBus = auxId,
            Source = 1000
        };

        // Act & Assert
        Assert.Throws<InvalidIdError>(() => command.ApplyToState(state));
    }


    /// <summary>
    /// Creates an AtemState with a valid auxiliary output at the specified index
    /// </summary>
    private static AtemState CreateStateWithAuxiliary(byte auxId, ushort source = 0)
    {
        return new AtemState
        {
            Video = new VideoState
            {
                Auxiliaries = new Dictionary<byte, ushort> { {auxId, source} },
            },
            Info = new DeviceInfo
            {
                Capabilities = new AtemCapabilities
                {
                    Auxiliaries = auxId+1
                }
            }
        };
    }

    [Test]
    public void ApplyToState_ValidIndex_ShouldSucceed()
    {
        // Arrange
        var state = new AtemState();
        state.Info.Capabilities = new AtemCapabilities
        {
            Auxiliaries = 4 // 0-3 valid
        };

        var command = new AuxSourceUpdateCommand
        {
            AuxBus = 2,
            Source = 3000
        };

        // Act & Assert
        Assert.DoesNotThrow(() => command.ApplyToState(state));
        Assert.That(state.Video.Auxiliaries[2], Is.EqualTo(3000));
    }

    [Test]
    public void ApplyToState_InvalidIndex_ShouldThrowInvalidIdError()
    {
        // Arrange
        var state = new AtemState();
        state.Info.Capabilities = new AtemCapabilities
        {
            Auxiliaries = 4 // 0-3 valid
        };

        var command = new AuxSourceUpdateCommand
        {
            AuxBus = 6, // Invalid - only 0-3 are valid
            Source = 3000
        };

        // Act & Assert
        var ex = Assert.Throws<InvalidIdError>(() => command.ApplyToState(state));
        Assert.That(ex.Message, Contains.Substring("Auxiliary"));
        Assert.That(ex.Message, Contains.Substring("6"));
    }

    [Test]
    public void ApplyToState_NullCapabilities_ShouldThrowInvalidIdError()
    {
        // Arrange
        var state = new AtemState();
        state.Info.Capabilities = null;

        var command = new AuxSourceUpdateCommand
        {
            AuxBus = 0,
            Source = 3000
        };

        // Act & Assert
        var ex = Assert.Throws<InvalidIdError>(() => command.ApplyToState(state));
        Assert.That(ex.Message, Contains.Substring("Auxiliary"));
    }
}
