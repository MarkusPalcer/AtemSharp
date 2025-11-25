namespace AtemSharp.State;

// TODO: Replace with proper exceptions where it is still needed
public class InvalidIdError : Exception
{
	public InvalidIdError(string entityType, int id)
		: base($"Invalid {entityType} id: {id}")
	{
	}
}
