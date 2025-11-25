namespace AtemSharp.State.Video.MixEffect.Transition;

/// <summary>
/// Transition selection flags for mix effect block transitions
/// </summary>
[Flags]
public enum TransitionSelection : byte
{
    /// <summary>
    /// Background video source
    /// </summary>
    Background = 1 << 0,

    /// <summary>
    /// Upstream key 1
    /// </summary>
    Key1 = 1 << 1,

    /// <summary>
    /// Upstream key 2
    /// </summary>
    Key2 = 1 << 2,

    /// <summary>
    /// Upstream key 3
    /// </summary>
    Key3 = 1 << 3,

    /// <summary>
    /// Upstream key 4
    /// </summary>
    Key4 = 1 << 4
}
