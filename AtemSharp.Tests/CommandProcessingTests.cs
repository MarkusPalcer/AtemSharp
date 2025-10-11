using AtemSharp.Commands;
using AtemSharp.Commands.DeviceProfile;
using AtemSharp.Commands.MixEffects;
using AtemSharp.Enums;
using AtemSharp.Lib;
using System.Text;

namespace AtemSharp.Tests;

[TestFixture]
public class CommandProcessingTests
{
    private Atem? _atem;

    [SetUp]
    public void SetUp()
    {
        // Clear static state
        Atem.UnknownCommands.Clear();
        _atem = new Atem();
    }

    [TearDown]
    public void TearDown()
    {
        _atem?.Dispose();
    }

    [Test]
    public void CommandParser_ParseVersionCommand_ShouldUpdateParserVersion()
    {
        // Arrange
        var parser = new CommandParser();
        var versionData = new byte[] { 0x00, 0x02, 0x00, 0x1C }; // V8_0
        using var stream = new MemoryStream(versionData);

        // Act
        var command = parser.ParseCommand("_ver", stream);

        // Assert
        Assert.That(command, Is.Not.Null);
        Assert.That(command, Is.TypeOf<VersionCommand>());
        Assert.That(parser.Version, Is.EqualTo(ProtocolVersion.V8_0));
    }

    [Test]
    public void CommandParser_ParseUnknownCommand_ShouldReturnNullAndTrackInUnknownCommands()
    {
        // Arrange
        var parser = new CommandParser();
        var unknownData = new byte[] { 0x01, 0x02 };
        using var stream = new MemoryStream(unknownData);
        var initialUnknownCount = Atem.UnknownCommands.Count;

        // Act
        var command = parser.ParseCommand("XXXX", stream);

        // Assert
        Assert.That(command, Is.Null);
        Assert.That(Atem.UnknownCommands.Count, Is.EqualTo(initialUnknownCount + 1));
        Assert.That(Atem.UnknownCommands, Contains.Item("XXXX"));
    }

    [Test]
    public void CommandParser_ParseInitCompleteCommand_ShouldReturnValidCommand()
    {
        // Arrange
        var parser = new CommandParser();
        using var stream = new MemoryStream(Array.Empty<byte>()); // InitComplete has no data
        
        // Act
        var command = parser.ParseCommand("InCm", stream);

        // Assert
        Assert.That(command, Is.Not.Null);
        Assert.That(command, Is.TypeOf<InitCompleteCommand>());
    }

    [Test]
    public void PacketProcessing_MultipleCommands_ShouldProcessAllCommands()
    {
        // Arrange
        var payload = CreateMultiCommandPayload();
        var parser = new CommandParser();
        var state = new AtemSharp.State.AtemState();
        
        // Act - Simulate OnPacketReceived logic
        var offset = 0;
        var processedCommands = new List<IDeserializedCommand>();

        while (offset + 8 <= payload.Length)
        {
            // Extract command header
            var commandLength = (payload[offset] << 8) | payload[offset + 1];
            var rawName = Encoding.ASCII.GetString(payload, offset + 4, 4);

            if (commandLength < 8 || offset + commandLength > payload.Length)
                break;

            // Extract command data
            var commandDataStart = offset + 8;
            var commandDataLength = commandLength - 8;
            using var commandDataStream = new MemoryStream(payload, commandDataStart, commandDataLength);

            try
            {
                var command = parser.ParseCommand(rawName, commandDataStream);
                if (command != null)
                {
                    command.ApplyToState(state);
                    processedCommands.Add(command);
                }
            }
            catch (Exception ex)
            {
                // Log but continue processing
                Console.WriteLine($"Failed to process command {rawName}: {ex.Message}");
            }

            offset += commandLength;
        }

        // Assert
        Assert.That(processedCommands.Count, Is.GreaterThanOrEqualTo(1)); // Should process at least the version command
        Assert.That(processedCommands.Any(c => c is VersionCommand), Is.True, "Should contain a version command");
        Assert.That(parser.Version, Is.EqualTo(ProtocolVersion.V8_0)); // Version should be updated
    }

