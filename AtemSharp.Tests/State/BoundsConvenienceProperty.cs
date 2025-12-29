using System.Drawing;
using AtemSharp.State.Video.MixEffect.UpstreamKeyer;

namespace AtemSharp.Tests.State;

[TestFixture]
public class BoundsConvenienceProperty
{
    [Test]
    public void UpstreamKeyerDigitalVideoEffectSettings()
    {
        var sut = new UpstreamKeyerDigitalVideoEffectSettings
        {
            Location = new PointF(12.3f, 4.56f),
            Size = new SizeF(6f, 12f),
        };
        Assert.Multiple(() =>
        {
            Assert.That(sut.Bounds.Location, Is.EqualTo(new PointF(12.3f, 4.56f)));
            Assert.That(sut.Bounds.Size, Is.EqualTo(new SizeF(6f, 12f)));
        });
    }

    [Test]
    public void UpstreamKeyerFlyKeyframe()
    {
        var sut = new UpstreamKeyerFlyKeyframe
        {
            Location = new PointF(12.3f, 4.56f),
            Size = new SizeF(6f, 12f),
        };
        Assert.Multiple(() =>
        {
            Assert.That(sut.Bounds.Location, Is.EqualTo(new PointF(12.3f, 4.56f)));
            Assert.That(sut.Bounds.Size, Is.EqualTo(new SizeF(6f, 12f)));
        });
    }
}
