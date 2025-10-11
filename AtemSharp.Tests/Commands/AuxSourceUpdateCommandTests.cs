using AtemSharp.Commands;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands;

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
        var changedPaths = command.ApplyToState(state);

        // Assert
        Assert.That(state.Video!.Auxiliaries[auxId], Is.EqualTo(newSource));
        Assert.That(changedPaths, Has.Length.EqualTo(1));
        Assert.That(changedPaths[0], Is.EqualTo($"video.auxiliaries.{auxId}"));
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
        var changedPaths = command.ApplyToState(state);

        // Assert
        Assert.That(state.Video, Is.Not.Null, "Video state should be created");
        Assert.That(state.Video!.Auxiliaries, Has.Length.GreaterThan(auxId));
        Assert.That(state.Video.Auxiliaries[auxId], Is.EqualTo(newSource));
        Assert.That(changedPaths[0], Is.EqualTo($"video.auxiliaries.{auxId}"));
    }

    [Test]
    public void ApplyToState_WithSmallAuxiliariesArray_ExpandsArray()
    {
        // Arrange
        const int auxId = 5;
        const int newSource = 3000;
        
        var state = new AtemState
        {
            Video = new VideoState
            {
                Auxiliaries = new int?[2] // Smaller than auxId
            },
            Info = new DeviceInfo
            {
                Capabilities = new AtemCapabilities
                {
                    Auxiliaries = 10 // More than auxId
                }
            }
        };
        
        var command = new AuxSourceUpdateCommand
        {
            AuxBus = auxId,
            Source = newSource
        };

        // Act
        var changedPaths = command.ApplyToState(state);

        // Assert
        Assert.That(state.Video.Auxiliaries, Has.Length.GreaterThan(auxId));
        Assert.That(state.Video.Auxiliaries[auxId], Is.EqualTo(newSource));
        Assert.That(changedPaths[0], Is.EqualTo($"video.auxiliaries.{auxId}"));
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

    [Test]
    public void Deserialize_ValidBytes_CreatesCorrectCommand()
    {
        // Arrange
        var bytes = new byte[] { 0x03, 0x00, 0x05, 0xDC }; // auxId = 3, source = 1500
        using var stream = new MemoryStream(bytes);

        // Act
        var command = AuxSourceUpdateCommand.Deserialize(stream, ProtocolVersion.V8_0);

        // Assert
        Assert.That(command.AuxBus, Is.EqualTo(3));
        Assert.That(command.Source, Is.EqualTo(1500));
    }

    /// <summary>
    /// Creates an AtemState with a valid auxiliary output at the specified index
    /// </summary>
    private static AtemState CreateStateWithAuxiliary(int auxId, int source = 0)
    {
        var auxiliaries = new int?[Math.Max(auxId + 1, 2)];
        auxiliaries[auxId] = source;

        return new AtemState
        {
            Video = new VideoState
            {
                Auxiliaries = auxiliaries
            },
            Info = new DeviceInfo
            {
                Capabilities = new AtemCapabilities
                {
                    Auxiliaries = auxiliaries.Length
                }
            }
        };
    }
}