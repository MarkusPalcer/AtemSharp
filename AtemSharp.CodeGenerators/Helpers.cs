using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AtemSharp.CodeGenerators.Serialization;
using Microsoft.CodeAnalysis;

namespace AtemSharp.CodeGenerators
{
    public static class Helpers
    {
        private static readonly IReadOnlyDictionary<string, string> SerializationMethods =
            new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "Boolean", "Boolean" },
                { "Byte", "UInt8" },
                { "UInt8",  "UInt8" },
                { "SByte", "Int8"},
                { "Int8", "Int8" },
                { "Int16", "Int16BigEndian" },
                { "UInt16", "UInt16BigEndian" },
                { "Int32", "Int32BigEndian" },
                { "UInt32", "UInt32BigEndian" },
                { "Int64", "Int64BigEndian" },
                { "Uint64", "UInt64BigEndian" },
            };

        private static readonly IReadOnlyDictionary<string, string> SerializationMethodTypes =
            new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "Boolean", "bool" },
                { "UInt8", "byte" },
                { "Int8", "sbyte" },
                { "Int16BigEndian", "short" },
                { "UInt16BigEndian", "ushort" },
                { "Int32BigEndian", "int" },
                { "UInt32BigEndian", "uint" },
                { "Int64BigEndian", "long" },
                { "UInt64BigEndian", "ulong" },
            };

        public static string GetExtensionMethodType(string extensionMethodName)
        {
            return SerializationMethodTypes.TryGetValue(extensionMethodName, out var result)
                       ? result
                       : $"object /* No mapping for {extensionMethodName} */";
        }

        public static string? GetSerializationMethod(IFieldSymbol fieldSymbol)
        {
            var typeSymbol = GetSerializedFieldType(fieldSymbol);

            var name = typeSymbol.Name;

            // Handle double type specially
            if (name == "Double" || typeSymbol.ToDisplayString() == "double" || typeSymbol.ToDisplayString() == "System.Double")
            {
                // Default underlying type for double is UInt16
                return "UInt16BigEndian";
            }

            // Handle enums by underlying type
            if (typeSymbol.TypeKind == TypeKind.Enum)
            {
                var enumUnderlying = (typeSymbol as INamedTypeSymbol)?.EnumUnderlyingType;
                if (enumUnderlying != null)
                {
                    name = enumUnderlying.Name;
                }
            }

            return SerializationMethods.TryGetValue(name, out var value)
                       ? value
                       : $"Unknown/* For {name} */";
        }

        public static string? GetUnderlyingTypeName(ITypeSymbol enumType)
        {
            if (enumType.TypeKind == TypeKind.Enum)
            {
                var namedType = enumType as INamedTypeSymbol;
                if (namedType != null)
                {
                    var underlying = namedType.EnumUnderlyingType;
                    // If the enum does not have an explicit underlying type, EnumUnderlyingType is null
                    if (underlying != null)
                    {
                        // If the underlying type is Int32 (the default), check if it's explicit
                        // If the enum declaration does not specify, EnumUnderlyingType is still Int32
                        // So, we need to check if the enum declaration explicitly specifies the type
                        // Unfortunately, Roslyn does not provide a direct way to check if it's explicit
                        // So, we return null if Int32, otherwise the name
                        if (underlying.SpecialType == SpecialType.System_Int32)
                        {
                            // Could be implicit, so return null
                            return null;
                        }
                        return underlying.Name;
                    }
                }
                return null;
            }
            // Not an enum, just return the type name
            return enumType.Name;
        }

        public static ITypeSymbol GetSerializedFieldType(IFieldSymbol fieldSymbol)
        {
            // Check for SerializedTypeAttribute and use its constructor parameter as the type if present
            var serializedTypeAttr = fieldSymbol.GetAttributes()
                                                .FirstOrDefault(a => a.AttributeClass?.Name == "SerializedTypeAttribute");
            ITypeSymbol? typeSymbol = null;

            if (serializedTypeAttr != null && serializedTypeAttr.ConstructorArguments.Length > 0)
            {
                var arg = serializedTypeAttr.ConstructorArguments[0];
                if (arg.Value is ITypeSymbol typeArg)
                {
                    typeSymbol = typeArg;
                }
            }

            // Fallback to the field's type if no attribute or invalid argument
            typeSymbol ??= fieldSymbol.Type;

            // If the type is double, return the type symbol for System.Int16 (short)
            if (typeSymbol.Name == "Double" || typeSymbol.ToDisplayString() == "double" || typeSymbol.ToDisplayString() == "System.Double")
            {
                var int16Type = fieldSymbol.ContainingType.ContainingAssembly.GetTypeByMetadataName("System.Int16");
                if (int16Type != null)
                {
                    return int16Type;
                }
            }
            return typeSymbol;
        }

        /// <summary>
        /// Gets the MSDoc-comment (XML documentation comment) of a field as a multiline string.
        /// Returns an empty string if no MSDoc-comment exists.
        /// </summary>
        /// <param name="field">The field symbol to get the documentation for.</param>
        /// <returns>The MSDoc-comment as a multiline string, or empty if none exists.</returns>
        public static string GetFieldMsDocComment(IFieldSymbol field)
        {
            var xml = field.GetDocumentationCommentXml();
            if (string.IsNullOrWhiteSpace(xml))
            {
                return string.Empty;
            }

            try
            {
                var doc = System.Xml.Linq.XDocument.Parse(xml);
                var member = doc.Root;
                if (member == null || member.Name != "member")
                {
                    return string.Empty;
                }

                // Get the inner XML (everything inside <member>...</member>)
                var innerXml = string.Concat(member.Nodes().Select(n => n.ToString()));
                var lines = innerXml.Replace("\r\n", "\n").Replace("\r", "\n").Split('\n');
                var sb = new StringBuilder();
                foreach (var line in lines)
                {
                    var trimmed = line.TrimEnd();
                    if (trimmed.Length > 0)
                    {
                        sb.AppendLine($"/// {trimmed}");
                    }
                }
                return sb.ToString();
            }
            catch
            {
                // Fallback: return nothing if XML is malformed
                return string.Empty;
            }
        }

        public static double? GetScalingFactor(IFieldSymbol f)
        {
            // Check for ScalingFactorAttribute
            var scalingAttr = f.GetAttributes().FirstOrDefault(a => a.AttributeClass?.Name == "ScalingFactorAttribute");
            if (scalingAttr == null)
            {
                return null;
            }

            var arg = scalingAttr.ConstructorArguments[0];

            return arg.Value switch
            {
                double value1 => value1,
                float value2 => value2,
                int value3 => value3,
                _ => null
            };
        }

        public static string GetFieldType(IFieldSymbol f)
        {
            var fieldType = f.Type.ToDisplayString();
            return fieldType;
        }

        public static string GetPropertyName(IFieldSymbol f)
        {
            // Derive property name from field name (remove leading underscores, capitalize first letter)
            var propertyName = f.Name.TrimStart('_');
            if (!string.IsNullOrEmpty(propertyName) && propertyName.Length > 1)
            {
                propertyName = char.ToUpper(propertyName[0]) + propertyName.Substring(1);
            }
            else if (!string.IsNullOrEmpty(propertyName))
            {
                propertyName = propertyName.ToUpper();
            }

            return propertyName;
        }

        public static uint GetDeserializationOffest(IFieldSymbol f)
        {
            var attr = f.GetAttributes().FirstOrDefault(a => a.AttributeClass?.Name == "DeserializedFieldAttribute");
            uint offset = 0;
            if (attr != null && attr.ConstructorArguments.Length > 0)
            {
                var arg = attr.ConstructorArguments[0];
                if (arg.Value is int intVal)
                {
                    offset = (uint)intVal;
                }
            }

            return offset;
        }

        public static uint GetSerializationOffest(IFieldSymbol f)
        {
            var attr = f.GetAttributes().FirstOrDefault(a => a.AttributeClass?.Name == "SerializedFieldAttribute");
            uint offset = 0;
            if (attr != null && attr.ConstructorArguments.Length > 0)
            {
                var arg = attr.ConstructorArguments[0];
                if (arg.Value is int intVal)
                {
                    offset = (uint)intVal;
                }
            }

            return offset;
        }

        public static string? LoadTemplate(string template, SourceProductionContext spc)
        {
            var assembly = typeof(SerializedCommandGenerator).Assembly;
            var resourceName = assembly.GetManifestResourceNames()
                                       .FirstOrDefault(n => n.EndsWith(template));
            string? templateText;
            if (resourceName != null)
            {
                try
                {
                    using var stream = assembly.GetManifestResourceStream(resourceName);
                    if (stream == null)
                    {
                        throw new InvalidOperationException("Resource stream is null.");
                    }

                    using var reader = new StreamReader(stream);

                    templateText = reader.ReadToEnd();
                }
                catch (Exception ex)
                {
                    spc.ReportDiagnostic(Diagnostic.Create(
                                             DiagnosticDescriptors.TemplateLoadError(template),
                                             Location.None,
                                             ex.Message));
                    return null;
                }
            }
            else
            {
                spc.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.TemplateNotFound(template), Location.None));
                return null;
            }

            return templateText;
        }

        public static string? GetAttributeStringValue(IFieldSymbol f, string attributeTypeName)
        {
            // Look for the CustomScalingAttribute
            var attr = f.GetAttributes().FirstOrDefault(a => a.AttributeClass?.Name == attributeTypeName);
            if (attr == null || attr.ConstructorArguments.Length == 0)
            {
                return null;
            }

            var arg = attr.ConstructorArguments[0];

            switch (arg.Kind)
            {
                // Handle constant string, nameof, and interpolated string
                case TypedConstantKind.Primitive when arg.Value is string s:
                    return s;
                case TypedConstantKind.Array when arg.Values.Length > 0:
                    // Interpolated string is compiled as a constant string in attributes, so just join parts
                    return string.Concat(arg.Values.Select(v => v.Value?.ToString() ?? ""));
                case TypedConstantKind.Enum:
                case TypedConstantKind.Type:
                case TypedConstantKind.Error:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return arg.Value?.ToString();
        }

        public static bool InheritsFrom(INamedTypeSymbol? symbol, string baseTypeName)
        {
            while (symbol is { BaseType: { } })
            {
                if (symbol.BaseType.Name == baseTypeName)
                {
                    return true;
                }

                symbol = symbol.BaseType;
            }

            return false;
        }
    }
}
