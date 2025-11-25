using AtemSharp.Commands.MixEffects.Key;
using AtemSharp.State;
using AtemSharp.State.Info;

namespace AtemSharp.Tests.Commands.MixEffects.Key;

[TestFixture]
public class MixEffectKeyAdvancedChromaPropertiesCommandTests : SerializedCommandTestBase<MixEffectKeyAdvancedChromaPropertiesCommand,
    MixEffectKeyAdvancedChromaPropertiesCommandTests.CommandData>
{
    /// <summary>
    /// Specify which byte ranges contain floating-point values that should be compared with tolerance
    /// </summary>
    protected override Range[] GetFloatingPointByteRanges()
    {
        return
        [
            4..6, // ForegroundLevel field (UInt16)
            6..8, // BackgroundLevel field (UInt16)
            8..10, // KeyEdge field (UInt16)
            10..12, // SpillSuppression field (UInt16)
            12..14, // FlareSuppression field (UInt16)
            14..16, // Brightness field (Int16)
            16..18, // Contrast field (Int16)
            18..20, // Saturation field (UInt16)
            20..22, // Red field (Int16)
            22..24, // Green field (Int16)
            24..26 // Blue field (Int16)
        ];
    }

    public class CommandData : CommandDataBase
    {
        public byte MixEffectIndex { get; set; }
        public byte KeyerIndex { get; set; }
        public double ForegroundLevel { get; set; }
        public double BackgroundLevel { get; set; }
        public double KeyEdge { get; set; }
        public double SpillSuppression { get; set; }
        public double FlareSuppression { get; set; }
        public double Brightness { get; set; }
        public double Contrast { get; set; }
        public double Saturation { get; set; }
        public double Red { get; set; }
        public double Green { get; set; }
        public double Blue { get; set; }
    }

    protected override MixEffectKeyAdvancedChromaPropertiesCommand CreateSut(TestCaseData testCase)
    {
        // Create state with the required mix effect and upstream keyer
        var state = CreateStateWithUpstreamKeyerAdvancedChroma(
            testCase.Command.MixEffectIndex,
            testCase.Command.KeyerIndex,
            testCase.Command.ForegroundLevel,
            testCase.Command.BackgroundLevel,
            testCase.Command.KeyEdge,
            testCase.Command.SpillSuppression,
            testCase.Command.FlareSuppression,
            testCase.Command.Brightness,
            testCase.Command.Contrast,
            testCase.Command.Saturation,
            testCase.Command.Red,
            testCase.Command.Green,
            testCase.Command.Blue);

        // Create command with the mix effect and keyer IDs
        var command = new MixEffectKeyAdvancedChromaPropertiesCommand(testCase.Command.MixEffectIndex, testCase.Command.KeyerIndex, state);

        // Set the actual advanced chroma properties that should be written
        command.ForegroundLevel = testCase.Command.ForegroundLevel;
        command.BackgroundLevel = testCase.Command.BackgroundLevel;
        command.KeyEdge = testCase.Command.KeyEdge;
        command.SpillSuppression = testCase.Command.SpillSuppression;
        command.FlareSuppression = testCase.Command.FlareSuppression;
        command.Brightness = testCase.Command.Brightness;
        command.Contrast = testCase.Command.Contrast;
        command.Saturation = testCase.Command.Saturation;
        command.Red = testCase.Command.Red;
        command.Green = testCase.Command.Green;
        command.Blue = testCase.Command.Blue;

        return command;
    }

    /// <summary>
    /// Creates an AtemState with a valid mix effect and upstream keyer with advanced chroma settings at the specified indices
    /// </summary>
    private static AtemState CreateStateWithUpstreamKeyerAdvancedChroma(byte mixEffectId, byte keyerId,
                                                                        double foregroundLevel = 0.0, double backgroundLevel = 0.0,
                                                                        double keyEdge = 0.0,
                                                                        double spillSuppression = 0.0, double flareSuppression = 0.0,
                                                                        double brightness = 0.0,
                                                                        double contrast = 0.0, double saturation = 0.0, double red = 0.0,
                                                                        double green = 0.0, double blue = 0.0)
    {
        var mixEffects = new MixEffect[mixEffectId + 1];
        var upstreamKeyers = new Dictionary<int, UpstreamKeyer>();

        upstreamKeyers[keyerId] = new UpstreamKeyer
        {
            Id = keyerId,
            OnAir = false,
            FillSource = 1000,
            CutSource = 1001,
            AdvancedChromaSettings =
            {
                Properties =
                {
                    ForegroundLevel = foregroundLevel,
                    BackgroundLevel = backgroundLevel,
                    KeyEdge = keyEdge,
                    SpillSuppression = spillSuppression,
                    FlareSuppression = flareSuppression,
                    Brightness = brightness,
                    Contrast = contrast,
                    Saturation = saturation,
                    Red = red,
                    Green = green,
                    Blue = blue
                }
            }
        };

        mixEffects[mixEffectId] = new MixEffect
        {
            Id = mixEffectId,
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
