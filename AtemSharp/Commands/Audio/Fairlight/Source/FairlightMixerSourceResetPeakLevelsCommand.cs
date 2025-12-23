namespace AtemSharp.Commands.Audio.Fairlight.Source;

/// <summary>
/// Used to reset the peaks for a fairlight mixer source
/// </summary>
[Command("RFIP")]
[BufferSize(20)]
public partial class FairlightMixerSourceResetPeakLevelsCommand(State.Audio.Fairlight.Source source) : SerializedCommand
{
    [SerializedField(0)] [NoProperty] private readonly ushort _inputId = source.InputId;

    [SerializedField(8)] [NoProperty] private readonly long _sourceId = source.Id;

    public bool Output { get; set; }
    public bool DynamicsInput { get; set; }
    public bool DynamicsOutput { get; set; }

    private void SerializeInternal(byte[] buffer)
    {
        byte val = 0;
        if (DynamicsInput)
        {
            val |= 1 << 0;
        }

        if (DynamicsOutput)
        {
            val |= 1 << 1;
        }

        if (Output)
        {
            val |= 1 << 2;
        }

        buffer.WriteUInt8(val, 17);
    }
}
