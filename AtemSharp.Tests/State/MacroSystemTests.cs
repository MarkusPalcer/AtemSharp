using AtemSharp.Commands.Macro;
using AtemSharp.State.Macro;
using NSubstitute;

namespace AtemSharp.Tests.State;

[TestFixture]
public class MacroSystemTests
{
    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public void ChangeIsLooping(bool newValue)
    {
        var switcher = Substitute.For<IAtemSwitcher>();
        var sut = new MacroSystem(switcher);
        sut.Populate(5);

        sut.UpdatePlayLooped(!newValue);
        sut.PlayLooped = newValue;

        switcher.Received(1).SendCommandAsync(Arg.Is<MacroRunStatusCommand>(x => x.Loop == newValue));
    }

    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public void NoChangeIsLooping(bool newValue)
    {
        var switcher = Substitute.For<IAtemSwitcher>();
        var sut = new MacroSystem(switcher);
        sut.Populate(5);

        sut.UpdatePlayLooped(newValue);
        sut.PlayLooped = newValue;

        switcher.DidNotReceiveWithAnyArgs().SendCommandAsync(null!);
    }
}
