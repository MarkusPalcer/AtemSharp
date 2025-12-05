using AtemSharp.Communication;
using AtemSharp.DependencyInjection;
using AtemSharp.FrameworkAbstraction;
using AtemSharp.Lib;

namespace Microsoft.Extensions.DependencyInjection;

public class ContainerBridge(IServiceProvider serviceProvider) : IServices
{
    public IAtemClient CreateAtemClient()
    {
        return serviceProvider.GetRequiredService<IAtemClient>();
    }

    public IActionLoop StartActionLoop(Func<CancellationToken, Task> loopedAction, string name = "")
    {
        var result = new ActionLoop(loopedAction, name);
        result.Loop();
        return result;
    }

    public IAtemProtocol CreateAtemProtocol()
    {
        return serviceProvider.GetRequiredService<IAtemProtocol>();
    }

    public IUdpClient CreateUdpClient()
    {
        return serviceProvider.GetRequiredService<IUdpClient>();
    }

    public ITimeProvider TimeProvider { get; } = serviceProvider.GetRequiredService<ITimeProvider>();
}
