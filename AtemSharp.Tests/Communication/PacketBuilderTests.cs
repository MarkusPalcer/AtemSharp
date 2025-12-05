using AtemSharp.Communication;
using AtemSharp.Tests.TestUtilities.TestCommands;

namespace AtemSharp.Tests.Communication;

[TestFixture]
public class PacketBuilderTests
{
    [Test]
    public void AddCommand_WithoutCommandAttribute_Throws()
    {
        var sut = new PacketBuilder();
        var command = new NoRawNameCommand();

        var ex = Assert.Throws<InvalidOperationException>(() => sut.AddCommand(command));
        Assert.Multiple(() => {
            Assert.That(ex.Message, Contains.Substring("raw name"));
            Assert.That(ex.Message, Contains.Substring(nameof(NoRawNameCommand)));
            Assert.That(command.SerializeCalled, Is.False);
        });
    }

    [Test]
    public void AddCommand_BiggerThanRestSize_FinishesBuffer()
    {
        var sut = new PacketBuilder();
        sut.AddCommand(new VariableSizeCommand(0x69, PacketBuilder.MaxPacketSize - 10));
        sut.AddCommand(new VariableSizeCommand(0x42, PacketBuilder.MaxPacketSize - 10));

        var packets = sut.GetPackets();
        Assert.That(packets.Count, Is.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(packets[0][Constants.AtemConstants.CommandHeaderSize..],
                            Is.EquivalentTo(Enumerable.Repeat((byte)0x69, PacketBuilder.MaxPacketSize - 10)));
            Assert.That(packets[1][Constants.AtemConstants.CommandHeaderSize..],
                        Is.EquivalentTo(Enumerable.Repeat((byte)0x42, PacketBuilder.MaxPacketSize - 10)));
        });
    }

    [Test]
    public void AddCommand_WithExactBufferSize_ReturnsOnlyOneBuffer()
    {
        var sut = new PacketBuilder();
        sut.AddCommand(new VariableSizeCommand(0x69, PacketBuilder.MaxPacketSize - Constants.AtemConstants.CommandHeaderSize));

        var packets = sut.GetPackets();
        Assert.That(packets.Count, Is.EqualTo(1));
        Assert.That(packets[0][Constants.AtemConstants.CommandHeaderSize..],
                    Is.EquivalentTo(Enumerable.Repeat((byte)0x69, PacketBuilder.MaxPacketSize - Constants.AtemConstants.CommandHeaderSize)));
    }

    [Test]
    public void GetPackets_ForgetsOldCommands()
    {
        var sut = new PacketBuilder();
        sut.AddCommand(new VariableSizeCommand(0x69, PacketBuilder.MaxPacketSize - 10));
        _ = sut.GetPackets();
        sut.AddCommand(new VariableSizeCommand(0x42, PacketBuilder.MaxPacketSize - 10));

        var packets = sut.GetPackets();
        Assert.That(packets.Count, Is.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(packets[0][Constants.AtemConstants.CommandHeaderSize..],
                        Is.EquivalentTo(Enumerable.Repeat((byte)0x42, PacketBuilder.MaxPacketSize - 10)));
        });
    }

    [Test]
    public void GetPackest_WithoutAddingCommand()
    {
        var sut = new PacketBuilder();
        Assert.That(sut.GetPackets(), Is.Empty);
    }
}
