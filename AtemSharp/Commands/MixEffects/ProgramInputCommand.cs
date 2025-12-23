using AtemSharp.State.Video.MixEffect;

namespace AtemSharp.Commands.MixEffects;

/// <summary>
/// Used to set the program input source for a mix effect
/// </summary>
[Command("CPgI")]
[BufferSize(4)]
public partial class ProgramInputCommand(MixEffect mixEffect) : SerializedCommand
{
    [SerializedField(0)] [NoProperty] private readonly byte _mixEffectId = mixEffect.Id;

    /// <summary>
    /// Program input source number
    /// </summary>
    [SerializedField(2)] private ushort _source = mixEffect.ProgramInput;
}
