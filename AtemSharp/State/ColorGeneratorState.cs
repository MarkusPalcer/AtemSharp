using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class ColorGeneratorState : ItemWithId<int>
{
    public byte Id { get; internal set; }
    public double Hue { get; internal set; }
    public double Saturation { get; internal set; }
    public double Luma { get; internal set; }

    internal override void SetId(int id) => Id = (byte)id;
}
