using System.Diagnostics.CodeAnalysis;
using AtemSharp.Communication;
using AtemSharp.FrameworkAbstraction;
using AtemSharp.Lib;

namespace AtemSharp.DependencyInjection;

[ExcludeFromCodeCoverage]
internal class InternalServices : IServices
{
    public IAtemClient CreateAtemClient() => new AtemClient(this);

    public IActionLoop StartActionLoop(Func<CancellationToken, Task> loopedAction, string name = "")
    {
        var result = new ActionLoop(loopedAction, name);
        result.Loop();
        return result;
    }

    public IAtemProtocol CreateAtemProtocol() => new AtemProtocol(this);

    public IUdpClient CreateUdpClient() => new UdpClientWrapper();

    public ITimeProvider TimeProvider { get; } = new SystemTimeProvider();

    public ICommandParser CreateCommandParser() => new CommandParser();
}
