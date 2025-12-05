using System.Runtime.CompilerServices;
using AtemSharp.Communication;
using AtemSharp.FrameworkAbstraction;
using AtemSharp.Lib;

namespace AtemSharp.DependencyInjection;

public interface IServices
{
    IAtemClient CreateAtemClient();
    IActionLoop StartActionLoop(Func<CancellationToken, Task> loopedAction, [CallerArgumentExpression(nameof(loopedAction))] string name = "");
    IAtemProtocol CreateAtemProtocol();
    IUdpClient CreateUdpClient();
    ITimeProvider TimeProvider { get; }
}
