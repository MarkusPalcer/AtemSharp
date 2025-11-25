using AtemSharp.Lib;
using AtemSharp.State.Info;
using AtemSharp.State.Video.MixEffect;

namespace AtemSharp.Commands.MixEffects.Transition;

/// <summary>
/// Command to set the transition mix rate for a mix effect
/// </summary>
[Command("CTMx")]
[BufferSize(4)]
public class TransitionMixCommand(MixEffect mixEffect) : SerializedCommand
{
    internal readonly int MixEffectId = mixEffect.Id;

    private int _rate = mixEffect.TransitionSettings.Mix.Rate;


    /// <summary>
    /// The rate of the mix transition in frames
    /// </summary>
    public int Rate
    {
        get => _rate;
        set
        {
            _rate = value;
            Flag |= 1 << 0; // Automatic flag setting!
        }
    }

    /// <summary>
    /// Serialize command to binary stream for transmission to ATEM
    /// </summary>
    /// <param name="version">Protocol version</param>
    /// <returns>Serialized command data</returns>
    public override byte[] Serialize(ProtocolVersion version)
    {
        using var memoryStream = new MemoryStream(4);
        using var writer = new BinaryWriter(memoryStream);

        writer.Write((byte)MixEffectId);
        writer.Write((byte)Rate);
        writer.Pad(2); // Skip 2 bytes padding

        return memoryStream.ToArray();
    }
}
