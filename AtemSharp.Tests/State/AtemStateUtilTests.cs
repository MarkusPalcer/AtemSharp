using AtemSharp.State;
using AtemSharp.State.Audio.ClassicAudio;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Tests.State;

[TestFixture]
public class AtemStateUtilTests
{
    [Test]
    public void GetFairlight_IfFairlightAudioPresent_Returns()
    {
        var fairlightAudioState = new FairlightAudioState();
        var state = new AtemState
        {
            Audio = fairlightAudioState
        };

        var retrieved = state.GetFairlight();

        Assert.That(retrieved, Is.SameAs(fairlightAudioState));
    }

    [Test]
    public void GetFairlight_IfNoAudioPresent_Throws()
    {
        var state = new AtemState();

        var ex = Assert.Throws<InvalidOperationException>(() => state.GetFairlight());
        Assert.That(ex.Message, Contains.Substring("is not available"));
        Assert.That(ex.Message, Contains.Substring("Fairlight"));
    }

    [Test]
    public void GetFairlight_IfClassicAudioPresent_Throws()
    {
        var state = new AtemState
        {
            Audio = new ClassicAudioState()
        };

        var ex = Assert.Throws<InvalidOperationException>(() => state.GetFairlight());
        Assert.That(ex.Message, Contains.Substring("is not available"));
        Assert.That(ex.Message, Contains.Substring("Fairlight"));
    }

    [Test]
    public void GetClassicAudio_IfClassicAudioPresent_Returns()
    {
        var classicAudioState = new ClassicAudioState();
        var state = new AtemState
        {
            Audio = classicAudioState
        };

        var retrieved = state.GetClassicAudio();
        Assert.That(retrieved, Is.SameAs(classicAudioState));
    }

    [Test]
    public void GetClassicAudio_IfNoAudioPresent_Throws()
    {
        var state = new AtemState();
        var ex= Assert.Throws<InvalidOperationException>(() => state.GetClassicAudio());
        Assert.That(ex.Message, Contains.Substring("is not available"));
        Assert.That(ex.Message, Contains.Substring("Classic"));
    }

    [Test]
    public void GetClassicAudio_IfFairlightAudioPresent_Throws()
    {
        var state = new AtemState
        {
            Audio = new FairlightAudioState()
        };

        var ex = Assert.Throws<InvalidOperationException>(() => state.GetClassicAudio());
        Assert.That(ex.Message, Contains.Substring("is not available"));
        Assert.That(ex.Message, Contains.Substring("Classic"));
    }
}
