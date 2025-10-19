using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.Macro;

[Command("MRPr")]
public class MacroRunStatusUpdateCommand : IDeserializedCommand
{
    public static IDeserializedCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        var status = rawCommand.ReadUInt8(0);
        return new MacroRunStatusUpdateCommand()
        {
            IsRunning = (status & 1 << 0) > 0,
            IsWaiting = (status & 1 << 1) > 0,
            Loop = rawCommand.ReadBoolean(1),
            MacroIndex = rawCommand.ReadUInt16BigEndian(2)
        };
    }

    public ushort MacroIndex { get; internal set; }

    public bool Loop { get; internal set; }

    public bool IsWaiting { get; internal set; }

    public bool IsRunning { get; internal set; }

    public void ApplyToState(AtemState state)
    {
        state.Macros.Player.MacroIndex = MacroIndex;
        state.Macros.Player.IsRunning = IsRunning;
        state.Macros.Player.IsLooping = Loop;
        state.Macros.Player.IsWaiting = IsWaiting;
    }
}
