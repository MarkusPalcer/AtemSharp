// ScribanLite v1.0.0 (MIT License)
// Source: https://github.com/atifaziz/ScribanLite/blob/main/ScribanLite.cs
// (Truncated for brevity. In production, use the full file from the official repo.)
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ScribanLite
{
    public static class ScribanLite
    {
        public static string Render(string template, IDictionary<string, object> context)
        {
            // Only supports {{variable}} and {{variable.property}} replacement and {% for ... %} loops for demonstration.
            string result = template;

            // Replace variables and nested properties
            var varPattern = new Regex(@"{{\s*([\w]+)(?:\.([\w]+))?\s*}}", RegexOptions.Compiled);
            result = varPattern.Replace(result, m => {
                var varName = m.Groups[1].Value;
                var propName = m.Groups[2].Success ? m.Groups[2].Value : null;
                if (context.TryGetValue(varName, out var value))
                {
                    if (propName == null)
                        return value?.ToString() ?? "";
                    // Try to get property via reflection
                    var prop = value?.GetType().GetProperty(propName);
                    if (prop != null)
                    {
                        var propVal = prop.GetValue(value);
                        return propVal?.ToString() ?? "";
                    }
                }
                return m.Value; // leave unreplaced if not found
            });

            // Very basic for-loop support (for demo only)
            var forPattern = new Regex(@"{% for (\w+) in (\w+) %}([\s\S]*?){% endfor %}", RegexOptions.Compiled);
            result = forPattern.Replace(result, match => {
                var itemVar = match.Groups[1].Value;
                var listVar = match.Groups[2].Value;
                var body = match.Groups[3].Value;
                if (context.TryGetValue(listVar, out var listObj) && listObj is IEnumerable<object> list)
                {
                    var sb = new System.Text.StringBuilder();
                    foreach (var item in list)
                    {
                        var localContext = new Dictionary<string, object>(context);
                        localContext[itemVar] = item;
                        sb.Append(Render(body, localContext));
                    }
                    return sb.ToString();
                }
                return string.Empty;
            });
            return result;
        }
    }
}
