using System.Drawing;
using AtemSharp.Types;

namespace AtemSharp.Tests.Types;

[TestFixture]
public class HslColorTests
{

    // These cases are the default colors available in the ATEM-Software and the
    // corresponding HSL-Values that the ATEM-Software shows for them
    public static TestCaseData[] TestCases =
    [
        new(0.0, 0.0, 0.0, "#000000"),
        new(0.0, 1.0, 0.333, "#AA0000"),
        new(120.0, 1.0, 0.167, "#005500"),
        new(30.0, 1.0, 0.333, "#AA5500"),
        new(120.0, 1.0, 0.333, "#00AA00"),
        new(60.0, 1.0, 0.333, "#AAAA00"),
        new(120.0, 1.0, 0.5, "#00FF00"),
        new(80.0, 1.0, 0.5, "#AAFF00"),
        new(239.9, 1.0, 0.249, "#00007F"),
        new(315.1, 1.0, 0.333, "#AA007F"),
        new(199.8, 1.0, 0.249, "#00557F"),
        new(330.3, 0.333, 0.5, "#AA557F"),
        new(164.8, 1.0, 0.333, "#00AA7F"),
        new(60.0, 0.202, 0.582, "#AAAA7F"),
        new(149.8, 1.0, 0.5, "#00FF7F"),
        new(99.8, 1.0, 0.749, "#AAFF7F"),
        new(239.9, 1.0, 0.5, "#0000FF"),
        new(279.9, 1.0, 0.5, "#AA00FF"),
        new(219.9, 1.0, 0.5, "#0055FF"),
        new(269.9, 1.0, 0.667, "#AA55FF"),
        new(199.9, 1.0, 0.5, "#00AAFF"),
        new(239.9, 1.0, 0.833, "#AAAAFF"),
        new(180.0, 1.0, 0.5, "#00FFFF"),
        new(180.0, 1.0, 0.833, "#AAFFFF"),
        new(0.0, 1.0, 0.167, "#550000"),
        new(0.0, 1.0, 0.5, "#FF0000"),
        new(60.0, 1.0, 0.167, "#555500"),
        new(20.0, 1.0, 0.5, "#FF5500"),
        new(90.0, 1.0, 0.333, "#55AA00"),
        new(40.0, 1.0, 0.5, "#ffaa00"),
        new(100.0, 1.0, 0.5, "#55ff00"),
        new(60.0, 1.0, 0.5, "#ffff00"),
        new(280.1, 1.0, 0.249, "#55007f"),
        new(330.0, 1.0, 0.5, "#ff007f"),
        new(239.9, 0.198, 0.416, "#55557f"),
        new(345.1, 1.0, 0.667, "#ff557f"),
        new(149.6, 0.333, 0.5, "#55aa7f"),
        new(20.2, 1.0, 0.749, "#ffaa7f"),
        new(134.8, 1.0, 0.667, "#55ff7f"),
        new(60.0, 1.0, 0.749, "#ffff7f"),
        new(259.9, 1.0, 0.5, "#5500ff"),
        new(299.9, 1.0, 0.5, "#ff00ff"),
        new(239.9, 1.0, 0.667, "#5555ff"),
        new(299.9, 1.0, 0.667, "#ff55ff"),
        new(209.9, 1.0, 0.667, "#55aaff"),
        new(299.9, 1.0, 0.833, "#ffaaff"),
        new(180.0, 1.0, 0.667, "#55ffff"),
        new(0.0, 0.0, 1, "#ffffff"),

    ];

    [TestCaseSource(nameof(TestCases))]
    public void HslToRgbTest(double hue, double saturation, double luma, string webColor)
    {
        var hsl = new HslColor(hue, saturation * 100.0, luma * 100.0);
        var rgb = (Color)hsl;

        var expectedRgb = ColorTranslator.FromHtml(webColor);

        Assert.Multiple(() =>
        {
            Assert.That(rgb.R, Is.EqualTo(expectedRgb.R).Within(1));
            Assert.That(rgb.G, Is.EqualTo(expectedRgb.G).Within(1));
            Assert.That(rgb.B, Is.EqualTo(expectedRgb.B).Within(1));
        });
    }

    [TestCaseSource(nameof(TestCases))]
    public void RgbToHslTest(double hue, double saturation, double luma, string webColor)
    {
        TestContext.Out.WriteLine($"Running: {webColor}");

        var rgb = ColorTranslator.FromHtml(webColor);
        var hsl = (HslColor)rgb;

        Assert.Multiple(() =>
        {
            Assert.That(hsl.Hue, Is.EqualTo(hue).Within(0.12));
            Assert.That(hsl.Saturation, Is.EqualTo(saturation * 100.0).Within(0.1));
            Assert.That(hsl.Luma, Is.EqualTo(luma * 100.0).Within(0.1));
        });
    }

}
