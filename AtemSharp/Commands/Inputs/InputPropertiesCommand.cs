using AtemSharp.Enums.Ports;
using AtemSharp.Helpers;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.Inputs;

/// <summary>
/// Command to update input channel properties
/// </summary>
[Command("CInL")]
[BufferSize(32)]
public partial class InputPropertiesCommand : SerializedCommand
{
    [SerializedField(2)]
    private ushort _inputId;

    /// <summary>
    /// Long descriptive name for the input (max 20 characters)
    /// </summary>
    [CustomSerialization(0)]
    private string _longName;

    /// <summary>
    /// Short name for the input (max 4 characters)
    /// </summary>
    [CustomSerialization(1)]
    private string _shortName;

    /// <summary>
    /// External port type for the input
    /// </summary>
    [SerializedField(28, 2)]
    private ExternalPortType _externalPortType;

    public InputPropertiesCommand(InputChannel inputChannel)
    {
        InputId = inputChannel.InputId;
        _longName = inputChannel.LongName;
        _shortName = inputChannel.ShortName;
        _externalPortType = inputChannel.ExternalPortType;
    }

    private void SerializeInternal(byte[] buffer)
    {
        buffer.WriteString(_longName, 4, 20);
        buffer.WriteString(_shortName, 24, 4);
    }
}
