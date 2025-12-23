using AtemSharp.State.Info;

namespace AtemSharp.Tests.TestUtilities.CommandTests;

public class TestCaseData<TTestData> where TTestData : CommandDataBase, new()
{
    public string Name { get; set; } = "";
    public ProtocolVersion FirstVersion { get; set; }
    public byte[] Payload { get; set; } = [];
    public TTestData Command { get; set; } = new();

    public string Json { get; set; } = "";
}
