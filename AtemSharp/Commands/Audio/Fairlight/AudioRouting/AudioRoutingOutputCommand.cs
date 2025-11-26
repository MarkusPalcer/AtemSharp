using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Audio.Fairlight.AudioRouting;

[Command("AROC")]
[BufferSize(76)]
public partial class AudioRoutingOutputCommand(AudioRoutingEntry output) : SerializedCommand
{
    [SerializedField(4)]
    private uint _id = output.Id;

    [SerializedField(8, 0)]
    private uint _sourceId;

    [CustomSerialization(1)]
    private string _name = output.Name;

    private void SerializeInternal(byte[] buffer)
    {
        buffer.WriteString(_name, 12, 64);
    }
}
