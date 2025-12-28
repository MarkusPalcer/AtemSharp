using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using AtemSharp.Commands.Macro;
using AtemSharp.Lib;

namespace AtemSharp.State.Macro;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
[DebuggerDisplay("{" + nameof(ToString) + ",nq}")]
public partial class Macro(IAtemSwitcher switcher)
{
    public ushort Id { get; internal set; }

    private string _name = string.Empty;
    private string _description = string.Empty;
    [ReadOnly(true)] private bool _isUsed;
    [ReadOnly(true)] private bool _hasUnsupportedOps;

    [ExcludeFromCodeCoverage]
    public override string ToString() => $"{GetType().Name} #{Id} ({Name})";

    public async Task Run()
    {
        if (!IsUsed)
        {
            throw new InvalidOperationException("Cannot run macros that are unused");
        }

        await switcher.SendCommandAsync(new MacroActionCommand(this, MacroAction.Run));
    }

    public async Task Record(string name, string description)
    {
        await switcher.SendCommandAsync(new MacroRecordCommand(this) { Name = name, Description = description });
    }

    private partial void SendNameUpdateCommand(string value)
    {
        switcher.SendCommandAsync(new MacroPropertiesCommand(this) { Name = value }).FireAndForget();
    }

    private partial void SendDescriptionUpdateCommand(string value)
    {
        switcher.SendCommandAsync(new MacroPropertiesCommand(this) { Description = value }).FireAndForget();
    }
}
