using AtemSharp.Commands;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects;

/// <summary>
/// Cut command for performing an immediate cut transition
/// </summary>
public class CutCommand : BasicWritableCommand<object?>
{
    public static new string RawName { get; } = "DCut";

    public int MixEffect { get; }

    public CutCommand(int mixEffect) : base(null)
    {
        MixEffect = mixEffect;
    }

    public override byte[] Serialize(ProtocolVersion version)
    {
        var buffer = new byte[4];
        buffer[0] = (byte)MixEffect;
        return buffer;
    }
}