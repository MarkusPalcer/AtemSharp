using AtemSharp.Communication;
using AtemSharp.DependencyInjection;
using AtemSharp.FrameworkAbstraction;
using AtemSharp.Lib;
using NSubstitute;

namespace AtemSharp.Tests.TestUtilities;

public sealed class TestServices : IServices, IAsyncDisposable
{
    internal readonly List<ActionLoop> RunningLoops = new();

    public VirtualTime VirtualTime { get; } = new(DateTime.Now);
    public UdpClientFake UdpFake { get; } = new();
    public AtemClientFake ClientFake { get; } = new();

    public AtemProtocolFake ProtocolFake { get; } = new();

    public CommandParserFake CommandParserFake { get; } = new();

    public IPacketBuilder PacketBuilder { get; } = Substitute.For<IPacketBuilder>();

    public IAtemClient CreateAtemClient() => ClientFake;

    public IActionLoop StartActionLoop(Func<CancellationToken, Task> loopedAction, string name = "")
    {
        var result = new ActionLoop(loopedAction, name);
        result.Loop();
        RunningLoops.Add(result);
        return result;
    }

    public IAtemProtocol CreateAtemProtocol() => ProtocolFake;


    public IUdpClient CreateUdpClient() => UdpFake;

    public ITimeProvider TimeProvider => VirtualTime;
    public ICommandParser CreateCommandParser() => CommandParserFake;

    public IPacketBuilder CreatePacketBuilder() => PacketBuilder;


    public async ValueTask DisposeAsync()
    {
        await Task.WhenAll(RunningLoops.Select(x => x.Cancel()));
    }
}
