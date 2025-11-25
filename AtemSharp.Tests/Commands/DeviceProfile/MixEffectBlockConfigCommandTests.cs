using AtemSharp.Commands.DeviceProfile;
using AtemSharp.State;
using AtemSharp.State.Info;

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
        var state = new AtemState
        {
            Info =
            {
                MixEffects = AtemStateUtil.CreateArray<MixEffectInfo>(2)
            }
        };

        var command = new MixEffectBlockConfigCommand
        {
            Index = 1,
            KeyCount = 4
        };

        // Act
        command.ApplyToState(state);

        // Assert
        Assert.That(state.Info.MixEffects[1], Is.Not.Null);
        Assert.That(state.Info.MixEffects[1].KeyCount, Is.EqualTo(4));
    }

    [Test]
    public void ApplyToState_MultipleMixEffects_ShouldHandleMultipleIndices()
    {
        // Arrange
        var state = new AtemState
        {
            Info =
            {
                MixEffects = AtemStateUtil.CreateArray<MixEffectInfo>(2)
            }
        };

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
        command1.ApplyToState(state);
        command2.ApplyToState(state);

        // Assert
        Assert.That(state.Info.MixEffects[0].KeyCount, Is.EqualTo(2));
        Assert.That(state.Info.MixEffects[1].KeyCount, Is.EqualTo(4));
    }

    [Test]
    public void ApplyToState_ReplacingExistingMixEffect_ShouldReplaceInfo()
    {
        // Arrange
        var state = new AtemState
        {
            Info =
            {
                MixEffects = AtemStateUtil.CreateArray<MixEffectInfo>(2)
            }
        };

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
        secondCommand.ApplyToState(state);

        // Assert - should have the values from the second command
        Assert.That(state.Info.MixEffects, Is.Not.Null);
        Assert.That(state.Info.MixEffects[1].KeyCount, Is.EqualTo(8));
    }
}
