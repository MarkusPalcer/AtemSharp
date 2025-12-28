using System.ComponentModel;
using AtemSharp.Commands.Macro;
using AtemSharp.Extensions;

namespace AtemSharp.State.Macro;

public partial class MacroRecorder(IAtemSwitcher switcher)
{
    [ReadOnly(true)] private Macro? _currentlyRecording;

    public async Task StopRecording()
    {
        var macroToStop = _currentlyRecording;

        if (macroToStop is null)
        {
            return;
        }

        await switcher.SendCommandAsync(new MacroActionCommand(macroToStop, MacroAction.StopRecord));
    }

    public async Task AddPause(ushort frameCount)
    {
        if (_currentlyRecording is null)
        {
            throw new InvalidOperationException("Can only add pause while recording.");
        }

        await switcher.SendCommandAsync(new MacroAddTimedPauseCommand { Frames = frameCount });
    }

    public async Task AddPause(TimeSpan duration)
    {
        await AddPause((ushort)(duration.TotalSeconds * switcher.State.Settings.VideoMode.FramesPerSecond()));
    }
}
