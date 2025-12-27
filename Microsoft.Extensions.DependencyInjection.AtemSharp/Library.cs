using AtemSharp;
using AtemSharp.Constants;
using AtemSharp.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection;

public class Library : IAtemSharpLibrary
{
    private readonly IServices _services;

    public Library(IServices services)
    {
        _services = services;
    }

    /// <inheritdoc />
    public IAtemSwitcher CreateAtemSwitcher(string remoteHost, int remotePort = AtemConstants.DefaultPort)
    {
        return new AtemSwitcher(remoteHost, remotePort, _services);
    }
}
