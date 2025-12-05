using System.Diagnostics.CodeAnalysis;
using AtemSharp.Communication;
using AtemSharp.FrameworkAbstraction;
using AtemSharp.Lib;

namespace AtemSharp.DependencyInjection;

[ExcludeFromCodeCoverage]
internal class InternalServices : IServices
{
    public IAtemClient CreateAtemClient()
    {
        return new AtemClient(this);
    }

    public IActionLoop StartActionLoop(Func<CancellationToken, Task> loopedAction, string name = "")
    {
        var result = new ActionLoop(loopedAction, name);
        result.Loop();
        return result;
    }

    public IAtemProtocol CreateAtemProtocol()
    {
        return new AtemProtocol(this);
    }

    public IUdpClient CreateUdpClient()
    {
        return new UdpClientWrapper();
    }

    public ITimeProvider TimeProvider { get; } = new SystemTimeProvider();
}
