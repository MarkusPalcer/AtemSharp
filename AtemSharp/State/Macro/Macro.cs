using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using AtemSharp.Commands.Macro;
using AtemSharp.Lib;

namespace AtemSharp.State.Macro;

/// <summary>
/// Represents a macro in the ATEM switcher
/// </summary>
[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
[DebuggerDisplay("{" + nameof(ToString) + ",nq}")]
public partial class Macro(IAtemSwitcher switcher)
{
    /// <summary>
    /// Gets the id of the macro
    /// </summary>
    public ushort Id { get; internal set; }

    /// <summary>
    /// Gets/Sets the name
    /// </summary>
    private string _name = string.Empty;

    /// <summary>
    /// Gets/Sets the additional description
    /// </summary>
    private string _description = string.Empty;

    /// <summary>
    /// Gets whether this macro slot contains an actual macro
    /// </summary>
    [ReadOnly(true)] private bool _isUsed;

    /// <summary>
    /// Gets whether this macro uses unsupported operations
    /// </summary>
    [ReadOnly(true)] private bool _hasUnsupportedOps;

    [ExcludeFromCodeCoverage]
    public override string ToString() => $"{GetType().Name} #{Id} ({Name})";

    /// <summary>
    /// Runs this macro
    /// </summary>
    /// <exception cref="InvalidOperationException">When the macro is not used</exception>
    public async Task Run()
    {
        if (!IsUsed)
        {
            throw new InvalidOperationException("Cannot run macros that are unused");
        }

        await switcher.SendCommandAsync(MacroActionCommand.Run(this));
    }

    /// <summary>
    /// Records a macro into this slot
    /// </summary>
    /// <param name="name">The name of the new macro</param>
    /// <param name="description">The description of the new macro</param>
    public async Task Record(string name, string description)
    {
        await switcher.SendCommandAsync(new MacroRecordCommand(this) { Name = name, Description = description });
    }

    /// <summary>
    /// Empties this macro slot
    /// </summary>
    public async Task Delete()
    {
        await switcher.SendCommandAsync(MacroActionCommand.Delete(this));
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
