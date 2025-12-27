using AtemSharp.State;
using AtemSharp.State.Macro;
using NSubstitute;

namespace AtemSharp.Tests.TestUtilities;

public class TestStateHolder : IStateHolder
{
    public TestStateHolder()
    {
        Switcher = Substitute.For<IAtemSwitcher>();
        Macros = new MacroSystem(Switcher);
    }

    public IAtemSwitcher Switcher { get; }

    public AtemState State { get; } = new();
    public MacroSystem Macros { get; }
}
