using System.ComponentModel;
using AtemSharp.Commands.Macro;
using AtemSharp.Lib;

namespace AtemSharp.State.Macro;

/// <summary>
/// The playback engine of the macro system
/// </summary>
public partial class MacroPlayer(IAtemSwitcher switcher)
{
    /// <summary>
    /// Gets/sets whether the macro player should continuously repeat a playing macro until told to stop
    /// </summary>
    private bool _playLooped;

    /// <summary>
    /// Gets whether the currently playing macro waits for the user to tell it to continue
    /// </summary>
    [ReadOnly(true)] private bool _playbackIsWaitingForUserAction;

    /// <summary>
    /// Gets the currently playing macro or <c>null</c> if none is playing
    /// </summary>
    [ReadOnly(true)] private Macro? _currentlyPlaying;

    private partial void SendPlayLoopedUpdateCommand(bool value)
    {
        switcher.SendCommandAsync(new MacroRunStatusCommand { Loop = value }).FireAndForget();
    }

    /// <summary>
    /// Stops playing the current macro
    /// </summary>
    public async Task StopPlayback()
    {
        await switcher.SendCommandAsync(MacroActionCommand.Stop());
    }

    /// <summary>
    /// Tells the currently running macro to continue playing when it is waiting for the user to tell it so
    /// </summary>
    public async Task Continue()
    {
        await switcher.SendCommandAsync(MacroActionCommand.Continue());
    }
}
