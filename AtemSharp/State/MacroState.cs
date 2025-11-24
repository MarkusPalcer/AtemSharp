namespace AtemSharp.State;

public class MacroState
{
    public MacroPlayer Player { get; } = new();

    public MacroRecorder Recorder { get; } = new();

    public Macro[] Macros { get; internal set; } = [];
}

public class Macro
{
    public ushort Id { get; internal set; }
    public string Name { get; internal set; } = string.Empty;
    public string Description { get; internal set; } = string.Empty;
    public bool IsUsed { get; internal set; }
    public bool HasUnsupportedOps { get; internal set; }
}
