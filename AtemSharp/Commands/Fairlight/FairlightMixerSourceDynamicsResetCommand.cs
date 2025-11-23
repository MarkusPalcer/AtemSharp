using AtemSharp.Helpers;
using AtemSharp.Lib;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Fairlight;

[Command("RICD")]
[BufferSize(20)]
public partial class FairlightMixerSourceDynamicsResetCommand(Source source) : SerializedCommand
{
    [SerializedField(0)]
    [NoProperty]
    private readonly ushort _inputId = source.InputId;

    [SerializedField(8)]
    [NoProperty]
    private readonly long _sourceId = source.Id;

    public bool ResetDynamics { get; set; }
    public bool ResetExpander { get; set; }
    public bool ResetCompressor { get; set; }
    public bool ResetLimiter { get; set; }

    private void SerializeInternal(byte[] buffer)
    {
        byte val = 0;
        if (ResetDynamics) val |= 1 << 0;
        if (ResetExpander) val |= 1 << 1;
        if (ResetCompressor) val |= 1 << 2;
        if (ResetLimiter) val |= 1 << 3;

        buffer.WriteUInt8(val, 17);
    }
}
