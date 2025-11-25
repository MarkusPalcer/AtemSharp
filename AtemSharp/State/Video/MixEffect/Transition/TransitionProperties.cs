namespace AtemSharp.State.Video.MixEffect.Transition;

/// <summary>
/// Transition properties for a mix effect block
/// </summary>
public class TransitionProperties
{
    /// <summary>
    /// If in a transition, this is the style of the running transition.
    /// If no transition is active it will mirror NextStyle
    /// </summary>
    public TransitionStyle Style { get; internal set; }

    /// <summary>
    /// If in a transition, this is the selection of the running transition.
    /// If no transition is active it will mirror NextSelection
    /// </summary>
    public TransitionSelection Selection { get; internal set; }

    /// <summary>
    /// The style for the next transition
    /// </summary>
    public TransitionStyle NextStyle { get; internal set; }

    /// <summary>
    /// The selection for the next transition
    /// </summary>
    public TransitionSelection NextSelection { get; internal set; }
}
