using AtemSharp.Commands.DeviceProfile;
using AtemSharp.State;
using AtemSharp.State.Info;

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
        command.ApplyToState(state);

        // Assert
        Assert.That(state.Info.ProductIdentifier, Is.EqualTo("bbdf2a89-c79d-4b72-9460-423b6478e60ba"));
        Assert.That(state.Info.Model, Is.EqualTo(Model.TwoMEBS4K));
        Assert.That(state.Info.Power, Is.EqualTo(new[] { false, false })); // TwoMEBS4K has 2 power supplies
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
        command.ApplyToState(state);

        // Assert
        Assert.That(state.Info.ProductIdentifier, Is.EqualTo("test-device"));
        Assert.That(state.Info.Model, Is.EqualTo(Model.OneME));
        Assert.That(state.Info.Power, Is.EqualTo(new[] { false })); // OneME has 1 power supply
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
        command.ApplyToState(state);

        // Assert
        Assert.That(state.Info.ProductIdentifier, Is.EqualTo("constellation-device"));
        Assert.That(state.Info.Model, Is.EqualTo(Model.Constellation));
        Assert.That(state.Info.Power, Is.EqualTo(new[] { false, false })); // Constellation has 2 power supplies
    }
}
