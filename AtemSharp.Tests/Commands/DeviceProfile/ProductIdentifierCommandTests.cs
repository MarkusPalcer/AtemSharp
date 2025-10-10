using AtemSharp.Commands.DeviceProfile;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.DeviceProfile;

[TestFixture]
public class ProductIdentifierCommandTests : DeserializedCommandTestBase<ProductIdentifierCommand, ProductIdentifierCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public string Name { get; set; } = string.Empty;
        public Model Model { get; set; }
    }

    protected override void CompareCommandProperties(ProductIdentifierCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        var failures = new List<string>();

        // Compare ProductIdentifier (mapped from "Name" in test data)
        if (actualCommand.ProductIdentifier != expectedData.Name)
        {
            failures.Add($"ProductIdentifier: expected '{expectedData.Name}', actual '{actualCommand.ProductIdentifier}'");
        }

        // Compare Model
        if (actualCommand.Model != expectedData.Model)
        {
            failures.Add($"Model: expected {expectedData.Model}, actual {actualCommand.Model}");
        }

        if (failures.Count > 0)
        {
            Assert.Fail($"Command property comparison failed:\n{string.Join("\n", failures)}");
        }
    }

    [Test]
    public void ApplyToState_WithValidData_ShouldUpdateDeviceInfo()
    {
        // Arrange
        var state = new AtemState();
        var command = new ProductIdentifierCommand
        {
            ProductIdentifier = "bbdf2a89-c79d-4b72-9460-423b6478e60ba",
            Model = Model.TwoMEBS4K
        };

        // Act
        var result = command.ApplyToState(state);

        // Assert
        Assert.That(state.Info.ProductIdentifier, Is.EqualTo("bbdf2a89-c79d-4b72-9460-423b6478e60ba"));
        Assert.That(state.Info.Model, Is.EqualTo(Model.TwoMEBS4K));
        Assert.That(state.Info.Power, Is.EqualTo(new[] { false, false })); // TwoMEBS4K has 2 power supplies
        Assert.That(result, Is.EqualTo(new[] { "info" }));
    }

    [Test]
    public void ApplyToState_WithSinglePowerSupplyModel_ShouldSetSinglePowerSupply()
    {
        // Arrange
        var state = new AtemState();
        var command = new ProductIdentifierCommand
        {
            ProductIdentifier = "test-device",
            Model = Model.OneME // Single power supply model
        };

        // Act
        var result = command.ApplyToState(state);

        // Assert
        Assert.That(state.Info.ProductIdentifier, Is.EqualTo("test-device"));
        Assert.That(state.Info.Model, Is.EqualTo(Model.OneME));
        Assert.That(state.Info.Power, Is.EqualTo(new[] { false })); // OneME has 1 power supply
        Assert.That(result, Is.EqualTo(new[] { "info" }));
    }

    [Test]
    public void ApplyToState_WithDualPowerSupplyModel_ShouldSetDualPowerSupply()
    {
        // Arrange
        var state = new AtemState();
        var command = new ProductIdentifierCommand
        {
            ProductIdentifier = "constellation-device",
            Model = Model.Constellation // Dual power supply model
        };

        // Act
        var result = command.ApplyToState(state);

        // Assert
        Assert.That(state.Info.ProductIdentifier, Is.EqualTo("constellation-device"));
        Assert.That(state.Info.Model, Is.EqualTo(Model.Constellation));
        Assert.That(state.Info.Power, Is.EqualTo(new[] { false, false })); // Constellation has 2 power supplies
        Assert.That(result, Is.EqualTo(new[] { "info" }));
    }

    [Test]
    public void Deserialize_WithTypicalData_ShouldDeserializeCorrectly()
    {
        // Arrange - Test data based on libatem-data.json
        // "bbdf2a89-c79d-4b72-9460-423b6478e60ba" followed by null bytes and model 7 (TwoMEBS4K)
        var productIdentifierBytes = System.Text.Encoding.UTF8.GetBytes("bbdf2a89-c79d-4b72-9460-423b6478e60ba");
        var data = new byte[41]; // 40 bytes for product identifier + 1 byte for model
        Array.Copy(productIdentifierBytes, 0, data, 0, productIdentifierBytes.Length);
        data[40] = 0x07; // Model.TwoMEBS4K

        using var stream = new MemoryStream(data);

        // Act
        var command = ProductIdentifierCommand.Deserialize(stream, ProtocolVersion.V7_2);

        // Assert
        Assert.That(command.ProductIdentifier, Is.EqualTo("bbdf2a89-c79d-4b72-9460-423b6478e60ba"));
        Assert.That(command.Model, Is.EqualTo(Model.TwoMEBS4K));
    }

    [Test]
    public void Deserialize_WithShorterString_ShouldDeserializeCorrectly()
    {
        // Arrange - Test data with shorter product identifier
        var productIdentifierBytes = System.Text.Encoding.UTF8.GetBytes("short-id");
        var data = new byte[41]; // 40 bytes for product identifier + 1 byte for model
        Array.Copy(productIdentifierBytes, 0, data, 0, productIdentifierBytes.Length);
        // Remaining bytes stay 0 (null terminated)
        data[40] = 0x0B; // Model.Constellation

        using var stream = new MemoryStream(data);

        // Act
        var command = ProductIdentifierCommand.Deserialize(stream, ProtocolVersion.V7_2);

        // Assert
        Assert.That(command.ProductIdentifier, Is.EqualTo("short-id"));
        Assert.That(command.Model, Is.EqualTo(Model.Constellation));
    }

    [Test]
    public void Deserialize_WithEmptyString_ShouldDeserializeCorrectly()
    {
        // Arrange - Test data with empty product identifier
        var data = new byte[41]; // All zeros: empty string + null terminator + model 0
        data[40] = 0x00; // Model.Unknown

        using var stream = new MemoryStream(data);

        // Act
        var command = ProductIdentifierCommand.Deserialize(stream, ProtocolVersion.V7_2);

        // Assert
        Assert.That(command.ProductIdentifier, Is.EqualTo(string.Empty));
        Assert.That(command.Model, Is.EqualTo(Model.Unknown));
    }
}