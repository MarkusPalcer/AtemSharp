using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Audio.Fairlight.AudioRouting;

/// <summary>
/// Command to set the source and name for a specific <see cref="AudioRoutingEntry"/>
/// </summary>
[Command("AROC")]
[BufferSize(76)]
public partial class AudioRoutingOutputCommand(AudioRoutingEntry entry) : SerializedCommand
{
    [SerializedField(4)]
    [NoProperty]
    private readonly uint _id = entry.Id;

    [SerializedField(8, 0)]
    private uint _sourceId;

    [CustomSerialization(1)]
    private string _name = entry.Name;

    private void SerializeInternal(byte[] buffer)
    {
        buffer.WriteString(_name, 12, 64);
    }
}
