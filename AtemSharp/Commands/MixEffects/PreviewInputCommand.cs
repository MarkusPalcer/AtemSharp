using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects;

/// <summary>
/// Command to set the preview input source for a mix effect
/// </summary>
[Command("CPvI")]
[BufferSize(4)]
public partial class PreviewInputCommand(MixEffect mixEffect) : SerializedCommand
{
    [SerializedField(0)] [NoProperty] internal readonly byte MixEffectId = mixEffect.Id;

    [SerializedField(2)] private ushort _source = mixEffect.PreviewInput;
}
