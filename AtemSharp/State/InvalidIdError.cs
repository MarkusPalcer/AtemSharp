namespace AtemSharp.State;

/// <summary>
/// Exception thrown when an invalid ID is used
/// </summary>
public class InvalidIdError : Exception
{
	public InvalidIdError(string entityType, int id) 
		: base($"Invalid {entityType} id: {id}")
	{
	}
    
	public InvalidIdError(string entityType, string id) 
		: base($"Invalid {entityType} id: {id}")
	{
	}
}