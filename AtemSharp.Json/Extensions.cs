using AtemSharp.Json.Converters;
using Newtonsoft.Json;

namespace AtemSharp.Json;

public static class Extensions
{
    /// <summary>
    /// Adds the converters needed to properly convert the ATEM state to JSON to the given
    /// <see cref="JsonSerializerSettings"/>
    /// </summary>
    public static JsonSerializerSettings WithAtemStateSupport(this JsonSerializerSettings options)
    {
        options.Converters.Add(new MacroSystemConverter());
        options.Converters.Add(new ItemCollectionConverter());

        return options;
    }
}
