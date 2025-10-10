using AtemSharp.Commands.DeviceProfile;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.DeviceProfile;

[TestFixture]
public class MixEffectBlockConfigCommandTests : DeserializedCommandTestBase<MixEffectBlockConfigCommand, MixEffectBlockConfigCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public byte KeyCount { get; set; }
    }

    protected override void CompareCommandProperties(MixEffectBlockConfigCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        var failures = new List<string>();

        // Compare Index
        if (actualCommand.Index != expectedData.Index)
        {
            failures.Add($"Index: expected {expectedData.Index}, actual {actualCommand.Index}");
        }

        // Compare KeyCount
        if (actualCommand.KeyCount != expectedData.KeyCount)
        {
            failures.Add($"KeyCount: expected {expectedData.KeyCount}, actual {actualCommand.KeyCount}");
        }

        if (failures.Count > 0)
        {
            Assert.Fail($"Command property comparison failed:\n{string.Join("\n", failures)}");
        }
    }

    [Test]
    public void ApplyToState_WithValidData_ShouldUpdateMixEffectInfo()
    {
        // Arrange
        var state = new AtemState();
        var command = new MixEffectBlockConfigCommand
        {
            Index = 1,
            KeyCount = 4
        };

        // Act
        var result = command.ApplyToState(state);

        // Assert
        Assert.That(state.Info.MixEffects, Is.Not.Null);
        Assert.That(state.Info.MixEffects.ContainsKey(1), Is.True);
        Assert.That(state.Info.MixEffects[1], Is.Not.Null);
        Assert.That(state.Info.MixEffects[1].KeyCount, Is.EqualTo(4));
        
        Assert.That(result, Is.EqualTo(new[] { "info.mixEffects.1" }));
    }

    [Test]
    public void Deserialize_WithTypicalValues_ShouldDeserializeCorrectly()
    {
        // Arrange - from test data: Index 3, KeyCount 22
        var data = new byte[] { 0x03, 0x16 };
        using var stream = new MemoryStream(data);

        // Act
        var command = MixEffectBlockConfigCommand.Deserialize(stream, ProtocolVersion.V7_2);

        // Assert
        Assert.That(command.Index, Is.EqualTo(3));
        Assert.That(command.KeyCount, Is.EqualTo(22));
    }

    [Test]
    public void Deserialize_WithZeroValues_ShouldDeserializeCorrectly()
    {
        // Arrange
        var data = new byte[] { 0x00, 0x00 };
        using var stream = new MemoryStream(data);

        // Act
        var command = MixEffectBlockConfigCommand.Deserialize(stream, ProtocolVersion.V7_2);

        // Assert
        Assert.That(command.Index, Is.EqualTo(0));
        Assert.That(command.KeyCount, Is.EqualTo(0));
    }

    [Test]
    public void Deserialize_WithMaxValues_ShouldDeserializeCorrectly()
    {
        // Arrange
        var data = new byte[] { 0xFF, 0xFF };
        using var stream = new MemoryStream(data);

        // Act
        var command = MixEffectBlockConfigCommand.Deserialize(stream, ProtocolVersion.V7_2);

        // Assert
        Assert.That(command.Index, Is.EqualTo(255));
        Assert.That(command.KeyCount, Is.EqualTo(255));
    }

    [Test]
    public void Deserialize_WithRealWorldValues_ShouldDeserializeCorrectly()
    {
        // Arrange - from test data: Index 0, KeyCount 106
        var data = new byte[] { 0x00, 0x6A };
        using var stream = new MemoryStream(data);

        // Act
        var command = MixEffectBlockConfigCommand.Deserialize(stream, ProtocolVersion.V7_2);

        // Assert
        Assert.That(command.Index, Is.EqualTo(0));
        Assert.That(command.KeyCount, Is.EqualTo(106));
    }

    [Test]
    public void ApplyToState_MultipleMixEffects_ShouldHandleMultipleIndices()
    {
        // Arrange
        var state = new AtemState();
        
        var command1 = new MixEffectBlockConfigCommand
        {
            Index = 0,
            KeyCount = 2
        };
        
        var command2 = new MixEffectBlockConfigCommand
        {
            Index = 1,
            KeyCount = 4
        };

        // Act
        var result1 = command1.ApplyToState(state);
        var result2 = command2.ApplyToState(state);

        // Assert
        Assert.That(state.Info.MixEffects, Is.Not.Null);
        Assert.That(state.Info.MixEffects.Count, Is.EqualTo(2));
        
        Assert.That(state.Info.MixEffects.ContainsKey(0), Is.True);
        Assert.That(state.Info.MixEffects[0].KeyCount, Is.EqualTo(2));
        
        Assert.That(state.Info.MixEffects.ContainsKey(1), Is.True);
        Assert.That(state.Info.MixEffects[1].KeyCount, Is.EqualTo(4));
        
        Assert.That(result1, Is.EqualTo(new[] { "info.mixEffects.0" }));
        Assert.That(result2, Is.EqualTo(new[] { "info.mixEffects.1" }));
    }

    [Test]
    public void ApplyToState_ReplacingExistingMixEffect_ShouldReplaceInfo()
    {
        // Arrange
        var state = new AtemState();
        
        var firstCommand = new MixEffectBlockConfigCommand
        {
            Index = 1,
            KeyCount = 2
        };
        
        var secondCommand = new MixEffectBlockConfigCommand
        {
            Index = 1,
            KeyCount = 8
        };

        // Act
        firstCommand.ApplyToState(state);
        var result = secondCommand.ApplyToState(state);

        // Assert - should have the values from the second command
        Assert.That(state.Info.MixEffects, Is.Not.Null);
        Assert.That(state.Info.MixEffects.ContainsKey(1), Is.True);
        Assert.That(state.Info.MixEffects[1].KeyCount, Is.EqualTo(8));
        
        Assert.That(result, Is.EqualTo(new[] { "info.mixEffects.1" }));
    }
}