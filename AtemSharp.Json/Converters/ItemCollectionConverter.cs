using System.Numerics;
using System.Reflection;
using AtemSharp.Types;
using Newtonsoft.Json;

namespace AtemSharp.Json.Converters;

public class ItemCollectionConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is null)
        {
            serializer.Serialize(writer, null);
            return;
        }

        var objectType = value.GetType();
        var keyType = objectType.GetGenericArguments()[0];
        var valueType = objectType.GetGenericArguments()[1];

        var writeDelegate = typeof(ItemCollectionConverter).GetMethod(nameof(Write), BindingFlags.Static | BindingFlags.NonPublic)!
                                                           .MakeGenericMethod(keyType, valueType)
                                                           .CreateDelegate<WriteMethod>();

        writeDelegate(writer, value, serializer);
    }

    delegate void WriteMethod(JsonWriter writer, object? value, JsonSerializer serializer);

    private static void Write<TKey, TValue>(JsonWriter writer, object? value, JsonSerializer serializer)
        where TKey : IIncrementOperators<TKey>,
        IComparisonOperators<TKey, TKey, bool>,
        IConvertible
    {
        if (value is not ItemCollection<TKey, TValue> itemCollection)
        {
            serializer.Serialize(writer, null);
            return;
        }

        var items = itemCollection.AsReadOnly();
        var dictionary = items.ToDictionary(x => $"{x.Key}", x => (object?)x.Value);

        serializer.Serialize(writer, dictionary);
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        throw new InvalidOperationException("Parsing JSON is not supported.");
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType.IsConstructedGenericType && objectType.GetGenericTypeDefinition() == typeof(ItemCollection<,>);
    }
}
