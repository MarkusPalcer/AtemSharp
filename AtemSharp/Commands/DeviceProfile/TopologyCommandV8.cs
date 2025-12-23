using AtemSharp.State;
using AtemSharp.State.Info;

namespace AtemSharp.Commands.DeviceProfile;

[Command("_top", ProtocolVersion.V8_0)]
internal partial class TopologyCommandV8 : IDeserializedCommand
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
        state.Info.MixEffects.Populate(MixEffects);
        state.Video.MixEffects.Populate(MixEffects);

        state.Video.Auxiliaries.Populate(Auxiliaries);

        state.Media.Players.Populate(MediaPlayers);

        state.Info.SuperSources.Populate(SuperSources);
        state.Video.SuperSources.Populate(SuperSources);

        state.Video.DownstreamKeyers.Populate(DownstreamKeyers);

        state.Info.MultiViewer.Count = 0;
        state.Info.MultiViewer.WindowCount = 0;
    }
}
