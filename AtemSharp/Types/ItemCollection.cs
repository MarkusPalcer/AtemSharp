using System.Collections;
using System.Numerics;

namespace AtemSharp.Types;

/// <summary>
/// Represents a collection of state items.
/// Each state item has an ID and can be searched for by ID in the collection
/// </summary>
/// <typeparam name="TId">The numeric type that is used as ID for the type</typeparam>
/// <typeparam name="TItem">The item type</typeparam>
/// <remarks>
/// This acts as a hybrid between a dictionary and an array. <br />
/// For arrays the <see cref="Populate"/> method can be used to fill the array with new items up to a certain ID <br />
/// For dictionaries, the indexer can instantiate
/// </remarks>
public class ItemCollection<TId, TItem> : IEnumerable<TItem>
    where TId : IIncrementOperators<TId>, IComparisonOperators<TId, TId, bool>, IConvertible
{
    private readonly Dictionary<TId, TItem> _items = new();
    private readonly Func<TId, TItem> _factory;

    /// <summary>
    /// Creates a new ItemCollection
    /// </summary>
    /// <param name="factory">The method to create a new item with the given ID</param>
    internal ItemCollection(Func<TId, TItem> factory)
    {
        _factory = factory;
    }

    public TItem this[TId id] => _items[id];

    /// <summary>
    /// Creates all items with IDs from 0 to <code>count - 1</code>
    /// </summary>
    /// <param name="count">The number of items reported by the ATEM switcher</param>
    /// <remarks>
    /// This method is used when applying commands received from the ATEM switcher
    /// </remarks>
    internal void Populate(int count)
    {
        if (_items.Count == count)
        {
            return;
        }

        _items.Clear();

        // IIncrementOperators<TId> makes TId non-nullable, so we can use default! here
        var id = default(TId)!;
        for (var i = 0; i < count; i++)
        {
            _items.Add(id, _factory(id));
            id++;
        }
    }

    /// <summary>
    /// Retrieves an Item that is updated by the ATEM switcher and creates it in case it didn't exist
    /// </summary>
    /// <remarks>
    /// This method is used when applying commands received from the ATEM switcher
    /// </remarks>
    internal TItem GetOrCreate(TId id)
    {
        if (_items.TryGetValue(id, out var item))
        {
            return item;
        }

        item = _factory(id);
        _items.Add(id, item);
        return item;
    }

    internal TItem? GetValueOrDefault(TId id)
    {
        return _items.GetValueOrDefault(id);
    }

    public IEnumerator<TItem> GetEnumerator() => _items.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _items.Values.GetEnumerator();

    public void Remove(TId id)
    {
        _items.Remove(id);
    }
}
