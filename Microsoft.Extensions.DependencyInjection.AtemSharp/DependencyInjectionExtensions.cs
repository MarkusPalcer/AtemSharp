using AtemSharp;
using AtemSharp.Communication;
using AtemSharp.DependencyInjection;
using AtemSharp.FrameworkAbstraction;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Adds the <code>AtemSharp</code> library to the <see cref="IServiceProvider"/>
    /// </summary>
    public static IServiceCollection AddAtemSharp(this IServiceCollection self)
    {
        self.AddTransient<IAtemClient, AtemClient>();
        self.AddTransient<IAtemProtocol, AtemProtocol>();
        self.AddTransient<IUdpClient, UdpClientWrapper>();
        self.AddTransient<ICommandParser, CommandParser>();

        self.AddSingleton<ITimeProvider, SystemTimeProvider>();
        self.AddSingleton<IAtemSharpLibrary, Library>();
        self.AddSingleton<IServices, ContainerBridge>();

        return self;
    }
}
