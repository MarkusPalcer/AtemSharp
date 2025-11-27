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
    private readonly int _mixEffectId = mixEffect.Id;

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

    /// <inheritdoc />
    public override byte[] Serialize(ProtocolVersion version)
    {
        using var memoryStream = new MemoryStream(4);
        using var writer = new BinaryWriter(memoryStream);

        writer.Write((byte)_mixEffectId);
        writer.Write((byte)Rate);
        writer.Pad(2); // Skip 2 bytes padding

        return memoryStream.ToArray();
    }
}
