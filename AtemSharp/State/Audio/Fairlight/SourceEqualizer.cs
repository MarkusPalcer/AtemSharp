using System.Diagnostics.CodeAnalysis;
using AtemSharp.Types;

namespace AtemSharp.State.Audio.Fairlight;

public class SourceEqualizer : Equalizer
{
    internal SourceEqualizer(Source source)
    {
        Bands = new ItemCollection<byte, SourceEqualizerBand>(id => new SourceEqualizerBand
        {
            Id = id,
            InputId = source.InputId,
            SourceId = source.Id
        });
    }

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public ItemCollection<byte, SourceEqualizerBand> Bands { get; }
}
