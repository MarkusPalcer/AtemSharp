using System;
using System.IO;
using System.Linq;
using System.Text;
using AtemSharp.CodeGenerators.Serialization;
using Microsoft.CodeAnalysis;

namespace AtemSharp.CodeGenerators
{
    public static class Helpers
    {
        public static string? GetSerializationMethod(IFieldSymbol fieldSymbol)
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
                return string.Empty;

            try
            {
                var doc = System.Xml.Linq.XDocument.Parse(xml);
                var member = doc.Root;
                if (member == null || member.Name != "member")
                    return string.Empty;

                // Get the inner XML (everything inside <member>...</member>)
                var innerXml = string.Concat(member.Nodes().Select(n => n.ToString()));
                var lines = innerXml.Replace("\r\n", "\n").Replace("\r", "\n").Split('\n');
                var sb = new StringBuilder();
                foreach (var line in lines)
                {
                    var trimmed = line.TrimEnd();
                    if (trimmed.Length > 0)
                        sb.AppendLine($"/// {trimmed}");
                }
                return sb.ToString();
            }
            catch
            {
                // Fallback: return nothing if XML is malformed
                return string.Empty;
            }
        }

        public static double GetScalingFactor(IFieldSymbol f)
        {
            // Check for ScalingFactorAttribute
            var scalingAttr = f.GetAttributes().FirstOrDefault(a => a.AttributeClass?.Name == "ScalingFactorAttribute");
            if (scalingAttr == null) return 1.0;

            var arg = scalingAttr.ConstructorArguments[0];

            return arg.Value switch
            {
                double value1 => value1,
                float value2 => value2,
                int value3 => value3,
                _ => 1.0
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
                propertyName = char.ToUpper(propertyName[0]) + propertyName.Substring(1);
            else if (!string.IsNullOrEmpty(propertyName))
                propertyName = propertyName.ToUpper();
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
                    offset = (uint)intVal;
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
                    offset = (uint)intVal;
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
                        throw new InvalidOperationException("Resource stream is null.");

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
                return null;

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
    }
}
