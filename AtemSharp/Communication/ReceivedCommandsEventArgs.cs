using AtemSharp.Commands;

namespace AtemSharp.Communication;

public class ReceivedCommandsEventArgs : EventArgs
{
    public required IDeserializedCommand[] Commands { get; init; }
}