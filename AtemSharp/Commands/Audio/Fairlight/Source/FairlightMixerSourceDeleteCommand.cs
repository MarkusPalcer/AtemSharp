using AtemSharp.State;

namespace AtemSharp.Commands.Audio.Fairlight.Source;

[Command("FASD")]
public partial class FairlightMixerSourceDeleteCommand : IDeserializedCommand
{
    [DeserializedField(0)] private ushort _inputId;

    [DeserializedField(8)] private long _sourceId;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var audio = state.GetFairlight();

        if (!audio.Inputs.TryGetValue(_inputId, out var input))
        {
            return;
        }

        input.Sources.Remove(_sourceId);
    }
}
