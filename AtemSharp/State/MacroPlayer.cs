namespace AtemSharp.State;

public class MacroPlayer
{
    public ushort MacroIndex { get; set; }
    public bool IsRunning { get; set; }
    public bool IsLooping { get; set; }
    public bool IsWaiting { get; set; }
}
