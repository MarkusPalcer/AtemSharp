namespace AtemSharp.Commands.StartupState;

/// <summary>
/// Used to reset the startup state to defaults
/// </summary>
[Command("SRcl")]
[BufferSize(4)]
public partial class StartupStateClearCommand : SerializedCommand
{
    // Mode is always 0 for now according to TypeScript implementation
    [SerializedField(0)] [NoProperty] private readonly ushort _mode = 0;
}
