using AtemSharp.Commands.MixEffects.Key;

namespace AtemSharp.Tests.Commands.MixEffects.Key;

[TestFixture]
public class MixEffectKeyAdvancedChromaSampleUpdateCommandTests : DeserializedCommandTestBase<MixEffectKeyAdvancedChromaSampleUpdateCommand, MixEffectKeyAdvancedChromaSampleUpdateCommandTests.CommandData>
{
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

    protected override void CompareCommandProperties(MixEffectKeyAdvancedChromaSampleUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        var failures = new List<string>();

        // Compare MixEffectIndex - it is not floating point so it needs to equal
        if (!actualCommand.MixEffectId.Equals(expectedData.MixEffectIndex))
        {
            failures.Add($"MixEffectId: expected {expectedData.MixEffectIndex}, actual {actualCommand.MixEffectId}");
        }

        // Compare KeyerIndex - it is not floating point so it needs to equal
        if (!actualCommand.KeyerId.Equals(expectedData.KeyerIndex))
        {
            failures.Add($"KeyerId: expected {expectedData.KeyerIndex}, actual {actualCommand.KeyerId}");
        }

        // Compare boolean properties
        if (!actualCommand.EnableCursor.Equals(expectedData.EnableCursor))
        {
            failures.Add($"EnableCursor: expected {expectedData.EnableCursor}, actual {actualCommand.EnableCursor}");
        }

        if (!actualCommand.Preview.Equals(expectedData.Preview))
        {
            failures.Add($"Preview: expected {expectedData.Preview}, actual {actualCommand.Preview}");
        }

        // Compare floating-point properties with tolerance based on scaling factors
        // CursorX and CursorY are scaled by 1000, so use 3 decimal places
        if (!AreApproximatelyEqual(actualCommand.CursorX, expectedData.CursorX, 3))
        {
            failures.Add($"CursorX: expected {expectedData.CursorX}, actual {actualCommand.CursorX}");
        }

        if (!AreApproximatelyEqual(actualCommand.CursorY, expectedData.CursorY, 3))
        {
            failures.Add($"CursorY: expected {expectedData.CursorY}, actual {actualCommand.CursorY}");
        }

        // CursorSize is scaled by 100, so use 2 decimal places
        if (!AreApproximatelyEqual(actualCommand.CursorSize, expectedData.CursorSize, 2))
        {
            failures.Add($"CursorSize: expected {expectedData.CursorSize}, actual {actualCommand.CursorSize}");
        }

        // Sampled values are scaled by 10000, so use 4 decimal places
        if (!AreApproximatelyEqual(actualCommand.SampledY, expectedData.SampledY, 4))
        {
            failures.Add($"SampledY: expected {expectedData.SampledY}, actual {actualCommand.SampledY}");
        }

        if (!AreApproximatelyEqual(actualCommand.SampledCb, expectedData.SampledCb, 4))
        {
            failures.Add($"SampledCb: expected {expectedData.SampledCb}, actual {actualCommand.SampledCb}");
        }

        if (!AreApproximatelyEqual(actualCommand.SampledCr, expectedData.SampledCr, 4))
        {
            failures.Add($"SampledCr: expected {expectedData.SampledCr}, actual {actualCommand.SampledCr}");
        }

        // Assert results
        if (failures.Count > 0)
        {
            Assert.Fail($"Command deserialization property mismatch for version {testCase.FirstVersion}:\n" +
                       string.Join("\n", failures));
        }
    }
}