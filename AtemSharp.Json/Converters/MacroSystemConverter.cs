using AtemSharp.State.Macro;
using Newtonsoft.Json;

namespace AtemSharp.Json.Converters;

public class MacroSystemConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) =>  objectType == typeof(MacroSystem);

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        throw new InvalidOperationException("Parsing JSON is not supported.");
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is null)
        {
            writer.WriteNull();
        }

        if (value is not MacroSystem macroSystem)
        {
            throw new InvalidOperationException("Tried to serialize a non MacroSystem with MacroSystemConverter");
        }

        var serialized = new Dictionary<string, object>();

        foreach (var (key, macro) in macroSystem.AsReadOnly())
        {
            serialized[key.ToString()] = macro;
        }

        serialized["Player"] = macroSystem.Player;
        serialized["Recorder"] = macroSystem.Recorder;

        serializer.Serialize(writer, serialized);
    }
}
