using AtemSharp.State;
using AtemSharp.State.Info;
using AtemSharp.State.Media;
using AtemSharp.State.Video;
using AtemSharp.State.Video.DownstreamKeyer;
using AtemSharp.State.Video.MixEffect;

namespace AtemSharp.Commands.DeviceProfile;

/// <summary>
/// Device topology command received from ATEM containing device capabilities and configuration
/// </summary>
[Command("_top", ProtocolVersion.V8_0)]
public partial class TopologyCommandV8 : IDeserializedCommand
{
    [DeserializedField(0)] private byte _mixEffects;
    [DeserializedField(1)] private byte _sources;
    [DeserializedField(2)] private byte _downstreamKeyers;
    [DeserializedField(3)] private byte _auxiliaries;
    [DeserializedField(4)] private byte _mixMinusOutputs;
    [DeserializedField(5)] private byte _mediaPlayers;
    [DeserializedField(6)] private byte _serialPorts;
    [DeserializedField(7)] private byte _maxHyperdecks;
    [DeserializedField(8)] private byte _digitalVideoEffects;
    [DeserializedField(9)] private byte _stingers;
    [DeserializedField(10)] private byte _superSources;
    [DeserializedField(12)] private byte _talkbackChannels;
    [DeserializedField(17)] private bool _cameraControl;
    [DeserializedField(21)] private bool _advancedChromaKeyers;
    [DeserializedField(22)] private bool _onlyConfigurableOutputs;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Create capabilities object with all the topology data
        state.Info.Capabilities.MixEffects = MixEffects;
        state.Info.Capabilities.Sources = Sources;
        state.Info.Capabilities.Auxiliaries = Auxiliaries;
        state.Info.Capabilities.MixMinusOutputs = MixMinusOutputs;
        state.Info.Capabilities.MediaPlayers = MediaPlayers;
        state.Info.Capabilities.MultiViewers = 0;
        state.Info.Capabilities.SerialPorts = SerialPorts;
        state.Info.Capabilities.MaxHyperdecks = MaxHyperdecks;
        state.Info.Capabilities.DigitalVideoEffects = DigitalVideoEffects;
        state.Info.Capabilities.Stingers = Stingers;
        state.Info.Capabilities.SuperSources = SuperSources;
        state.Info.Capabilities.TalkbackChannels = TalkbackChannels;
        state.Info.Capabilities.DownstreamKeyers = DownstreamKeyers;
        state.Info.Capabilities.CameraControl = CameraControl;
        state.Info.Capabilities.AdvancedChromaKeyers = AdvancedChromaKeyers;
        state.Info.Capabilities.OnlyConfigurableOutputs = OnlyConfigurableOutputs;

        // Create arrays now that their sizes are known
        state.Info.MixEffects = AtemStateUtil.CreateArray<MixEffectInfo>(MixEffects);
        state.Video.MixEffects = AtemStateUtil.CreateArray<MixEffect>(MixEffects);

        state.Video.Auxiliaries = AtemStateUtil.CreateArray<AuxiliaryOutput>(Auxiliaries);

        state.Media.Players = AtemStateUtil.CreateArray<MediaPlayer>(MediaPlayers);

        state.Info.SuperSources = new SuperSourceInfo[SuperSources];
        state.Video.SuperSources = AtemStateUtil.CreateArray<State.Video.SuperSource.SuperSource>(SuperSources);

        state.Video.DownstreamKeyers = AtemStateUtil.CreateArray<DownstreamKeyer>(DownstreamKeyers);

        state.Info.MultiViewer.Count = 0;
        state.Info.MultiViewer.WindowCount = 0;
    }
}
