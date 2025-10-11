using AtemSharp.State;

namespace AtemSharp;

public class Atem
{
	public async Task Connect(string remoteHost, int remotePort = Constants.AtemConstants.DEFAULT_PORT)
	{
		State = new AtemState();
		throw new NotImplementedException();
	}

	public async Task Disconnect()
	{
		throw new NotImplementedException();
	}

	public AtemState State { get; set; } = new();
	public HashSet<string> UnknownCommands { get; set; }
}