using System.Reflection;
using AtemSharp.Attributes;
using Newtonsoft.Json;

namespace AtemSharp.Tests.TestUtilities.CommandTests.RecordedTestCases;

public static class Helper
{
    public static IEnumerable<TestCaseData> GetTestCases<TCommand, TTestData>() where TTestData : CommandDataBase, new()
    {
        var commandAttribute = typeof(TCommand).GetCustomAttribute<CommandAttribute>();
        Assert.That(commandAttribute, Is.Not.Null, $"CommandAttribute is required on command class {typeof(TCommand).Name}");

        var rawName = commandAttribute.RawName;

        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "AtemSharp.Tests.TestData.atem-mini-iso-pro.json";

        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            throw new FileNotFoundException($"Could not find embedded resource: {resourceName}");
        }

        using var reader = new StreamReader(stream);
        var json = reader.ReadToEnd();
        var allTestCases = JsonConvert.DeserializeObject<Recordings>(json) ?? new Recordings();
        if (!allTestCases.Commands.TryGetValue(rawName, out var cases))
        {
            yield break;
        }

        foreach (var c in cases)
        {
            var testCase = new TestCaseData<TTestData>
            {
                Command = c.Command.ToObject<TTestData>()!,
                Payload = CommandTests.Helper.ParseHexBytes(c.Payload),
                Json = c.Command.ToString()
            };

            yield return new TestCaseData(testCase).SetName(c.Name);
        }
    }

}
