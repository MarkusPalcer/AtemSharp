using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Ports;

/// <summary>
/// Internal port types
/// </summary>
[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum InternalPortType : byte
{
    External = 0,
    Black = 1,
    ColorBars = 2,
    ColorGenerator = 3,
    MediaPlayerFill = 4,
    MediaPlayerKey = 5,
    SuperSource = 6,
    // Since V8_1_1
    ExternalDirect = 7,

    MEOutput = 128,
    Auxiliary = 129,
    Mask = 130,
    // Since V8_1_1
    MultiViewer = 131,
}
