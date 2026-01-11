using AtemSharp.Batch;
using AtemSharp.Types;

namespace AtemSharp.State.Macro;

public partial class MacroSystem : ItemCollection<ushort, Macro>
{
    internal MacroSystem(IBatchLike switcher) : base(id => new Macro(switcher) { Id = id })
    {
        Player = new MacroPlayer(switcher);
        Recorder = new MacroRecorder(switcher);
    }

    public MacroPlayer Player { get; }

    public MacroRecorder Recorder { get; }

    private void CopyToInternal(MacroSystem target)
    {
        foreach (var macro in this)
        {
            macro.CopyTo(target.GetOrCreate(macro.Id));
        }

        target.Player.UpdateCurrentlyPlaying(Player.CurrentlyPlaying is null ? null : this[Player.CurrentlyPlaying.Id]);
        target.Recorder.UpdateCurrentlyRecording(Recorder.CurrentlyRecording is null ? null : this[Recorder.CurrentlyRecording.Id]);
    }
}
