using System.ComponentModel;
using AtemSharp.Batch;
using AtemSharp.Commands.Macro;
using AtemSharp.Extensions;

namespace AtemSharp.State.Macro;

/// <summary>
/// The recording part of the macro system
/// </summary>
public partial class MacroRecorder(IBatchLike switcher)
{
    /// <summary>
    /// Gets the macro which is currently being recorded or <c>null</c> if no recording is in progress
    /// </summary>
    [ReadOnly(true)] private Macro? _currentlyRecording;

    /// <summary>
    /// Stops recording a macro
    /// </summary>
    public async Task StopRecording()
    {
        await switcher.SendCommandAsync(MacroActionCommand.StopRecord());
    }

    /// <summary>
    /// Adds a timed pause to the macro
    /// </summary>
    /// <param name="frameCount">The pause time in frames</param>
    /// <exception cref="InvalidOperationException">When no macro is being recorded</exception>
    public async Task AddPause(ushort frameCount)
    {
        if (_currentlyRecording is null)
        {
            throw new InvalidOperationException("Can only add pause while recording.");
        }

        await switcher.SendCommandAsync(new MacroAddTimedPauseCommand { Frames = frameCount });
    }

    /// <summary>
    /// Adds a timed pause to the macro according to the current video mode
    /// </summary>
    /// <param name="duration">The timespan to wait. It will be converted to frames according to the currently set video mode</param>
    public async Task AddPause(TimeSpan duration)
    {
        await AddPause((ushort)(duration.TotalSeconds * switcher.State.Settings.VideoMode.FramesPerSecond()));
    }

    /// <summary>
    /// Adds a pause to the macro that prompts the user to tell it to continue
    /// </summary>
    public async Task AddWaitForUser()
    {
        if (_currentlyRecording is null)
        {
            throw new InvalidOperationException("Can only add pause while recording.");
        }

        await switcher.SendCommandAsync(MacroActionCommand.InsertUserWait());
    }
}
