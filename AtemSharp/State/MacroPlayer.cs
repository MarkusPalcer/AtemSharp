namespace AtemSharp.State;

public class MacroPlayer
{
    public ushort MacroIndex { get; internal set; }
    public bool IsRunning { get; internal set; }
    public bool IsLooping { get; internal set; }
    public bool IsWaiting { get; internal set; }
}