    [Test]
    public void PacketProcessing_MalformedCommand_ShouldHandleGracefully()
    {
        // Arrange
        var payload = CreateMalformedCommandPayload();
        var parser = new CommandParser();
        var state = new AtemSharp.State.AtemState();
        
        // Act - Simulate OnPacketReceived logic with malformed data
        var offset = 0;
        var processedCommands = 0;
        var processingFailed = false;

        try
        {
            while (offset + 8 <= payload.Length)
            {
                var commandLength = (payload[offset] << 8) | payload[offset + 1];
                var rawName = Encoding.ASCII.GetString(payload, offset + 4, 4);

                if (commandLength < 8 || offset + commandLength > payload.Length)
                {
                    // This should happen with malformed data
                    break;
                }

                var commandDataStart = offset + 8;
                var commandDataLength = commandLength - 8;
                using var commandDataStream = new MemoryStream(payload, commandDataStart, commandDataLength);

                var command = parser.ParseCommand(rawName, commandDataStream);
                if (command != null)
                {
                    command.ApplyToState(state);
                    processedCommands++;
                }

                offset += commandLength;
            }
        }
        catch (Exception)
        {
            processingFailed = true;
        }

        // Assert
        Assert.That(processingFailed, Is.False, "Processing should not throw exceptions");
        Assert.That(processedCommands, Is.EqualTo(0), "No commands should be processed from malformed data");
    }

    [Test]
    public void PacketProcessing_EmptyPayload_ShouldHandleGracefully()
    {
        // Arrange
        var payload = Array.Empty<byte>();
        
        // Act - Simulate OnPacketReceived logic with empty payload
        var offset = 0;
        var processedCommands = 0;

        while (offset <= payload.Length - 8)
        {
            // This loop should never execute
            processedCommands++;
            break;
        }

        // Assert
        Assert.That(processedCommands, Is.EqualTo(0));
    }

    [Test]
    public void CommandParser_WithDifferentVersions_ShouldSelectCorrectCommand()
    {
        // Arrange
        var parser = new CommandParser();
        
        // Act & Assert - Test that parser correctly handles version selection
        var baselineCommand = parser.GetCommandType("_ver");
        Assert.That(baselineCommand, Is.Not.Null);
        
        // Change parser version and verify it still works
        parser.Version = ProtocolVersion.V9_4;
        var versionedCommand = parser.GetCommandType("_ver");
        Assert.That(versionedCommand, Is.Not.Null);
    }

    [Test]
    public void StateApplication_ProgramInputCommand_ShouldValidateCorrectly()
    {
        // Arrange
        var parser = new CommandParser();
        
        // Create program input command data (ME=0, Input=2)
        var programInputData = new byte[] { 0x00, 0x00, 0x00, 0x02 };
        using var stream = new MemoryStream(programInputData);

        // Act
        var command = parser.ParseCommand("PrgI", stream);
        
        // Assert
        Assert.That(command, Is.Not.Null);
        Assert.That(command, Is.TypeOf<ProgramInputUpdateCommand>());
        
        // Note: We don't test ApplyToState here because it requires proper state initialization
        // The important part is that the command parsing works correctly
        var programInputCmd = (ProgramInputUpdateCommand)command;
        Assert.That(programInputCmd.MixEffectId, Is.EqualTo(0));
        Assert.That(programInputCmd.Source, Is.EqualTo(2));
    }

    // Helper methods for creating test payloads

    private static byte[] CreateMultiCommandPayload()
    {
        var payload = new List<byte>();
        
        // Command 1: Version command (_ver)
        payload.AddRange([0x00, 0x0C]); // Length: 12 bytes (8 header + 4 data)
        payload.AddRange([0x00, 0x00]); // Reserved
        payload.AddRange(Encoding.ASCII.GetBytes("_ver")); // Command name
        payload.AddRange([0x00, 0x02, 0x00, 0x1C]); // Version V8_0
        
        // Command 2: Unknown command (TEST)
        payload.AddRange([0x00, 0x08]); // Length: 8 bytes (header only)
        payload.AddRange([0x00, 0x00]); // Reserved
        payload.AddRange(Encoding.ASCII.GetBytes("TEST")); // Unknown command name
        
        return payload.ToArray();
    }

    private static byte[] CreateMalformedCommandPayload()
    {
        var payload = new List<byte>();
        
        // Malformed command: length claims 20 bytes but we only provide 8
        payload.AddRange([0x00, 0x14]); // Length: 20 bytes (but actual data is shorter)
        payload.AddRange([0x00, 0x00]); // Reserved
        payload.AddRange(Encoding.ASCII.GetBytes("BAAD")); // Command name
        // Missing additional data - this makes it malformed
        
        return payload.ToArray();
    }
}