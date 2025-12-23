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

    public static IEnumerable<TestCaseData> GetTestCases<TCommand, TTestData>() where TTestData : CommandDataBase, new()
    {
        var libAtemTestCases = LibAtemTestCases.Helper.GetTestCases<TCommand, TTestData>().ToArray();
        var recordedTestCases = RecordedTestCases.Helper.GetTestCases<TCommand, TTestData>().ToArray();

        return libAtemTestCases.Concat(recordedTestCases);
    }
}
