using AtemSharp.Helpers;
using AtemSharp.Lib;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Fairlight.AudioRouting;

[Command("ARSC")]
[BufferSize(72)]
public partial class AudioRoutingSourceCommand(AudioRoutingEntry output) : SerializedCommand
{
    [SerializedField(4)]
    private uint _id = output.Id;

    [CustomSerialization(0)]
    private string _name = output.Name;

    private void SerializeInternal(byte[] buffer)
    {
        buffer.WriteString(_name, 8, 64);
    }
}
