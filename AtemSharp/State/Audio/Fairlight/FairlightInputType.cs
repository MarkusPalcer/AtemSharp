using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Audio.Fairlight;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum FairlightInputType : byte
{
    EmbeddedWithVideo = 0,
    MediaPlayer = 1,
    AudioIn = 2,
    MADI = 4,
}
