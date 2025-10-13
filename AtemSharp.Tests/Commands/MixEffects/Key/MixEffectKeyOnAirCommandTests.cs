using AtemSharp.Commands.MixEffects.Key;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.MixEffects.Key;

[TestFixture]
public class MixEffectKeyOnAirCommandTests : SerializedCommandTestBase<MixEffectKeyOnAirCommand,
    MixEffectKeyOnAirCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int MixEffectIndex { get; set; }
        public int KeyerIndex { get; set; }
        public bool OnAir { get; set; }
    }

    protected override MixEffectKeyOnAirCommand CreateSut(TestCaseData testCase)
    {
        // Create state with the required mix effect and upstream keyer
        var state = CreateStateWithUpstreamKeyer(testCase.Command.MixEffectIndex, testCase.Command.KeyerIndex, testCase.Command.OnAir);
        
        // Create command with the mix effect and keyer IDs
        var command = new MixEffectKeyOnAirCommand(testCase.Command.MixEffectIndex, testCase.Command.KeyerIndex, state);

        // Set the actual on-air value that should be written
        command.OnAir = testCase.Command.OnAir;
        
        return command;
    }

    /// <summary>
    /// Creates an AtemState with a valid mix effect and upstream keyer at the specified indices
    /// </summary>
    private static AtemState CreateStateWithUpstreamKeyer(int mixEffectId, int keyerId, bool onAir = false)
    {
        var mixEffects = new Dictionary<int, MixEffect>();
        var upstreamKeyers = new Dictionary<int, UpstreamKeyer>();
        
        upstreamKeyers[keyerId] = new UpstreamKeyer
        {
            Index = keyerId,
            OnAir = onAir,
            FillSource = 1000,
            CutSource = 1001
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