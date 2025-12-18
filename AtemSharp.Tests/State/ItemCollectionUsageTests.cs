using AtemSharp.State;
using AtemSharp.State.Audio.ClassicAudio;
using AtemSharp.State.Audio.Fairlight;
using AtemSharp.State.Info;
using AtemSharp.State.Macro;
using AtemSharp.State.Media;
using AtemSharp.State.Recording;
using AtemSharp.State.Settings;
using AtemSharp.State.Settings.MultiViewer;
using AtemSharp.State.Video;
using AtemSharp.State.Video.MixEffect;
using AtemSharp.State.Video.SuperSource;

namespace AtemSharp.Tests.State;

[TestFixture]
public class ItemCollectionUsageTests
{
    [Test]
    public void ClassicAudioChannels()
    {
        var sut = new ClassicAudioState();
        sut.Channels.Populate(5);

        Assert.That(sut.Channels.Select(x => x.Id), Is.EquivalentTo(Enumerable.Range(0, 5)));
    }

    [Test]
    public void FairlightAudioInputSources()
    {
        var sut = new FairlightAudioInput();

        sut.Sources.Populate(5);
        Assert.That(sut.Sources.Select(x => x.Id), Is.EquivalentTo(Enumerable.Range(0, 5)));
    }

    [Test]
    public void FairlightAudioStateInputs()
    {
        var sut = new FairlightAudioState();

        sut.Inputs.Populate(5);
        Assert.That(sut.Inputs.Select(x => x.Id), Is.EquivalentTo(Enumerable.Range(0, 5)));
    }

    [Test]
    public void SourceEqualizerBands()
    {
        var sut = new SourceEqualizer(new Source { InputId = 2, Id = 4 });

        sut.Bands.Populate(5);

        Assert.Multiple(() =>
        {
            Assert.That(sut.Bands.Select(x => x.Id), Is.EquivalentTo(Enumerable.Range(0, 5)));
            Assert.That(sut.Bands.Select(x => x.InputId), Is.All.EqualTo(2));
            Assert.That(sut.Bands.Select(x => x.SourceId), Is.All.EqualTo(4));
        });
    }

    [Test]
    public void DeviceInfoMixEffects()
    {
        var sut = new DeviceInfo();

        sut.MixEffects.Populate(5);
        Assert.That(sut.MixEffects.Select(x => x.Id), Is.EquivalentTo(Enumerable.Range(0, 5)));
    }

    [Test]
    public void DeviceInfoSuperSources()
    {
        var sut = new DeviceInfo();

        sut.SuperSources.Populate(5);
        Assert.That(sut.SuperSources.Select(x => x.Id), Is.EquivalentTo(Enumerable.Range(0, 5)));
    }

    [Test]
    public void MacroStateMacros()
    {
        var sut = new MacroState();

        sut.Macros.Populate(5);
        Assert.That(sut.Macros.Select(x => x.Id), Is.EquivalentTo(Enumerable.Range(1, 5)));
    }

    [Test]
    public void MediaStatePlayers()
    {
        var sut = new MediaState();

        sut.Players.Populate(5);
        Assert.That(sut.Players.Select(x => x.Id), Is.EquivalentTo(Enumerable.Range(0, 5)));
    }

    [Test]
    public void MediaStateClips()
    {
        var sut = new MediaState();

        sut.Clips.Populate(5);
        Assert.That(sut.Clips.Select(x => x.Id), Is.EquivalentTo(Enumerable.Range(0, 5)));
    }

    [Test]
    public void MediaStateFrames()
    {
        var sut = new MediaState();

        sut.Frames.Populate(5);
        Assert.That(sut.Frames.Select(x => x.Id), Is.EquivalentTo(Enumerable.Range(0, 5)));
    }

    [Test]
    public void RecordingStateDisks()
    {
        var sut = new RecordingState();

        sut.Disks.Populate(5);
        Assert.That(sut.Disks.Select(x => x.DiskId), Is.EquivalentTo(Enumerable.Range(0, 5)));
    }

    [Test]
    public void MultiViewerWindows()
    {
        var sut = new MultiViewer { Id = 9 };

        sut.Windows.Populate(5);
        Assert.That(sut.Windows.Select(x => x.WindowIndex), Is.EquivalentTo(Enumerable.Range(0, 5)));
        Assert.That(sut.Windows.Select(x => x.MultiViewerId), Is.All.EqualTo(9));
    }

    [Test]
    public void SettingsStateMultiViewers()
    {
        var sut = new SettingsState();

        sut.MultiViewers.Populate(5);
        Assert.That(sut.MultiViewers.Select(x => x.Id), Is.EquivalentTo(Enumerable.Range(0, 5)));
    }

    [Test]
    public void MixEffectUpstreamKeyers()
    {
        var sut = new MixEffect() { Id = 9 };

        sut.UpstreamKeyers.Populate(5);
        Assert.That(sut.UpstreamKeyers.Select(x => x.Id), Is.EquivalentTo(Enumerable.Range(0, 5)));
        Assert.That(sut.UpstreamKeyers.Select(x => x.MixEffectId), Is.All.EqualTo(9));
    }

    [Test]
    public void SuperSource()
    {
        var sut = new SuperSource() { Id = 9 };

        sut.Boxes.Populate(5);
        Assert.That(sut.Boxes.Select(x => x.Id), Is.EquivalentTo(Enumerable.Range(0, 5)));
        Assert.That(sut.Boxes.Select(x => x.SuperSourceId), Is.All.EqualTo(9));
    }

    [Test]
    public void VideoStateInputs()
    {
        var sut = new VideoState();

        sut.Inputs.Populate(5);
        Assert.That(sut.Inputs.Select(x => x.InputId), Is.EquivalentTo(Enumerable.Range(0, 5)));
    }

    [Test]
    public void VideoStateDownstreamKeyers()
    {
        var sut = new VideoState();

        sut.DownstreamKeyers.Populate(5);
        Assert.That(sut.DownstreamKeyers.Select(x => x.Id), Is.EquivalentTo(Enumerable.Range(0, 5)));
    }

    [Test]
    public void VideoStateMixEffects()
    {
        var sut = new VideoState();

        sut.MixEffects.Populate(5);
        Assert.That(sut.MixEffects.Select(x => x.Id), Is.EquivalentTo(Enumerable.Range(0, 5)));
    }

    [Test]
    public void VideoStateSuperSources()
    {
        var sut = new VideoState();

        sut.SuperSources.Populate(5);
        Assert.That(sut.SuperSources.Select(x => x.Id), Is.EquivalentTo(Enumerable.Range(0, 5)));
    }

    [Test]
    public void VideoStateAuxiliaries()
    {
        var sut = new VideoState();

        sut.Auxiliaries.Populate(5);
        Assert.That(sut.Auxiliaries.Select(x => x.Id), Is.EquivalentTo(Enumerable.Range(0, 5)));
    }

    [Test]
    public void AtemStateColorGenerators()
    {
        var sut = new AtemState();

        sut.ColorGenerators.Populate(5);
        Assert.That(sut.ColorGenerators.Select(x => x.Id), Is.EquivalentTo(Enumerable.Range(0, 5)));
    }
}
