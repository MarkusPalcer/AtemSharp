using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp;

public class Atem
{
	private readonly CommandParser _commandParser = new();

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
	
	/// <summary>
	/// Command parser for handling ATEM protocol commands
	/// </summary>
	public CommandParser CommandParser => _commandParser;
	
	/// <summary>
	/// Set of unknown/unrecognized command names encountered during parsing
	/// </summary>
	public static HashSet<string> UnknownCommands { get; } = new();
}