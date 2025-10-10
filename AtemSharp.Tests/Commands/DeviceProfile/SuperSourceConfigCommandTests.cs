using AtemSharp.Commands.DeviceProfile;
using AtemSharp.Enums;
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
        if (actualCommand.SsrcId != expectedData.SsrcId)
        {
            failures.Add($"SsrcId: expected {expectedData.SsrcId}, actual {actualCommand.SsrcId}");
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
            SsrcId = 1,
            BoxCount = 54
        };

        // Act
        var result = command.ApplyToState(state);

        // Assert
        Assert.That(state.Info.SuperSources, Is.Not.Null);
        Assert.That(state.Info.SuperSources.ContainsKey(1), Is.True);
        Assert.That(state.Info.SuperSources[1], Is.Not.Null);
        Assert.That(state.Info.SuperSources[1].BoxCount, Is.EqualTo(54));
        
        Assert.That(result, Is.EqualTo(new[] { "info.superSources" }));
    }

    [Test]
    public void ApplyToState_WithDifferentSsrcIds_ShouldUpdateCorrectSuperSource()
    {
        // Arrange
        var state = new AtemState();
        var command1 = new SuperSourceConfigCommand
        {
            SsrcId = 0,
            BoxCount = 48
        };
        var command2 = new SuperSourceConfigCommand
        {
            SsrcId = 2,
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

    [Test]
    public void Deserialize_WithV8OrLater_ShouldDeserializeSsrcIdAndBoxCount()
    {
        // Arrange - Based on test data: "01-00-8B-00" -> SsrcId: 1, BoxCount: 139
        var data = new byte[] { 0x01, 0x00, 0x8B, 0x00 };
        using var stream = new MemoryStream(data);

        // Act
        var command = SuperSourceConfigCommand.Deserialize(stream, ProtocolVersion.V8_0);

        // Assert
        Assert.That(command.SsrcId, Is.EqualTo(1));
        Assert.That(command.BoxCount, Is.EqualTo(139));
    }

    [Test]
    public void Deserialize_WithV7_ShouldUseZeroSsrcIdAndReadBoxCount()
    {
        // Arrange - Based on test data for older versions: "36-00-00-00" -> BoxCount: 54
        var data = new byte[] { 0x36, 0x00, 0x00, 0x00 };
        using var stream = new MemoryStream(data);

        // Act
        var command = SuperSourceConfigCommand.Deserialize(stream, ProtocolVersion.V7_2);

        // Assert
        Assert.That(command.SsrcId, Is.EqualTo(0));
        Assert.That(command.BoxCount, Is.EqualTo(54));
    }

    [Test]
    public void Deserialize_WithTypicalValues_ShouldDeserializeCorrectly()
    {
        // Arrange - Based on test data: "00-00-16-00" -> SsrcId: 0, BoxCount: 22
        var data = new byte[] { 0x00, 0x00, 0x16, 0x00 };
        using var stream = new MemoryStream(data);

        // Act
        var command = SuperSourceConfigCommand.Deserialize(stream, ProtocolVersion.V8_0);

        // Assert
        Assert.That(command.SsrcId, Is.EqualTo(0));
        Assert.That(command.BoxCount, Is.EqualTo(22));
    }

    [Test]
    public void Deserialize_WithZeroBoxCount_ShouldDeserializeCorrectly()
    {
        // Arrange
        var data = new byte[] { 0x00, 0x00, 0x00, 0x00 };
        using var stream = new MemoryStream(data);

        // Act
        var command = SuperSourceConfigCommand.Deserialize(stream, ProtocolVersion.V8_0);

        // Assert
        Assert.That(command.SsrcId, Is.EqualTo(0));
        Assert.That(command.BoxCount, Is.EqualTo(0));
    }
}