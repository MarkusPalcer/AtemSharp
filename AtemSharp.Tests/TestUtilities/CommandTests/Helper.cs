using System.Reflection;
using AtemSharp.State.Settings;

namespace AtemSharp.Tests.TestUtilities.CommandTests;

public static class Helper
{
    /// <summary>
    /// Parse a hex string (e.g., "01-02-03") into a byte array
    /// </summary>
    public static byte[] ParseHexBytes(string hexString)
    {
        return hexString.Split('-').Select(hex => Convert.ToByte(hex, 16)).ToArray();
    }

    public static string GetRessource(string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();

        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            throw new FileNotFoundException($"Could not find embedded resource: {resourceName}");
        }

        using var reader = new StreamReader(stream);
        var json = reader.ReadToEnd();
        return json;
    }

    public static void CompareSerializedBytes(byte[] actualPayload, byte[] expectedPayload, Range[] floatingPointRanges)
    {
        // Check serialized byte stream
        Assert.That(actualPayload, Has.Length.EqualTo(expectedPayload.Length));

        // Step 1: Compare non-float bytes exactly
        var actualNonFloatBytes =
            string.Join("-", actualPayload.Select((b, i) => IsFloatingPointByte(i, actualPayload.Length, floatingPointRanges) ? "XX" : $"{b:X2}"));
        var expectedNonFloatBytes =
            string.Join("-", expectedPayload.Select((b, i) => IsFloatingPointByte(i, actualPayload.Length, floatingPointRanges) ? "XX" : $"{b:X2}"));
        Assert.That(actualNonFloatBytes, Is.EqualTo(expectedNonFloatBytes));

        // Then try approximate match for floating-point fields
        var actualFloatBytes =
            string.Join("-", actualPayload.Select((b, i) => !IsFloatingPointByte(i, actualPayload.Length, floatingPointRanges) ? "XX" : $"{b:X2}"));
        var expectedFloatBytes =
            string.Join("-", expectedPayload.Select((b, i) => !IsFloatingPointByte(i, actualPayload.Length, floatingPointRanges) ? "XX" : $"{b:X2}"));
        if (!AreApproximatelyEqual(actualPayload, expectedPayload, floatingPointRanges))
        {
            Assert.Fail($"Float-bytes differ more than 2 units\n" +
                        $"Expected: {expectedFloatBytes}\n" +
                        $"Actual:   {actualFloatBytes}");
        }
    }

    private static bool IsFloatingPointByte(int index, int totalLength, Range[] ranges)
    {
        foreach (var range in ranges)
        {
            var (start, length) = range.GetOffsetAndLength(totalLength);
            var end = start + length - 1;
            if (index >= start && index <= end)
            {
                return true;
            }
        }

        return false;
    }

    private static bool AreApproximatelyEqual(byte[] actual, byte[] expected, Range[] floatingPointRanges)
    {
        if (actual.Length != expected.Length)
        {
            return false;
        }

        for (var i = 0; i < actual.Length; i++)
        {
            var tolerance = IsFloatingPointByte(i, actual.Length, floatingPointRanges) ? 2 : 0;
            if (Math.Abs(actual[i] - expected[i]) > tolerance)
            {
                // Check if its within the tolerance with carry
                return !(Math.Abs(actual[i] - expected[i]) < 256 - tolerance);
            }
        }

        return true;
    }

    public static IStateHolder CreateDefaultStateHolder()
    {
        var result = new TestStateHolder();

        var macro = result.Macros.GetOrCreate(0);
        macro.UpdateName("First Macro");
        macro.UpdateDescription("First Description");
        macro.UpdateIsUsed(true);
        macro.UpdateHasUnsupportedOps(false);

        macro = result.Macros.GetOrCreate(1);
        macro.UpdateName("Second Macro");
        macro.UpdateDescription("Second Description");
        macro.UpdateIsUsed(true);
        macro.UpdateHasUnsupportedOps(true);

        for (ushort i = 2; i < 100; i++)
        {
            macro = result.Macros.GetOrCreate(i);
            macro.UpdateIsUsed(false);
        }

        result.State.Settings.VideoMode = VideoMode.P1080p25;
        result.State.Info.MacroPool.MacroCount = 100;

        return result;
    }
}
