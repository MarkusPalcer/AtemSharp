using AtemSharp.Enums;
using AtemSharp.Enums.Fairlight;
using AtemSharp.Lib;
using AtemSharp.State;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Fairlight;

[Command("FASP")]
public class FairlightMixerSourceUpdateCommand  : FairlightMixerSourceUpdateCommandBase
{
    public static IDeserializedCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion version)
    {
        return new FairlightMixerSourceUpdateCommand
        {
            SourceType = (FairlightAudioSourceType)rawCommand.ReadUInt8(16),
            MaxFramesDelay = rawCommand.ReadUInt8(17),
            FramesDelay = rawCommand.ReadUInt8(18),
            Gain = rawCommand.ReadInt32BigEndian(20) / 100.0,
            HasStereoSimulation = rawCommand.ReadBoolean(24),
            StereoSimulation = rawCommand.ReadInt16BigEndian(26) / 100.0,
            BandCount = rawCommand.ReadUInt8(28),
            EqualizerEnabled = rawCommand.ReadBoolean(29),
            EqualizerGain = rawCommand.ReadInt32BigEndian(32) / 100.0,
            MakeUpGain = rawCommand.ReadInt32BigEndian(36) / 100.0,
            Balance = rawCommand.ReadInt16BigEndian(40) / 100.0,
            FaderGain = rawCommand.ReadInt32BigEndian(44) / 100.0,
            SupportedMixOptions = AtemUtil.GetComponents((FairlightAudioMixOption)rawCommand.ReadUInt8(48)),
            MixOption = (FairlightAudioMixOption)rawCommand.ReadUInt8(49),
        }.DeserializeIds(rawCommand);
    }

    public FairlightAudioMixOption MixOption { get; set; }

    public required FairlightAudioMixOption[] SupportedMixOptions { get; set; }

    public double FaderGain { get; set; }

    public double Balance { get; set; }

    public double MakeUpGain { get; set; }

    public double EqualizerGain { get; set; }

    public bool EqualizerEnabled { get; set; }

    public byte BandCount { get; set; }

    public double StereoSimulation { get; set; }

    public bool HasStereoSimulation { get; set; }

    public byte FramesDelay { get; set; }

    public byte MaxFramesDelay { get; set; }

    public FairlightAudioSourceType SourceType { get; set; }

    public double Gain { get; set; }

    protected override void ApplyToSource(Source source)
    {
        source.Equalizer.Enabled = EqualizerEnabled;
        source.Equalizer.Gain = EqualizerGain;
        if (source.Equalizer.Bands.Length  < BandCount)
        {
            source.Equalizer.Bands = AtemStateUtil.CreateArray<SourceEqualizerBand>(BandCount).ForEachWithIndex((band, index) =>
            {
                band.Index = (byte)index;
                band.InputId = InputId;
                band.SourceId = SourceId;
            });
        }

        source.Dynamics.MakeUpGain = MakeUpGain;
        source.Type = SourceType;
        source.MaxFramesDelay = MaxFramesDelay;
        source.FramesDelay = FramesDelay;
        source.Gain = Gain;
        source.HasStereoSimulation = HasStereoSimulation;
        source.StereoSimulation = StereoSimulation;
        source.Balance = Balance;
        source.FaderGain = FaderGain;
        source.SupportedMixOptions = SupportedMixOptions;
        source.MixOption = MixOption;
    }
}
