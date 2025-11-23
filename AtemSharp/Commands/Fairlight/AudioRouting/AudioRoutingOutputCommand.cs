using AtemSharp.Helpers;
using AtemSharp.Lib;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Fairlight.AudioRouting;

[Command("AROC")]
[BufferSize(76)]
public partial class AudioRoutingOutputCommand(AudioRoutingOutput output) : SerializedCommand
{
    [SerializedField(4)]
    private uint _id = output.Id;

    [SerializedField(8)]
    private uint _sourceId;

    [SerializedField(0)]
    [CustomSerialization]
    private string _name = string.Empty;

    private void SerializeInternal(byte[] buffer)
    {
        buffer.WriteString(_name, 12, 64);
    }
}
