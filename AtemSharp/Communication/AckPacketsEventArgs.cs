namespace AtemSharp.Communication;

public class AckPacketsEventArgs : EventArgs
{
    public required int[] PacketIds { get; init; }
}
