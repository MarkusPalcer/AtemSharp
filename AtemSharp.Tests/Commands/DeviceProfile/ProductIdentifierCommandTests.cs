using AtemSharp.Commands.DeviceProfile;
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
        Assert.That(actualCommand.Model, Is.EqualTo(expectedData.Model));
        Assert.That(actualCommand.ProductIdentifier, Is.EqualTo(expectedData.Name));
    }
}
