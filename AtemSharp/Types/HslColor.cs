using System.Drawing;

namespace AtemSharp.Types;

/// <summary>
/// Structure to define HSL.
/// </summary>
public struct HslColor : IEquatable<HslColor>
{
    private double _hue;
    private double _saturation;
    private double _luma;

    /// <summary>
    /// Gets or sets the hue
    /// </summary>
    /// <remarks>
    /// Range: [0,360[
    /// </remarks>
    public double Hue
    {
        get => _hue;
        set
        {
            _hue = value;
            while (_hue < 360.0)
            {
                _hue += 360.0;
            }

            _hue %= 360.0;
        }
    }

    /// <summary>
    /// Gets or sets saturation
    /// </summary>
    /// <remarks>
    /// Range: [0.0,100.0]
    /// </remarks>
    public double Saturation
    {
        get => _saturation;
        set => _saturation = value > 100 ? 100 : value < 0 ? 0 : value;
    }

    /// <summary>
    /// Gets or sets the luminance
    /// </summary>
    /// <remarks>
    /// Range: [0.0,100.0]
    /// </remarks>
    public double Luma
    {
        get => _luma;
        set => _luma = value > 100 ? 100 : value < 0 ? 0 : value;
    }

    /// <summary>
    /// Creates an instance of a color represented by Hue, Saturation and Luma
    /// </summary>
    /// <param name="h">Hue ([0,360[)</param>
    /// <param name="s">Saturation in percent ([0.0,100.0])</param>
    /// <param name="l">Lightness in percent ([0.0,100.0])</param>
    public HslColor(double h, double s, double l)
    {
        Hue = h;
        Saturation = s;
        Luma = l;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        return Math.Abs(Hue - ((HslColor)obj).Hue) <= 0.1
            && Math.Abs(Saturation - ((HslColor)obj).Saturation) <= 0.1
            && Math.Abs(Luma - ((HslColor)obj).Luma) <= 0.1;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Hue.GetHashCode() ^ Saturation.GetHashCode() ^
               Luma.GetHashCode();
    }

    public static implicit operator HslColor(Color color)
    {
        var r = color.R / 255.0;
        var g = color.G / 255.0;
        var b = color.B / 255.0;

        var max = Math.Max(r, Math.Max(g, b));
        var min = Math.Min(r, Math.Min(g, b));
        var delta = max - min;

        var hue = 0.0;
        var saturation = 0.0;
        var luminance = (max + min) / 2.0;

        // --- Hue ---
        if (delta > double.Epsilon)
        {
            if (Math.Abs(max - r) < double.Epsilon)
            {
                hue = ((g - b) / delta) % 6.0;
            }
            else if (Math.Abs(max - g) < double.Epsilon)
            {
                hue = ((b - r) / delta) + 2.0;
            }
            else
            {
                hue = ((r - g) / delta) + 4.0;
            }

            hue *= 60.0;
            if (hue < 0)
            {
                hue += 360.0;
            }
        }

        // --- Saturation ---
        if (delta > double.Epsilon)
        {
            saturation = delta / (1 - Math.Abs(2 * luminance - 1));
        }

        return new HslColor(hue, saturation * 100.0, luminance * 100.0);
    }

    public static implicit operator Color(HslColor color)
    {
        var h = color.Hue;
        var s = color.Saturation / 100.0;
        var l = color.Luma / 100.0;

        if (s == 0)
        {
            var value = (byte)(l * 255.0);
            return Color.FromArgb(value, value, value);
        }

        var q = l < 0.5 ? l * (1.0 + s) : l + s - l * s;
        var p = 2.0 * l - q;

        var hk = h / 360.0;
        var T = new double[3];
        T[0] = hk + 1.0 / 3.0;
        T[1] = hk;
        T[2] = hk - 1.0 / 3.0;

        for (var i = 0; i < 3; i++)
        {
            if (T[i] < 0)
            {
                T[i] += 1.0;
            }

            if (T[i] > 1)
            {
                T[i] -= 1.0;
            }

            if (T[i] * 6 < 1)
            {
                T[i] = p + (q - p) * 6.0 * T[i];
            }
            else if (T[i] * 2.0 < 1)
            {
                T[i] = q;
            }
            else if (T[i] * 3.0 < 2)
            {
                T[i] = p + (q - p) * (2.0 / 3.0 - T[i]) * 6.0;
            }
            else
            {
                T[i] = p;
            }
        }

        return Color.FromArgb((byte)(T[0] * 255.0), (byte)(T[1] * 255.0), (byte)(T[2] * 255.0));
    }

    /// <inheritdoc />
    public bool Equals(HslColor other)
    {
        return Equals((object)other);
    }

    public override string ToString()
    {
        return $"HslColor({Hue}, {Saturation}, {Luma})";
    }
}
