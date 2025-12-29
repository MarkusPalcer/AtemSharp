using System.Diagnostics.CodeAnalysis;
using AtemSharp.State;

namespace AtemSharp.Commands.Macro;

[Command("MRPr")]
internal partial class MacroRunStatusUpdateCommand : IDeserializedCommand
{
    [DeserializedField(2)] private ushort _macroIndex;

    [DeserializedField(1)] private bool _loop;

    [CustomDeserialization] private bool _isWaiting;

    [CustomDeserialization] private bool _isRunning;

    private enum Status : byte
    {
        Running = 1,
        Waiting = 2,
    }

    private void DeserializeInternal(ReadOnlySpan<byte> rawCommand)
    {
        var status = (Status)rawCommand.ReadUInt8(0);
        IsRunning = status.HasFlag(Status.Running);
        IsWaiting = status.HasFlag(Status.Waiting);
    }

    /// <inheritdoc />
    public void Apply(IStateHolder switcher)
    {
        var macroSystem = switcher.Macros;
        macroSystem.Player.UpdatePlayLooped(Loop);
        macroSystem.Player.UpdateCurrentlyPlaying(_isRunning ? macroSystem[_macroIndex] : null);
        macroSystem.Player.UpdatePlaybackIsWaitingForUserAction(IsWaiting);
    }

    [ExcludeFromCodeCoverage(Justification = "Obsolete")]
    public void ApplyToState(AtemState state)
    {
        throw new NotImplementedException();
    }
}
