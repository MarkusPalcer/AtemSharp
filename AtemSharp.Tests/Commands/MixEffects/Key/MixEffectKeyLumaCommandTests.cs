using AtemSharp.Commands.MixEffects.Key;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.MixEffects.Key;

[TestFixture]
public class MixEffectKeyLumaCommandTests : SerializedCommandTestBase<MixEffectKeyLumaCommand,
    MixEffectKeyLumaCommandTests.CommandData>
{
    /// <summary>
    /// Specify which byte ranges contain floating-point values that should be compared with tolerance
    /// </summary>
    protected override Range[] GetFloatingPointByteRanges()
    {
        return [
            4..6, // Clip field (UInt16)
            6..8  // Gain field (UInt16)
        ];
    }

    public class CommandData : CommandDataBase
    {
        public int MixEffectIndex { get; set; }
        public int KeyerIndex { get; set; }
        public bool PreMultiplied { get; set; }
        public double Clip { get; set; }
        public double Gain { get; set; }
        public bool Invert { get; set; }
    }

    protected override MixEffectKeyLumaCommand CreateSut(TestCaseData testCase)
    {
        // Create state with the required mix effect and upstream keyer
        var state = CreateStateWithUpstreamKeyerLuma(
            testCase.Command.MixEffectIndex, 
            testCase.Command.KeyerIndex, 
            testCase.Command.PreMultiplied,
            testCase.Command.Clip,
            testCase.Command.Gain,
            testCase.Command.Invert);
        
        // Create command with the mix effect and keyer IDs
        var command = new MixEffectKeyLumaCommand(testCase.Command.MixEffectIndex, testCase.Command.KeyerIndex, state);

        // Set the actual luma settings that should be written
        command.PreMultiplied = testCase.Command.PreMultiplied;
        command.Clip = testCase.Command.Clip;
        command.Gain = testCase.Command.Gain;
        command.Invert = testCase.Command.Invert;
        
        return command;
    }

    /// <summary>
    /// Creates an AtemState with a valid mix effect and upstream keyer with luma settings at the specified indices
    /// </summary>
    private static AtemState CreateStateWithUpstreamKeyerLuma(int mixEffectId, int keyerId, 
        bool preMultiplied = false, double clip = 0.0, double gain = 0.0, bool invert = false)
    {
        var mixEffects = new Dictionary<int, MixEffect>();
        var upstreamKeyers = new Dictionary<int, UpstreamKeyer>();
        
        upstreamKeyers[keyerId] = new UpstreamKeyer
        {
            Index = keyerId,
            OnAir = false,
            FillSource = 1000,
            CutSource = 1001,
            LumaSettings = new UpstreamKeyerLumaSettings
            {
                PreMultiplied = preMultiplied,
                Clip = clip,
                Gain = gain,
                Invert = invert
            }
        };

        mixEffects[mixEffectId] = new MixEffect
        {
            Index = mixEffectId,
            ProgramInput = 1000,
            PreviewInput = 2001,
            TransitionPreview = false,
            TransitionPosition = new TransitionPosition
            {
                InTransition = false,
                HandlePosition = 0,
                RemainingFrames = 0
            },
            UpstreamKeyers = upstreamKeyers
        };

        return new AtemState
        {
            Video = new VideoState
            {
                MixEffects = mixEffects
            },
            Info = new DeviceInfo
            {
                Capabilities = new AtemCapabilities
                {
                    MixEffects = 4 // Support up to 4 mix effects
                }
            }
        };
    }
}