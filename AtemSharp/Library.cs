using System.Diagnostics.CodeAnalysis;
using AtemSharp.Constants;
using AtemSharp.DependencyInjection;

namespace AtemSharp;

[ExcludeFromCodeCoverage]
public class Library : IAtemSharpLibrary
{
    private readonly IServices _services = new InternalServices();

    /// <summary>
    /// Manually creates an ATEM switcher instance that represents a switcher at the given host and port combination
    /// </summary>
    /// <param name="remoteHost">IP address of the ATEM device</param>
    /// <param name="remotePort">Port number (default: 9910)</param>
    public IAtemSwitcher CreateAtemSwitcher(string remoteHost, int remotePort = AtemConstants.DefaultPort)
    {
        return new AtemSwitcher(remoteHost, remotePort, _services);
    }
}
