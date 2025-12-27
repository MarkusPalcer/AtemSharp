using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using AtemSharp.Commands.Macro;

namespace AtemSharp.State.Macro;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
[DebuggerDisplay("{" + nameof(ToString) + ",nq}")]
public class Macro(IAtemSwitcher switcher)
{
    public ushort Id { get; internal set; }
    public string Name { get; internal set; } = string.Empty;
    public string Description { get; internal set; } = string.Empty;
    public bool IsUsed { get; internal set; }
    public bool HasUnsupportedOps { get; internal set; }

    [ExcludeFromCodeCoverage]
    public override string ToString() => $"{GetType().Name} #{Id}";

    public async Task Run()
    {
        await switcher.SendCommandAsync(new MacroActionCommand(this, MacroAction.Run));
    }
}
