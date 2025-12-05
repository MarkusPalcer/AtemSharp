namespace AtemSharp;

public interface IAtemSharpLibrary
{
    public IAtemSwitcher CreateAtemSwitcher(string remoteHost, int remotePort = Constants.AtemConstants.DefaultPort);
}
