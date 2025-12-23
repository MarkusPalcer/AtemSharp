using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Audio.Fairlight.AudioRouting;

/// <summary>
/// Command to set the name of an <see cref="AudioRoutingEntry"/>
/// </summary>
[Command("ARSC")]
[BufferSize(72)]
public partial class AudioRoutingSourceCommand(AudioRoutingEntry output) : SerializedCommand
{
    [SerializedField(4)]
    [NoProperty]
    private readonly uint _id = output.Id;

    [CustomSerialization(0)]
    private string _name = output.Name;

    private void SerializeInternal(byte[] buffer)
    {
        buffer.WriteString(_name, 8, 64);
    }
}
