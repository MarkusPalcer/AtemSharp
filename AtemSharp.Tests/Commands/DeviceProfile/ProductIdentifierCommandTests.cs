using AtemSharp.Commands.DeviceProfile;
using AtemSharp.State;
using AtemSharp.State.Info;

namespace AtemSharp.Tests.Commands.DeviceProfile;

[TestFixture]
internal class ProductIdentifierCommandTests : DeserializedCommandTestBase<ProductIdentifierCommand,
    ProductIdentifierCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public string Name { get; set; } = string.Empty;
        public Model Model { get; set; }
    }

    internal override void CompareCommandProperties(ProductIdentifierCommand actualCommand, CommandData expectedData)
    {
        Assert.That(actualCommand.Model, Is.EqualTo(expectedData.Model));
        Assert.That(actualCommand.ProductIdentifier, Is.EqualTo(expectedData.Name));
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        Assert.That(state.Info.Model, Is.EqualTo(expectedData.Model));
        Assert.That(state.Info.ProductIdentifier, Is.EqualTo(expectedData.Name));
    }

    [Test]
    [TestCase(Model.TwoME)]
    [TestCase(Model.TwoME4K)]
    [TestCase(Model.TwoMEBS4K)]
    [TestCase(Model.Constellation)]
    [TestCase(Model.Constellation8K)]
    [TestCase(Model.ConstellationHD4ME)]
    [TestCase(Model.Constellation4K4ME)]
    public void TwoPowerSupplies(Model model)
    {
        var sut = new ProductIdentifierCommand
        {
            Model = model
        };

        var state = new AtemState();

        sut.ApplyToState(state);

        Assert.That(state.Info.Power, Is.EquivalentTo((bool[])[false, false]));
    }

    [Test]
    [TestCase(Model.Unknown)]
    [TestCase(Model.TVS)]
    [TestCase(Model.OneME)]
    [TestCase(Model.PS4K)]
    [TestCase(Model.OneME4K)]
    [TestCase(Model.TVSHD)]
    [TestCase(Model.TVSProHD)]
    [TestCase(Model.TVSPro4K)]
    [TestCase(Model.Mini)]
    [TestCase(Model.MiniPro)]
    [TestCase(Model.MiniProISO)]
    [TestCase(Model.MiniExtreme)]
    [TestCase(Model.MiniExtremeISO)]
    [TestCase(Model.ConstellationHD1ME)]
    [TestCase(Model.ConstellationHD2ME)]
    [TestCase(Model.SDI)]
    [TestCase(Model.SDIProISO)]
    [TestCase(Model.SDIExtremeISO)]
    [TestCase(Model.TelevisionStudioHD8)]
    [TestCase(Model.TelevisionStudioHD8ISO)]
    [TestCase(Model.Constellation4K1ME)]
    [TestCase(Model.Constellation4K2ME)]
    [TestCase(Model.TelevisionStudio4K8)]
    public void OnePowerSupply(Model model)
    {
        var sut = new ProductIdentifierCommand
        {
            Model = model
        };

        var state = new AtemState();

        sut.ApplyToState(state);

        Assert.That(state.Info.Power, Is.EquivalentTo((bool[])[false]));
    }
}
