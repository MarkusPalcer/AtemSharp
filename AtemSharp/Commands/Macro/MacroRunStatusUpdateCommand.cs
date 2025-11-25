using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.Macro;

[Command("MRPr")]
public partial class MacroRunStatusUpdateCommand : IDeserializedCommand
{
    [DeserializedField(2)]
    private ushort _macroIndex;

    [DeserializedField(1)]
    private bool _loop;

    [CustomDeserialization]
    private bool _isWaiting;

    [CustomDeserialization]
    private bool _isRunning;

    private void DeserializeInternal(ReadOnlySpan<byte> rawCommand)
    {
        var status = rawCommand.ReadUInt8(0);
        IsRunning = (status & 1 << 0) > 0;
        IsWaiting = (status & 1 << 1) > 0;
    }

    public void ApplyToState(AtemState state)
    {
        state.Macros.Player.MacroIndex = MacroIndex;
        state.Macros.Player.IsRunning = IsRunning;
        state.Macros.Player.IsLooping = Loop;
        state.Macros.Player.IsWaiting = IsWaiting;
    }
}
