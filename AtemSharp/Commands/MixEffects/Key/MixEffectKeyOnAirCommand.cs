using AtemSharp.Helpers;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Key;

/// <summary>
/// Command to set the on-air state of an upstream keyer
/// </summary>
[Command("CKOn")]
[BufferSize(4)]
public partial class MixEffectKeyOnAirCommand(UpstreamKeyer keyer) : SerializedCommand
{
    [SerializedField(0)]
    [NoProperty]
    private readonly byte _mixEffectId = keyer.MixEffectId;

    [SerializedField(1)]
    [NoProperty]
    private readonly byte _keyerId = keyer.Id;

    /// <summary>
    /// Whether the upstream keyer is on air
    /// </summary>
    [SerializedField(2, 0)]
    private bool _onAir = keyer.OnAir;
}
