using System.ComponentModel;
using System.Runtime.Serialization;
using AtemSharp.Commands.Macro;
using AtemSharp.Lib;
using AtemSharp.Types;

namespace AtemSharp.State.Macro;

public partial class MacroSystem : ItemCollection<ushort, Macro>
{
    [IgnoreDataMember]
    private readonly IAtemSwitcher _switcher;

    internal MacroSystem(IAtemSwitcher switcher) : base(id => new Macro(switcher) { Id = id })
    {
        _switcher = switcher;
    }

    private bool _playLooped;

    [ReadOnly(true)] private bool _playbackIsWaiting;

    [ReadOnly(true)]
    private Macro? _currentlyPlaying;

    [ReadOnly(true)]
    private Macro? _currentlyRecording;

    private partial void SendPlayLoopedUpdateCommand(bool value)
    {
        _switcher.SendCommandAsync(new MacroRunStatusCommand { Loop = value }).FireAndForget();
    }
}

