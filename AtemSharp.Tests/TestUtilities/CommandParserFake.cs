using AtemSharp.Commands;
using AtemSharp.Communication;
using AtemSharp.State.Info;

namespace AtemSharp.Tests.TestUtilities;

internal class CommandParserFake : ICommandParser
{
    public ProtocolVersion Version { get; set; }

    public Queue<IDeserializedCommand?> CommandsToReturn { get; } = new();
    public List<(string, byte[])> ParsedData { get; } = new();

    public IDeserializedCommand? ParseCommand(string rawName, ReadOnlySpan<byte> data)
    {
        Assert.That(CommandsToReturn, Is.Not.Empty);
        ParsedData.Add((rawName, data.ToArray()));
        return CommandsToReturn.Dequeue();
    }
}
