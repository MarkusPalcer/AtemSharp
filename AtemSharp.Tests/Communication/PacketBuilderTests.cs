using AtemSharp.Communication;
using AtemSharp.State.Info;
using AtemSharp.Tests.TestUtilities.TestCommands;

namespace AtemSharp.Tests.Communication;

[TestFixture]
public class PacketBuilderTests
{
    [Test]
    public void AddCommand_WithoutCommandAttribute_Throws()
    {
        var sut = new PacketBuilder(ProtocolVersion.Unknown);
        var command = new NoRawNameCommand();

        Assert.Throws<InvalidOperationException>(() => sut.AddCommand(command));
        Assert.That(command.SerializeCalled, Is.False);
    }

    [Test]
    public void AddCommand_BiggerThanRestSize_FinishesBuffer()
    {
        var sut = new PacketBuilder(ProtocolVersion.Unknown);
        sut.AddCommand(new VariableSizeCommand(PacketBuilder.MaxPacketSize - 10));
        sut.AddCommand(new VariableSizeCommand(PacketBuilder.MaxPacketSize - 10));

        var packets = sut.GetPackets();
        Assert.That(packets.Count, Is.EqualTo(2));
    }
}
