using AtemSharp.State.Ports;
using AtemSharp.State.Video.InputChannel;

namespace AtemSharp.Commands.Inputs;

/// <summary>
/// Command to update input channel properties
/// </summary>
[Command("CInL")]
[BufferSize(32)]
public partial class InputPropertiesCommand(InputChannel inputChannel) : SerializedCommand
{
    [SerializedField(2)] [NoProperty] private readonly ushort _inputId = inputChannel.InputId;

    /// <summary>
    /// Long descriptive name for the input (max 20 characters)
    /// </summary>
    [CustomSerialization(0)] private string _longName = inputChannel.LongName;

    /// <summary>
    /// Short name for the input (max 4 characters)
    /// </summary>
    [CustomSerialization(1)] private string _shortName = inputChannel.ShortName;

    /// <summary>
    /// External port type for the input
    /// </summary>
    [SerializedField(28, 2)] private ExternalPortType _externalPortType = inputChannel.ExternalPortType;

    private void SerializeInternal(byte[] buffer)
    {
        buffer.WriteString(_longName, 4, 20);
        buffer.WriteString(_shortName, 24, 4);
    }
}
