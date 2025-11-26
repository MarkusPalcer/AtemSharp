namespace AtemSharp.Commands.StartupState;

[Command("SRsv")]
[BufferSize(4)]
public partial class StartupStateSaveCommand : SerializedCommand
{
    // Mode is always 0 for now according to TypeScript implementation
    [SerializedField(0)] [NoProperty] private readonly ushort _mode = 0;
}
