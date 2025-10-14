using System.Text;
using AtemSharp.Commands;
using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.Enums;
using AtemSharp.State;
using AtemSharp.Tests.TestUtilities;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

[TestFixture]
// ReSharper disable once InconsistentNaming Domain Specific Acronym
public class TransitionDVEUpdateCommandTests : DeserializedCommandTestBase<TransitionDVEUpdateCommand,
    TransitionDVEUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public int Rate { get; set; }
        public int LogoRate { get; set; }
        public DVEEffect Style { get; set; }
        public int FillSource { get; set; }
        public int KeySource { get; set; }
        public bool EnableKey { get; set; }
        public bool PreMultiplied { get; set; }
        public double Clip { get; set; }  // Use double for fractional values
        public double Gain { get; set; }  // Use double for fractional values
        public bool InvertKey { get; set; }
        public bool Reverse { get; set; }
        public bool FlipFlop { get; set; }
    }

    protected override void CompareCommandProperties(TransitionDVEUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        var failures = new List<string>();

        // Compare Index to MixEffectId - exact match
        if (!actualCommand.MixEffectId.Equals(expectedData.Index))
        {
            failures.Add($"MixEffectId: expected {expectedData.Index}, actual {actualCommand.MixEffectId}");
        }

        // Compare Rate - exact match (integer value)
        if (!actualCommand.Rate.Equals(expectedData.Rate))
        {
            failures.Add($"Rate: expected {expectedData.Rate}, actual {actualCommand.Rate}");
        }

        // Compare LogoRate - exact match (integer value)
        if (!actualCommand.LogoRate.Equals(expectedData.LogoRate))
        {
            failures.Add($"LogoRate: expected {expectedData.LogoRate}, actual {actualCommand.LogoRate}");
        }

        // Compare Style - exact match (enum value)
        if (!actualCommand.Style.Equals(expectedData.Style))
        {
            failures.Add($"Style: expected {expectedData.Style}, actual {actualCommand.Style}");
        }

        // Compare FillSource - exact match (integer value)
        if (!actualCommand.FillSource.Equals(expectedData.FillSource))
        {
            failures.Add($"FillSource: expected {expectedData.FillSource}, actual {actualCommand.FillSource}");
        }

        // Compare KeySource - exact match (integer value)
        if (!actualCommand.KeySource.Equals(expectedData.KeySource))
        {
            failures.Add($"KeySource: expected {expectedData.KeySource}, actual {actualCommand.KeySource}");
        }

        // Compare EnableKey - exact match (boolean value)
        if (!actualCommand.EnableKey.Equals(expectedData.EnableKey))
        {
            failures.Add($"EnableKey: expected {expectedData.EnableKey}, actual {actualCommand.EnableKey}");
        }

        // Compare PreMultiplied - exact match (boolean value)
        if (!actualCommand.PreMultiplied.Equals(expectedData.PreMultiplied))
        {
            failures.Add($"PreMultiplied: expected {expectedData.PreMultiplied}, actual {actualCommand.PreMultiplied}");
        }

        // Compare Clip - use approximate comparison with 1 decimal place precision (scaled by 10)
        if (!Utilities.AreApproximatelyEqual(actualCommand.Clip, expectedData.Clip, 1))
        {
            failures.Add($"Clip: expected {expectedData.Clip}, actual {actualCommand.Clip}");
        }

        // Compare Gain - use approximate comparison with 1 decimal place precision (scaled by 10)
        if (!Utilities.AreApproximatelyEqual(actualCommand.Gain, expectedData.Gain, 1))
        {
            failures.Add($"Gain: expected {expectedData.Gain}, actual {actualCommand.Gain}");
        }

        // Compare InvertKey - exact match (boolean value)
        if (!actualCommand.InvertKey.Equals(expectedData.InvertKey))
        {
            failures.Add($"InvertKey: expected {expectedData.InvertKey}, actual {actualCommand.InvertKey}");
        }

        // Compare Reverse - exact match (boolean value)
        if (!actualCommand.Reverse.Equals(expectedData.Reverse))
        {
            failures.Add($"Reverse: expected {expectedData.Reverse}, actual {actualCommand.Reverse}");
        }

        // Compare FlipFlop - exact match (boolean value)
        if (!actualCommand.FlipFlop.Equals(expectedData.FlipFlop))
        {
            failures.Add($"FlipFlop: expected {expectedData.FlipFlop}, actual {actualCommand.FlipFlop}");
        }

        // Assert results
        if (failures.Count > 0)
        {
            Assert.Fail($"Command deserialization property mismatch for version {testCase.FirstVersion}:\n" +
                       string.Join("\n", failures));
        }
    }

    [Test]
    public void Deserialize_ValidData_ProducesCorrectCommand()
    {
        // Arrange - create test data that matches the TypeScript deserialization pattern
        var mixEffectId = (byte)1;
        var rate = (byte)25;
        var logoRate = (byte)15;
        var style = (byte)DVEEffect.SwooshTop;
        var fillSourceHigh = (byte)0x03;  // High byte of 1000 (0x03E8)
        var fillSourceLow = (byte)0xE8;   // Low byte of 1000
        var keySourceHigh = (byte)0x07;   // High byte of 2000 (0x07D0)
        var keySourceLow = (byte)0xD0;    // Low byte of 2000
        var enableKey = (byte)1;
        var preMultiplied = (byte)1;
        var clip = (ushort)500;
        var gain = (ushort)750;
        var invertKey = (byte)1;
        var reverse = (byte)1;
        var flipFlop = (byte)1;

        using var stream = new MemoryStream();
        using (var writer = new BinaryWriter(stream, Encoding.ASCII, leaveOpen: true))
        {
            writer.Write(mixEffectId);
            writer.Write(rate);
            writer.Write(logoRate);
            writer.Write(style);
            writer.Write(fillSourceHigh);
            writer.Write(fillSourceLow);
            writer.Write(keySourceHigh);
            writer.Write(keySourceLow);
            writer.Write(enableKey);
            writer.Write(preMultiplied);
            writer.WriteUInt16BigEndian(clip);
            writer.WriteUInt16BigEndian(gain);
            writer.Write(invertKey);
            writer.Write(reverse);
            writer.Write(flipFlop);
        }
        stream.Position = 0;

        // Act
        var command = TransitionDVEUpdateCommand.Deserialize(stream, ProtocolVersion.V8_0);

        // Assert
        Assert.That(command.MixEffectId, Is.EqualTo(mixEffectId));
        Assert.That(command.Rate, Is.EqualTo(rate));
        Assert.That(command.LogoRate, Is.EqualTo(logoRate));
        Assert.That(command.Style, Is.EqualTo(DVEEffect.SwooshTop));
        Assert.That(command.FillSource, Is.EqualTo(1000)); // (0x03 << 8) | 0xE8 = 1000
        Assert.That(command.KeySource, Is.EqualTo(2000));  // (0x07 << 8) | 0xD0 = 2000
        Assert.That(command.EnableKey, Is.True);
        Assert.That(command.PreMultiplied, Is.True);
        Assert.That(command.Clip, Is.EqualTo(50));  // 500 / 10 = 50
        Assert.That(command.Gain, Is.EqualTo(75));  // 750 / 10 = 75
        Assert.That(command.InvertKey, Is.True);
        Assert.That(command.Reverse, Is.True);
        Assert.That(command.FlipFlop, Is.True);
    }

    [Test]
    public void ApplyToState_WithValidState_UpdatesDVESettings()
    {
        // Arrange
        var mixEffectId = 0;
        var command = new TransitionDVEUpdateCommand
        {
            MixEffectId = mixEffectId,
            Rate = 30,
            LogoRate = 20,
            Style = DVEEffect.SwooshBottom,
            FillSource = 1500,
            KeySource = 2500,
            EnableKey = true,
            PreMultiplied = false,
            Clip = 600,
            Gain = 800,
            InvertKey = true,
            Reverse = false,
            FlipFlop = true
        };

        var state = new AtemState
        {
            Info = new DeviceInfo
            {
                Capabilities = new AtemCapabilities
                {
                    MixEffects = 1,
                    DVEs = 1
                }
            },
            Video = new VideoState
            {
                MixEffects = new Dictionary<int, MixEffect>
                {
                    [mixEffectId] = new MixEffect
                    {
                        Index = mixEffectId,
                        TransitionSettings = new TransitionSettings()
                    }
                }
            }
        };

        // Act
        command.ApplyToState(state);

        // Assert
        var dveSettings = state.Video.MixEffects[mixEffectId].TransitionSettings!.DVE;
        Assert.That(dveSettings, Is.Not.Null);
        Assert.That(dveSettings.Rate, Is.EqualTo(30));
        Assert.That(dveSettings.LogoRate, Is.EqualTo(20));
        Assert.That(dveSettings.Style, Is.EqualTo(DVEEffect.SwooshBottom));
        Assert.That(dveSettings.FillSource, Is.EqualTo(1500));
        Assert.That(dveSettings.KeySource, Is.EqualTo(2500));
        Assert.That(dveSettings.EnableKey, Is.True);
        Assert.That(dveSettings.PreMultiplied, Is.False);
        Assert.That(dveSettings.Clip, Is.EqualTo(600));
        Assert.That(dveSettings.Gain, Is.EqualTo(800));
        Assert.That(dveSettings.InvertKey, Is.True);
        Assert.That(dveSettings.Reverse, Is.False);
        Assert.That(dveSettings.FlipFlop, Is.True);
    }

    [Test]
    public void ApplyToState_WithInvalidMixEffect_ThrowsInvalidIdError()
    {
        // Arrange
        var command = new TransitionDVEUpdateCommand { MixEffectId = 5 };
        var state = new AtemState
        {
            Video = new VideoState { MixEffects = new Dictionary<int, MixEffect>() }
        };

        // Act & Assert
        var ex = Assert.Throws<InvalidIdError>(() => command.ApplyToState(state));
        Assert.That(ex.Message, Contains.Substring("MixEffect"));
        Assert.That(ex.Message, Contains.Substring("5"));
    }

    [Test]
    public void ApplyToState_WithoutDVECapability_ThrowsInvalidIdError()
    {
        // Arrange
        var command = new TransitionDVEUpdateCommand { MixEffectId = 0 };
        var state = new AtemState
        {
            Info = new DeviceInfo
            {
                Capabilities = new AtemCapabilities
                {
                    MixEffects = 1,
                    DVEs = 0  // No DVE capability
                }
            },
            Video = new VideoState
            {
                MixEffects = new Dictionary<int, MixEffect>
                {
                    [0] = new MixEffect { Index = 0 }
                }
            }
        };

        // Act & Assert
        var ex = Assert.Throws<InvalidIdError>(() => command.ApplyToState(state));
        Assert.That(ex.Message, Contains.Substring("Invalid DVE id: is not supported"));
    }
}
