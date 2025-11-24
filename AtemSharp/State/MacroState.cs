namespace AtemSharp.State;

public class MacroState
{
    public MacroPlayer Player { get; } = new();

    public MacroRecorder Recorder { get; } = new();

    public Macro[] Macros { get; internal set; } = [];
}