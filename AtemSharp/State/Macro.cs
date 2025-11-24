namespace AtemSharp.State;

public class Macro
{
    public ushort Id { get; internal set; }
    public string Name { get; internal set; } = string.Empty;
    public string Description { get; internal set; } = string.Empty;
    public bool IsUsed { get; internal set; }
    public bool HasUnsupportedOps { get; internal set; }
}