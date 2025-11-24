using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Key;

[Command("RACK")]
[BufferSize(4)]
public partial class MixEffectKeyAdvancedChromaSampleResetCommand(UpstreamKeyer keyer) :SerializedCommand
{
    [SerializedField(0)][NoProperty] private byte _mixEffectId = keyer.MixEffectId;
    [SerializedField(1)] [NoProperty] private byte _id = keyer.Id;
    [CustomSerialization] private bool _resetKeyAdjustments;
    [CustomSerialization] private bool _resetChromaCorrection;
    [CustomSerialization] private bool _resetColorAdjustments;

    private void SerializeInternal(byte[] buffer)
    {
        byte val = 0;
        if (_resetKeyAdjustments) val |= 1 << 0;
        if (_resetChromaCorrection) val |= 1 << 1;
        if (_resetColorAdjustments) val |= 1 << 2;
        buffer.WriteUInt8(val, 3);
    }
}
