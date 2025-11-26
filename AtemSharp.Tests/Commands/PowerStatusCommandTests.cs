using AtemSharp.Commands;
using AtemSharp.State;
using AtemSharp.State.Info;

namespace AtemSharp.Tests.Commands;

[TestFixture]
public class PowerStatusCommandTests {

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
        command.ApplyToState(state);

        // Assert - Only the first power supply should be updated
        Assert.That(state.Info.Power, Has.Length.EqualTo(1));
        Assert.That(state.Info.Power[0], Is.True);
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
        command.ApplyToState(state);

        // Assert - Both power supplies should be updated
        Assert.That(state.Info.Power, Has.Length.EqualTo(2));
        Assert.That(state.Info.Power[0], Is.True);
        Assert.That(state.Info.Power[1], Is.False);
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
        command.ApplyToState(state);

        // Assert - Power array should remain empty
        Assert.That(state.Info.Power, Has.Length.EqualTo(0));
    }

    [Test]
    public void TestDeserialization_BothPowerSuppliesOn()
    {
        Span<byte> buffer = [0b00000011]; // Binary: 11 (both bits set)

        // Act - Deserialize the command
        var command = PowerStatusCommand.Deserialize(buffer, ProtocolVersion.V7_2);

        // Assert - Both power supplies should be on
        Assert.That(command.PowerSupplies, Has.Length.EqualTo(2));
        Assert.That(command.PowerSupplies[0], Is.True);  // Bit 0
        Assert.That(command.PowerSupplies[1], Is.True);  // Bit 1
    }

    [Test]
    public void TestDeserialization_FirstPowerSupplyOnly()
    {
        Span<byte> buffer = [0b00000001];

        // Act - Deserialize the command
        var command = PowerStatusCommand.Deserialize(buffer, ProtocolVersion.V7_2);

        // Assert - Only first power supply should be on
        Assert.That(command.PowerSupplies, Has.Length.EqualTo(2));
        Assert.That(command.PowerSupplies[0], Is.True);   // Bit 0
        Assert.That(command.PowerSupplies[1], Is.False);  // Bit 1
    }

    [Test]
    public void TestDeserialization_SecondPowerSupplyOnly()
    {
        Span<byte> buffer = [0b00000010];

        // Act - Deserialize the command
        var command = PowerStatusCommand.Deserialize(buffer, protocolVersion: ProtocolVersion.V7_2);

        // Assert - Only second power supply should be on
        Assert.That(command.PowerSupplies, Has.Length.EqualTo(2));
        Assert.That(command.PowerSupplies[0], Is.False);  // Bit 0
        Assert.That(command.PowerSupplies[1], Is.True);   // Bit 1
    }

    [Test]
    public void TestDeserialization_NoPowerSuppliesOn()
    {
        Span<byte> buffer = [0b00000000];

        // Act - Deserialize the command
        var command = PowerStatusCommand.Deserialize(buffer, ProtocolVersion.V7_2);

        // Assert - No power supplies should be on
        Assert.That(command.PowerSupplies, Has.Length.EqualTo(2));
        Assert.That(command.PowerSupplies[0], Is.False);  // Bit 0
        Assert.That(command.PowerSupplies[1], Is.False);  // Bit 1
    }

    [Test]
    public void TestDeserialization_IgnoreHigherBits()
    {
        Span<byte> buffer = [0b11111101]; // Binary: 11111101 (bits 2-7 set, bit 1 unset, bit 0 set)

        // Act - Deserialize the command
        var command = PowerStatusCommand.Deserialize(buffer, ProtocolVersion.V7_2);

        // Assert - Only bits 0 and 1 should be considered
        Assert.That(command.PowerSupplies, Has.Length.EqualTo(2));
        Assert.That(command.PowerSupplies[0], Is.True);   // Bit 0 is set
        Assert.That(command.PowerSupplies[1], Is.False);  // Bit 1 is not set
    }

}
