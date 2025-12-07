using AtemSharp.Attributes;
using AtemSharp.Commands;
using AtemSharp.State;

namespace AtemSharp.Tests.TestUtilities.TestCommands;

[Command("BROK")]
public class BrokenCommand1 : IDeserializedCommand
{
    public void ApplyToState(AtemState state)
    {
        throw new InvalidOperationException();
    }
}
