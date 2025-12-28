using System.ComponentModel;
using AtemSharp.Commands.Macro;
using AtemSharp.Lib;

namespace AtemSharp.State.Macro;

public partial class MacroPlayer(IAtemSwitcher switcher)
{
    private bool _playLooped;

    [ReadOnly(true)] private bool _playbackIsWaiting;
    [ReadOnly(true)] private Macro? _currentlyPlaying;


    private partial void SendPlayLoopedUpdateCommand(bool value)
    {
        switcher.SendCommandAsync(new MacroRunStatusCommand { Loop = value }).FireAndForget();
    }

    public async Task StopPlayback()
    {
        var macroToStop = _currentlyPlaying;

        if (macroToStop is null)
        {
            return;
        }

        await switcher.SendCommandAsync(new MacroActionCommand(macroToStop, MacroAction.Stop));
    }
}
