namespace AtemSharp.State;

/// <summary>
/// Utility methods for working with ATEM state
/// </summary>
public static class AtemStateUtil
{
	/// <summary>
	/// Gets an existing value from the dictionary or creates a new instance if the key doesn't exist.
	/// This method provides sparse indexing behavior similar to JavaScript arrays, where only
	/// explicitly accessed indices contain objects.
	/// </summary>
	/// <typeparam name="T">The type of values stored in the dictionary. Must have a parameterless constructor.</typeparam>
	/// <param name="dict">The dictionary to search in or add to.</param>
	/// <param name="index">The key to look up or create an entry for.</param>
	/// <returns>
	/// The existing value if the key is found, or a newly created instance of <typeparamref name="T"/> 
	/// that has been added to the dictionary.
	/// </returns>
	/// <remarks>
	/// <para>
	/// This extension method enables efficient sparse indexing for ATEM state collections, matching 
	/// the behavior of the TypeScript implementation where arrays can have gaps (undefined entries).
	/// Unlike resizing arrays, this approach only creates objects for indices that are actually accessed.
	/// </para>
	/// <para>
	/// The method is thread-safe with respect to its own operations, but the underlying Dictionary
	/// is not thread-safe. If multiple threads access the same dictionary concurrently, external
	/// synchronization is required.
	/// </para>
	/// <para>
	/// Memory efficiency: Only stores objects for keys that have been accessed, avoiding the overhead
	/// of creating large arrays with mostly null/empty slots.
	/// </para>
	/// </remarks>
	/// <example>
	/// <code>
	/// // Create a dictionary for mix effects
	/// var mixEffects = new Dictionary&lt;int, MixEffect&gt;();
	/// 
	/// // Get or create mix effect at index 0
	/// var me0 = mixEffects.GetOrCreate(0);
	/// 
	/// // Get or create mix effect at index 5 (sparse - indices 1-4 don't exist)
	/// var me5 = mixEffects.GetOrCreate(5);
	/// 
	/// // Subsequent calls return the same instance
	/// var sameMe0 = mixEffects.GetOrCreate(0);
	/// Assert.That(sameMe0, Is.SameAs(me0));
	/// 
	/// // Dictionary only contains keys 0 and 5, not 1-4
	/// Assert.That(mixEffects.Count, Is.EqualTo(2));
	/// Assert.That(mixEffects.ContainsKey(2), Is.False);
	/// </code>
	/// </example>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="dict"/> is null.</exception>
	/// <seealso cref="Dictionary{TKey,TValue}.TryGetValue(TKey, out TValue)"/>
	/// <seealso cref="VideoState.MixEffects"/>
	/// <seealso cref="VideoState.DownstreamKeyers"/>
	/// <seealso cref="VideoState.Auxiliaries"/>
	public static T GetOrCreate<T>(this Dictionary<int, T> dict, int index) where T : new()
	{
		ArgumentNullException.ThrowIfNull(dict);
		
		if (dict.TryGetValue(index, out var value)) return value;
		value = new T();
		dict[index] = value;
		return value;
	}
}