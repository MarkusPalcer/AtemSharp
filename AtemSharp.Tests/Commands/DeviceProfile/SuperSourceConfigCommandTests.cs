using AtemSharp.Commands.DeviceProfile;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.DeviceProfile;

[TestFixture]
public class SuperSourceConfigCommandTests : DeserializedCommandTestBase<SuperSourceConfigCommand, SuperSourceConfigCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int SsrcId { get; set; }
        public byte BoxCount { get; set; }

        // For older protocol versions that may use "Boxes" property in test data
        public byte Boxes
        {
            get => BoxCount;
            set => BoxCount = value;
        }
    }

    protected override void CompareCommandProperties(SuperSourceConfigCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        var failures = new List<string>();

        // Compare SsrcId
        if (actualCommand.SuperSourceId != expectedData.SsrcId)
        {
            failures.Add($"SsrcId: expected {expectedData.SsrcId}, actual {actualCommand.SuperSourceId}");
        }

        // Compare BoxCount
        if (actualCommand.BoxCount != expectedData.BoxCount)
        {
            failures.Add($"BoxCount: expected {expectedData.BoxCount}, actual {actualCommand.BoxCount}");
        }

        if (failures.Count > 0)
        {
            Assert.Fail($"Command property comparison failed:\n{string.Join("\n", failures)}");
        }
    }

    [Test]
    public void ApplyToState_WithValidData_ShouldUpdateSuperSourceInfo()
    {
        // Arrange
        var state = new AtemState();
        var command = new SuperSourceConfigCommand
        {
            SuperSourceId = 1,
            BoxCount = 54
        };

        // Act
        command.ApplyToState(state);

        // Assert
        Assert.That(state.Info.SuperSources, Is.Not.Null);
        Assert.That(state.Info.SuperSources.ContainsKey(1), Is.True);
        Assert.That(state.Info.SuperSources[1], Is.Not.Null);
        Assert.That(state.Info.SuperSources[1].BoxCount, Is.EqualTo(54));
    }

    [Test]
    public void ApplyToState_WithDifferentSsrcIds_ShouldUpdateCorrectSuperSource()
    {
        // Arrange
        var state = new AtemState();
        var command1 = new SuperSourceConfigCommand
        {
            SuperSourceId = 0,
            BoxCount = 48
        };
        var command2 = new SuperSourceConfigCommand
        {
            SuperSourceId = 2,
            BoxCount = 196
        };

        // Act
        command1.ApplyToState(state);
        command2.ApplyToState(state);

        // Assert
        Assert.That(state.Info.SuperSources.ContainsKey(0), Is.True);
        Assert.That(state.Info.SuperSources[0].BoxCount, Is.EqualTo(48));

        Assert.That(state.Info.SuperSources.ContainsKey(2), Is.True);
        Assert.That(state.Info.SuperSources[2].BoxCount, Is.EqualTo(196));
    }
}
