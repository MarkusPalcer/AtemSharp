namespace AtemSharp.Commands2;

public abstract class WritableCommand<TSelf> : BasicWritableCommand
where TSelf : WritableCommand<TSelf>
{
	
	protected abstract (byte MaskFlag, Func<TSelf, object> Getter, Action<TSelf, object> Setter)[] PropertyMap { get; }
	
	/// <summary>
	/// Property change flags
	/// </summary>
	public byte Flag { get; protected set; }

	/// <summary>
	/// Update the values of some properties with this command
	/// </summary>
	/// <param name="newValue">Properties to update</param>
	/// <returns>True if any properties were changed</returns>
	public bool UpdateProps(TSelf newValue)
	{
		return UpdatePropsInternal(newValue);
	}
	
	protected bool UpdatePropsInternal(TSelf newValue)
	{
		bool hasChanges = false;
        
			foreach (var (flagValue,getter, setter) in PropertyMap)
			{
				if (!getter((TSelf)this).Equals(getter(newValue)))
				{
					Flag |= flagValue;
					setter((TSelf)this, getter(newValue));
					hasChanges = true;
				}
			}
        
		return hasChanges;
	}
}