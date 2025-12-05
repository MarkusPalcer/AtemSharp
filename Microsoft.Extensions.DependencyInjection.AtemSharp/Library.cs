using AtemSharp;
using AtemSharp.Constants;
using AtemSharp.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection;

public class Library(IServices services) : IAtemSharpLibrary
{
    /// <inheritdoc />
    public IAtemSwitcher CreateAtemSwitcher(string remoteHost, int remotePort = AtemConstants.DefaultPort)
    {
        return new AtemSwitcher(remoteHost, remotePort, services);
    }
}
