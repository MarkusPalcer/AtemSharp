using System.Diagnostics.CodeAnalysis;
using AtemSharp.Types;

namespace AtemSharp.State.Audio.Fairlight;

public class MasterEqualizer : Equalizer
{
    internal MasterEqualizer()
    {
        Bands = new ItemCollection<byte, MasterEqualizerBand>(id => new MasterEqualizerBand { Id = id });
    }

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public ItemCollection<byte, MasterEqualizerBand> Bands { get; }
}
