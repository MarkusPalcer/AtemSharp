using AtemSharp.State.Info;

namespace AtemSharp.Commands;

/// <summary>
/// Extension methods for commands so the values attached to the types can be retrieved easily
/// </summary>
public static class CommandExtensions
{
	private static readonly Dictionary<Type, string?> CommandRawNameCache = new();
	private static readonly Dictionary<Type, ProtocolVersion?> CommandMinimumVersionCache = new();

	/// <summary>
	/// Returns the raw name of the command (e.g. "CAMI" for AudioMixerInputCommand)
	/// or null if the command does not have a CommandAttribute
	/// </summary>
	/// <remarks>
	/// This recreates the rawName attribute in the typescript code and whenever that is used, use this method
	/// to get the raw name instead of hardcoding it.
	/// </remarks>
	public static string? GetRawName(this ICommand command)
	{
		return GetRawName(command.GetType());
	}


	private static string? GetRawName(Type commandType)
	{
		if (CommandRawNameCache.TryGetValue(commandType, out var result)) return result;
		var attr = commandType.GetCustomAttributes(typeof(CommandAttribute), false).FirstOrDefault() as CommandAttribute;
		var rawName = attr?.RawName;
		CommandRawNameCache[commandType] = rawName;
		return rawName;
	}

	/// <summary>
	/// Returns the raw name of the command type (e.g. "CAMI" for AudioMixerInputCommand)
	/// or null if the command does not have a CommandAttribute
	/// </summary>
	/// <remarks>
	/// This recreates the rawName attribute in the typescript code and whenever that is used, use this method
	/// to get the raw name instead of hardcoding it.
	/// </remarks>
	public static string? GetRawName<TCommand>() where TCommand : ICommand
	{
		return GetRawName(typeof(TCommand));
	}

	/// <summary>
	/// Returns the minimum protocol version required for the command
	/// or null if the command does not have a CommandAttribute or no minimum version is specified
	/// </summary>
	public static ProtocolVersion? GetMinimumVersion(this ICommand command)
	{
		return GetMinimumVersion(command.GetType());
	}

	private static ProtocolVersion? GetMinimumVersion(Type commandType)
	{
		if (CommandMinimumVersionCache.TryGetValue(commandType, out var result)) return result;
		var attr = commandType.GetCustomAttributes(typeof(CommandAttribute), false).FirstOrDefault() as CommandAttribute;
		var minimumVersion = attr?.MinimumVersion;
		CommandMinimumVersionCache[commandType] = minimumVersion;
		return minimumVersion;
	}

	/// <summary>
	/// Returns the minimum protocol version required for the command type
	/// or null if the command does not have a CommandAttribute or no minimum version is specified
	/// </summary>
	public static ProtocolVersion? GetMinimumVersion<TCommand>() where TCommand : ICommand
	{
		return GetMinimumVersion(typeof(TCommand));
	}

}
