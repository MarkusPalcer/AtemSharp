using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using AtemSharp.State.Ports;
using AtemSharp.Types;

namespace AtemSharp.State.Audio.Fairlight;

/// <summary>
/// Fairlight audio input state
/// </summary>
[DebuggerDisplay("{" + nameof(ToString) + ",nq}")]
public class FairlightAudioInput
{
    public FairlightAudioInput()
    {
        Sources = new ItemCollection<long, Source>(id => new Source { Id = id });
    }

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public ushort Id { get; internal set; }

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public FairlightInputType InputType { get; internal set; }

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public ExternalPortType ExternalPortType { get; internal set; }

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public FairlightInputConfiguration[] SupportedConfigurations { get; internal set; } = [];

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public FairlightInputConfiguration ActiveConfiguration { get; internal set; }

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public FairlightAnalogInputLevel[] SupportedInputLevels { get; internal set; } = [];

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public FairlightAnalogInputLevel ActiveInputLevel { get; internal set; }

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public bool RcaToXlrEnabled { get; internal set; }

    public ItemCollection<long, Source> Sources { get; }

    public override string ToString() => $"FairlightAudioInput #{Id}";
}
