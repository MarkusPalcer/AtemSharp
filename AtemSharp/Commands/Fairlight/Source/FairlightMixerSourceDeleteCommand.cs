using AtemSharp.Helpers;
using AtemSharp.State;

namespace AtemSharp.Commands.Fairlight.Source;

[Command("FASD")]
public partial class FairlightMixerSourceDeleteCommand : IDeserializedCommand
{
    [DeserializedField(0)]
    private ushort _inputId;

    [DeserializedField(8)]
    private long _sourceId;

    public void ApplyToState(AtemState state)
    {
        var audio = state.GetFairlight();

        if (!audio.Inputs.TryGetValue(_inputId, out var input))
        {
            throw new IndexOutOfRangeException($"Input ID {_inputId} does not exist");
        }

        input.Sources.Remove(_sourceId);
    }
}
