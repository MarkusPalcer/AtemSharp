namespace AtemSharp.State;

public class MacroState
{
    public MacroPlayer Player { get; } = new();

    public MacroRecorder Recorder { get; } = new();

    public Macro[] Macros { get; internal set; } = [];
}

public class Macro
{
    public ushort Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsUsed { get; set; }
    public bool HasUnsupportedOps { get; set; }
}
