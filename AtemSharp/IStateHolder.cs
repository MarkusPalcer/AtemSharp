using AtemSharp.State;
using AtemSharp.State.Macro;

namespace AtemSharp;

public interface IStateHolder
{
    AtemState State { get; }

    MacroSystem Macros { get; }
}
