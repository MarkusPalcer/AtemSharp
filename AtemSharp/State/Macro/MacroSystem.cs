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
}
