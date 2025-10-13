using AtemSharp.Commands.MixEffects.Key;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.MixEffects.Key;

[TestFixture]
public class MixEffectKeyAdvancedChromaSampleCommandTests : SerializedCommandTestBase<MixEffectKeyAdvancedChromaSampleCommand,
    MixEffectKeyAdvancedChromaSampleCommandTests.CommandData>
{
    /// <summary>
    /// Specify which byte ranges contain floating-point values that should be compared with tolerance
    /// </summary>
    protected override Range[] GetFloatingPointByteRanges()
    {
        return [
            6..8,   // CursorX field (Int16)
            8..10,  // CursorY field (Int16)
            10..12, // CursorSize field (UInt16)
            12..14, // SampledY field (UInt16)
            14..16, // SampledCb field (Int16)
            16..18  // SampledCr field (Int16)
        ];
    }

    public class CommandData : CommandDataBase
    {
        public int MixEffectIndex { get; set; }
        public int KeyerIndex { get; set; }
        public bool EnableCursor { get; set; }
        public bool Preview { get; set; }
        public double CursorX { get; set; }
        public double CursorY { get; set; }
        public double CursorSize { get; set; }
        public double SampledY { get; set; }
        public double SampledCb { get; set; }
        public double SampledCr { get; set; }
    }

    protected override MixEffectKeyAdvancedChromaSampleCommand CreateSut(TestCaseData testCase)
    {
        // Create state with the required mix effect and upstream keyer
        var state = CreateStateWithUpstreamKeyerAdvancedChromaSample(
            testCase.Command.MixEffectIndex, 
            testCase.Command.KeyerIndex, 
            testCase.Command.EnableCursor,
            testCase.Command.Preview,
            testCase.Command.CursorX,
            testCase.Command.CursorY,
            testCase.Command.CursorSize,
            testCase.Command.SampledY,
            testCase.Command.SampledCb,
            testCase.Command.SampledCr);
        
        // Create command with the mix effect and keyer IDs
        var command = new MixEffectKeyAdvancedChromaSampleCommand(testCase.Command.MixEffectIndex, testCase.Command.KeyerIndex, state);

        // Set the actual advanced chroma sample settings that should be written
        command.EnableCursor = testCase.Command.EnableCursor;
        command.Preview = testCase.Command.Preview;
        command.CursorX = testCase.Command.CursorX;
        command.CursorY = testCase.Command.CursorY;
        command.CursorSize = testCase.Command.CursorSize;
        command.SampledY = testCase.Command.SampledY;
        command.SampledCb = testCase.Command.SampledCb;
        command.SampledCr = testCase.Command.SampledCr;
        
        return command;
    }

    /// <summary>
    /// Creates an AtemState with a valid mix effect and upstream keyer with advanced chroma sample settings at the specified indices
    /// </summary>
    private static AtemState CreateStateWithUpstreamKeyerAdvancedChromaSample(int mixEffectId, int keyerId, 
        bool enableCursor = false, bool preview = false, double cursorX = 0.0, double cursorY = 0.0,
        double cursorSize = 0.0, double sampledY = 0.0, double sampledCb = 0.0, double sampledCr = 0.0)
    {
        var mixEffects = new Dictionary<int, MixEffect>();
        var upstreamKeyers = new Dictionary<int, UpstreamKeyer>();
        
        upstreamKeyers[keyerId] = new UpstreamKeyer
        {
            Index = keyerId,
            OnAir = false,
            FillSource = 1000,
            CutSource = 1001,
            AdvancedChromaSettings = new UpstreamKeyerAdvancedChromaSettings
            {
                Sample = new UpstreamKeyerAdvancedChromaSample
                {
                    EnableCursor = enableCursor,
                    Preview = preview,
                    CursorX = cursorX,
                    CursorY = cursorY,
                    CursorSize = cursorSize,
                    SampledY = sampledY,
                    SampledCb = sampledCb,
                    SampledCr = sampledCr
                }
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
                    MixEffects = mixEffectId + 1
                }
            }
        };
    }
}