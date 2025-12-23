using AtemSharp.State;

namespace AtemSharp.Commands.Audio.Fairlight.Source;

[Command("FASD")]
internal partial class FairlightMixerSourceDeleteCommand : IDeserializedCommand
{
    [DeserializedField(0)] private ushort _inputId;

    [DeserializedField(8)] private long _sourceId;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        state.GetFairlight().Inputs.GetValueOrDefault(_inputId)?.Sources.Remove(_sourceId);
    }
}
