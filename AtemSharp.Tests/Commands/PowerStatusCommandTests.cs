using AtemSharp.Commands;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands;

[TestFixture]
// Test data contains incorrect properties data, so we implement manual tests
public class PowerStatusCommandTests
{

    [Test]
    public void TestApplyToState_SinglePowerSupply()
    {
        // Arrange - Create a command with both power supplies data
        var command = new PowerStatusCommand
        {
            PowerSupplies = [true, false]
        };

        // Create state with single power supply device (like most models)
        var state = new AtemState();
        state.Info.Power = [false]; // Single power supply, initially off

        // Act - Apply the command to state
        var changedPaths = command.ApplyToState(state);

        // Assert - Only the first power supply should be updated
        Assert.That(state.Info.Power, Has.Length.EqualTo(1));
        Assert.That(state.Info.Power[0], Is.True);
        Assert.That(changedPaths, Is.EqualTo(new[] { "info.power" }));
    }

    [Test]
    public void TestApplyToState_DualPowerSupply()
    {
        // Arrange - Create a command with both power supplies data
        var command = new PowerStatusCommand
        {
            PowerSupplies = [true, false]
        };

        // Create state with dual power supply device (like 2ME models)
        var state = new AtemState();
        state.Info.Power = [false, false]; // Two power supplies, both initially off

        // Act - Apply the command to state
        var changedPaths = command.ApplyToState(state);

        // Assert - Both power supplies should be updated
        Assert.That(state.Info.Power, Has.Length.EqualTo(2));
        Assert.That(state.Info.Power[0], Is.True);
        Assert.That(state.Info.Power[1], Is.False);
        Assert.That(changedPaths, Is.EqualTo(new[] { "info.power" }));
    }

    [Test]
    public void TestApplyToState_EmptyPowerArray()
    {
        // Arrange - Create a command with power supply data
        var command = new PowerStatusCommand
        {
            PowerSupplies = [true, false]
        };

        // Create state with no power supplies configured (edge case)
        var state = new AtemState();
        state.Info.Power = []; // No power supplies configured

        // Act - Apply the command to state
        var changedPaths = command.ApplyToState(state);

        // Assert - Power array should remain empty
        Assert.That(state.Info.Power, Has.Length.EqualTo(0));
        Assert.That(changedPaths, Is.EqualTo(new[] { "info.power" }));
    }

    [Test]
    public void TestDeserialization_BothPowerSuppliesOn()
    {
        // Arrange - Create stream with both power supplies on (bits 0 and 1 set)
        using var stream = new MemoryStream([0b00000011]); // Binary: 11 (both bits set)

        // Act - Deserialize the command
        var command = PowerStatusCommand.Deserialize(stream, ProtocolVersion.V7_2);

        // Assert - Both power supplies should be on
        Assert.That(command.PowerSupplies, Has.Length.EqualTo(2));
        Assert.That(command.PowerSupplies[0], Is.True);  // Bit 0
        Assert.That(command.PowerSupplies[1], Is.True);  // Bit 1
    }

    [Test]
    public void TestDeserialization_FirstPowerSupplyOnly()
    {
        // Arrange - Create stream with only first power supply on (bit 0 set)
        using var stream = new MemoryStream([0b00000001]); // Binary: 01 (only bit 0 set)

        // Act - Deserialize the command
        var command = PowerStatusCommand.Deserialize(stream, ProtocolVersion.V7_2);

        // Assert - Only first power supply should be on
        Assert.That(command.PowerSupplies, Has.Length.EqualTo(2));
        Assert.That(command.PowerSupplies[0], Is.True);   // Bit 0
        Assert.That(command.PowerSupplies[1], Is.False);  // Bit 1
    }

    [Test]
    public void TestDeserialization_SecondPowerSupplyOnly()
    {
        // Arrange - Create stream with only second power supply on (bit 1 set)
        using var stream = new MemoryStream([0b00000010]); // Binary: 10 (only bit 1 set)

        // Act - Deserialize the command
        var command = PowerStatusCommand.Deserialize(stream, ProtocolVersion.V7_2);

        // Assert - Only second power supply should be on
        Assert.That(command.PowerSupplies, Has.Length.EqualTo(2));
        Assert.That(command.PowerSupplies[0], Is.False);  // Bit 0
        Assert.That(command.PowerSupplies[1], Is.True);   // Bit 1
    }

    [Test]
    public void TestDeserialization_NoPowerSuppliesOn()
    {
        // Arrange - Create stream with no power supplies on (no bits set)
        using var stream = new MemoryStream([0b00000000]); // Binary: 00 (no bits set)

        // Act - Deserialize the command
        var command = PowerStatusCommand.Deserialize(stream, ProtocolVersion.V7_2);

        // Assert - No power supplies should be on
        Assert.That(command.PowerSupplies, Has.Length.EqualTo(2));
        Assert.That(command.PowerSupplies[0], Is.False);  // Bit 0
        Assert.That(command.PowerSupplies[1], Is.False);  // Bit 1
    }

    [Test]
    public void TestDeserialization_IgnoreHigherBits()
    {
        // Arrange - Create stream with higher bits set (should be ignored)
        using var stream = new MemoryStream([0b11111101]); // Binary: 11111101 (bits 2-7 set, bit 1 unset, bit 0 set)

        // Act - Deserialize the command
        var command = PowerStatusCommand.Deserialize(stream, ProtocolVersion.V7_2);

        // Assert - Only bits 0 and 1 should be considered
        Assert.That(command.PowerSupplies, Has.Length.EqualTo(2));
        Assert.That(command.PowerSupplies[0], Is.True);   // Bit 0 is set
        Assert.That(command.PowerSupplies[1], Is.False);  // Bit 1 is not set
    }
}