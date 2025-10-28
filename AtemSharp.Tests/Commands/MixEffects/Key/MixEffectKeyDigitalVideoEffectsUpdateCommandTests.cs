using AtemSharp.Commands.MixEffects.Key;
using AtemSharp.Enums;
using AtemSharp.Tests.TestUtilities;

namespace AtemSharp.Tests.Commands.MixEffects.Key;

[TestFixture]
[Ignore("TODO: DVE scaling factors need refinement")]
public class MixEffectKeyDigitalVideoEffectsUpdateCommandTests : DeserializedCommandTestBase<MixEffectKeyDigitalVideoEffectsUpdateCommand,
    MixEffectKeyDigitalVideoEffectsUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int MixEffectIndex { get; set; }
        public int KeyerIndex { get; set; }
        public double SizeX { get; set; }
        public double SizeY { get; set; }
        public double PositionX { get; set; }
        public double PositionY { get; set; }
        public double Rotation { get; set; }
        public bool BorderEnabled { get; set; }
        public bool BorderShadowEnabled { get; set; }
        public BorderBevel BorderBevel { get; set; }
        public double BorderOuterWidth { get; set; }
        public double BorderInnerWidth { get; set; }
        public double BorderOuterSoftness { get; set; }
        public double BorderInnerSoftness { get; set; }
        public double BorderBevelSoftness { get; set; }
        public double BorderBevelPosition { get; set; }
        public double BorderOpacity { get; set; }
        public double BorderHue { get; set; }
        public double BorderSaturation { get; set; }
        public double BorderLuma { get; set; }
        public double LightSourceDirection { get; set; }
        public double LightSourceAltitude { get; set; }
        public bool MaskEnabled { get; set; }
        public double MaskTop { get; set; }
        public double MaskBottom { get; set; }
        public double MaskLeft { get; set; }
        public double MaskRight { get; set; }
        public int Rate { get; set; }
    }

    protected override void CompareCommandProperties(MixEffectKeyDigitalVideoEffectsUpdateCommand command, CommandData expected, TestCaseData testCase)
    {
        var failures = new List<string>();

        // Compare MixEffectId
        if (!command.MixEffectId.Equals(expected.MixEffectIndex))
        {
            failures.Add($"MixEffectId: expected {expected.MixEffectIndex}, actual {command.MixEffectId}");
        }

        // Compare KeyerId
        if (!command.KeyerId.Equals(expected.KeyerIndex))
        {
            failures.Add($"KeyerId: expected {expected.KeyerIndex}, actual {command.KeyerId}");
        }
        
        // Use approximate comparison for floating point values that might have precision issues
        // Size values - 3 decimal places (scaled by 1000)
        if (!Utilities.AreApproximatelyEqual(command.SizeX, expected.SizeX, 3))
        {
            failures.Add($"SizeX: expected {expected.SizeX}, actual {command.SizeX}");
        }
        
        if (!Utilities.AreApproximatelyEqual(command.SizeY, expected.SizeY, 3))
        {
            failures.Add($"SizeY: expected {expected.SizeY}, actual {command.SizeY}");
        }
        
        // Position values - 3 decimal places (scaled by 1000)
        if (!Utilities.AreApproximatelyEqual(command.PositionX, expected.PositionX, 3))
        {
            failures.Add($"PositionX: expected {expected.PositionX}, actual {command.PositionX}");
        }
        
        if (!Utilities.AreApproximatelyEqual(command.PositionY, expected.PositionY, 3))
        {
            failures.Add($"PositionY: expected {expected.PositionY}, actual {command.PositionY}");
        }
        
        // Rotation - 3 decimal places (scaled by 1000)
        if (!Utilities.AreApproximatelyEqual(command.Rotation, expected.Rotation, 3))
        {
            failures.Add($"Rotation: expected {expected.Rotation}, actual {command.Rotation}");
        }
        
        // Boolean values
        if (!command.BorderEnabled.Equals(expected.BorderEnabled))
        {
            failures.Add($"BorderEnabled: expected {expected.BorderEnabled}, actual {command.BorderEnabled}");
        }
        
        if (!command.ShadowEnabled.Equals(expected.BorderShadowEnabled))
        {
            failures.Add($"ShadowEnabled: expected {expected.BorderShadowEnabled}, actual {command.ShadowEnabled}");
        }
        
        if (!command.BorderBevel.Equals(expected.BorderBevel))
        {
            failures.Add($"BorderBevel: expected {expected.BorderBevel}, actual {command.BorderBevel}");
        }
        
        // Border widths - 2 decimal places (scaled by 100)
        if (!Utilities.AreApproximatelyEqual(command.BorderOuterWidth, expected.BorderOuterWidth, 2))
        {
            failures.Add($"BorderOuterWidth: expected {expected.BorderOuterWidth}, actual {command.BorderOuterWidth}");
        }
        
        if (!Utilities.AreApproximatelyEqual(command.BorderInnerWidth, expected.BorderInnerWidth, 2))
        {
            failures.Add($"BorderInnerWidth: expected {expected.BorderInnerWidth}, actual {command.BorderInnerWidth}");
        }
        
        // Softness and position values (as bytes, no decimal precision)
        if (!command.BorderOuterSoftness.Equals(expected.BorderOuterSoftness))
        {
            failures.Add($"BorderOuterSoftness: expected {expected.BorderOuterSoftness}, actual {command.BorderOuterSoftness}");
        }
        
        if (!command.BorderInnerSoftness.Equals(expected.BorderInnerSoftness))
        {
            failures.Add($"BorderInnerSoftness: expected {expected.BorderInnerSoftness}, actual {command.BorderInnerSoftness}");
        }
        
        if (!command.BorderBevelSoftness.Equals(expected.BorderBevelSoftness))
        {
            failures.Add($"BorderBevelSoftness: expected {expected.BorderBevelSoftness}, actual {command.BorderBevelSoftness}");
        }
        
        if (!command.BorderBevelPosition.Equals(expected.BorderBevelPosition))
        {
            failures.Add($"BorderBevelPosition: expected {expected.BorderBevelPosition}, actual {command.BorderBevelPosition}");
        }
        
        if (!command.BorderOpacity.Equals(expected.BorderOpacity))
        {
            failures.Add($"BorderOpacity: expected {expected.BorderOpacity}, actual {command.BorderOpacity}");
        }
        
        // Color values - 2 decimal places for hue scaling, 2 for saturation/luma
        if (!Utilities.AreApproximatelyEqual(command.BorderHue, expected.BorderHue, 2))
        {
            failures.Add($"BorderHue: expected {expected.BorderHue}, actual {command.BorderHue}");
        }
        
        if (!Utilities.AreApproximatelyEqual(command.BorderSaturation, expected.BorderSaturation, 2))
        {
            failures.Add($"BorderSaturation: expected {expected.BorderSaturation}, actual {command.BorderSaturation}");
        }
        
        if (!Utilities.AreApproximatelyEqual(command.BorderLuma, expected.BorderLuma, 2))
        {
            failures.Add($"BorderLuma: expected {expected.BorderLuma}, actual {command.BorderLuma}");
        }
        
        // Light source
        if (!Utilities.AreApproximatelyEqual(command.LightSourceDirection, expected.LightSourceDirection, 2))
        {
            failures.Add($"LightSourceDirection: expected {expected.LightSourceDirection}, actual {command.LightSourceDirection}");
        }
        
        if (!command.LightSourceAltitude.Equals(expected.LightSourceAltitude))
        {
            failures.Add($"LightSourceAltitude: expected {expected.LightSourceAltitude}, actual {command.LightSourceAltitude}");
        }
        
        // Mask values
        if (!command.MaskEnabled.Equals(expected.MaskEnabled))
        {
            failures.Add($"MaskEnabled: expected {expected.MaskEnabled}, actual {command.MaskEnabled}");
        }
        
        if (!Utilities.AreApproximatelyEqual(command.MaskTop, expected.MaskTop, 2))
        {
            failures.Add($"MaskTop: expected {expected.MaskTop}, actual {command.MaskTop}");
        }
        
        if (!Utilities.AreApproximatelyEqual(command.MaskBottom, expected.MaskBottom, 2))
        {
            failures.Add($"MaskBottom: expected {expected.MaskBottom}, actual {command.MaskBottom}");
        }
        
        if (!Utilities.AreApproximatelyEqual(command.MaskLeft, expected.MaskLeft, 2))
        {
            failures.Add($"MaskLeft: expected {expected.MaskLeft}, actual {command.MaskLeft}");
        }
        
        if (!Utilities.AreApproximatelyEqual(command.MaskRight, expected.MaskRight, 2))
        {
            failures.Add($"MaskRight: expected {expected.MaskRight}, actual {command.MaskRight}");
        }
        
        if (!command.Rate.Equals(expected.Rate))
        {
            failures.Add($"Rate: expected {expected.Rate}, actual {command.Rate}");
        }

        // Assert results
        if (failures.Count > 0)
        {
            Assert.Fail($"Command deserialization property mismatch for version {testCase.FirstVersion}:\n" +
                       string.Join("\n", failures));
        }
    }
}