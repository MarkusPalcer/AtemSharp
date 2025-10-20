using Microsoft.CodeAnalysis;

namespace CodeGenerators
{
    public static class Helpers
    {
        public static string? GetSpanExtensionMethodName(this IFieldSymbol fieldSymbol)
        {
            var typeSymbol = fieldSymbol.Type;
            var name = typeSymbol.Name;
            switch (name)
            {
                case "Boolean":
                    return "Boolean";
                case "Byte":
                case "UInt8":
                    return "UInt8";
                case "SByte":
                case "Int8":
                    return "Int8";
                case "Int16":
                    return "Int16BigEndian";
                case "UInt16":
                    return "UInt16BigEndian";
                case "Int32":
                    return "Int32BigEndian";
                case "UInt32":
                    return "UInt32BigEndian";
                case "Int64":
                    return "Int64BigEndian";
                // Add more mappings as needed
                default:
                    return null;
            }
        }
    }
}
