using System.Buffers.Binary;
using System.Text;

namespace AtemSharp.Commands;

/// <summary>
/// Extension methods for working with spans and memory streams for ATEM protocol parsing
/// </summary>
public static class DeserializationExtensions
{
    /// <summary>
    /// Convert UInt16 big-endian value to decibel
    /// </summary>
    /// <param name="input">UInt16 value</param>
    /// <returns>Decibel value</returns>
    public static double UInt16ToDecibel(this ushort input)
    {
        // 0 = -inf, 32768 = 0, 65381 = +6db
        return Math.Round(Math.Log10(input / 32768.0) * 20 * 100) / 100;
    }

    /// <summary>
    /// Convert integer value to balance
    /// </summary>
    /// <param name="input">Integer value</param>
    /// <returns>Balance value (-50 to +50)</returns>
    public static double Int16ToBalance(this short input)
    {
        // -100000 = -50, 0x0000 = 0, 0x2710 = +50
        return input / 200.0;
    }

    /// <summary>
    /// Creates a MemoryStream from a ReadOnlySpan&lt;byte&gt; for use with BinaryReader
    /// </summary>
    /// <param name="span">The span to wrap</param>
    /// <returns>A MemoryStream containing the span data</returns>
    public static MemoryStream ToMemoryStream(this ReadOnlySpan<byte> span)
    {
        return new MemoryStream(span.ToArray());
    }

    /// <summary>
    /// Reads a big-endian UInt16 from the specified offset in the span
    /// </summary>
    /// <param name="span">The span to read from</param>
    /// <param name="offset">The offset to start reading from</param>
    /// <returns>The UInt16 value in host byte order</returns>
    public static ushort ReadUInt16BigEndian(this ReadOnlySpan<byte> span, int offset)
    {
        return BinaryPrimitives.ReadUInt16BigEndian(span.Slice(offset, sizeof(ushort)));
    }

    public static long ReadInt64BigEndian(this ReadOnlySpan<byte> span, int offset)
    {
        return BinaryPrimitives.ReadInt64BigEndian(span.Slice(offset, sizeof(long)));
    }

    public static byte ReadUInt8(this ReadOnlySpan<byte> span, int offset)
    {
        return span[offset];
    }

    public static sbyte ReadInt8(this ReadOnlySpan<byte> span, int offset)
    {
        return unchecked((sbyte)span[offset]);
    }

    public static bool ReadBoolean(this ReadOnlySpan<byte> span, int offset) => ReadUInt8(span, offset) != 0;

    /// <summary>
    /// Reads a big-endian UInt32 from the specified offset in the span
    /// </summary>
    /// <param name="span">The span to read from</param>
    /// <param name="offset">The offset to start reading from</param>
    /// <returns>The UInt32 value in host byte order</returns>
    public static uint ReadUInt32BigEndian(this ReadOnlySpan<byte> span, int offset)
    {
        return BinaryPrimitives.ReadUInt32BigEndian(span.Slice(offset, sizeof(uint)));
    }

    /// <summary>
    /// Reads a big-endian Int16 from the specified offset in the span
    /// </summary>
    /// <param name="span">The span to read from</param>
    /// <param name="offset">The offset to start reading from</param>
    /// <returns>The Int16 value in host byte order</returns>
    public static short ReadInt16BigEndian(this ReadOnlySpan<byte> span, int offset)
    {
        return BinaryPrimitives.ReadInt16BigEndian(span.Slice(offset, sizeof(short)));
    }

    /// <summary>
    /// Reads a big-endian Int32 from the specified offset in the span
    /// </summary>
    /// <param name="span">The span to read from</param>
    /// <param name="offset">The offset to start reading from</param>
    /// <returns>The Int32 value in host byte order</returns>
    public static int ReadInt32BigEndian(this ReadOnlySpan<byte> span, int offset)
    {
        return BinaryPrimitives.ReadInt32BigEndian(span.Slice(offset, sizeof(int)));
    }

    /// <summary>
    /// Writes a big-endian UInt16 to the specified offset in the span
    /// </summary>
    /// <param name="span">The span to write to</param>
    /// <param name="offset">The offset to start writing at</param>
    /// <param name="value">The value to write</param>
    public static void WriteUInt16BigEndian(this Span<byte> span, int offset, ushort value)
    {
        BinaryPrimitives.WriteUInt16BigEndian(span.Slice(offset, sizeof(ushort)), value);
    }

    /// <summary>
    /// Writes a big-endian UInt32 to the specified offset in the span
    /// </summary>
    /// <param name="span">The span to write to</param>
    /// <param name="offset">The offset to start writing at</param>
    /// <param name="value">The value to write</param>
    public static void WriteUInt32BigEndian(this Span<byte> span, int offset, uint value)
    {
        BinaryPrimitives.WriteUInt32BigEndian(span.Slice(offset, sizeof(uint)), value);
    }

    /// <summary>
    /// Writes a big-endian Int16 to the specified offset in the span
    /// </summary>
    /// <param name="span">The span to write to</param>
    /// <param name="offset">The offset to start writing at</param>
    /// <param name="value">The value to write</param>
    public static void WriteInt16BigEndian(this Span<byte> span, int offset, short value)
    {
        BinaryPrimitives.WriteInt16BigEndian(span.Slice(offset, sizeof(short)), value);
    }

    /// <summary>
    /// Writes a big-endian Int32 to the specified offset in the span
    /// </summary>
    /// <param name="span">The span to write to</param>
    /// <param name="offset">The offset to start writing at</param>
    /// <param name="value">The value to write</param>
    public static void WriteInt32BigEndian(this Span<byte> span, int offset, int value)
    {
        BinaryPrimitives.WriteInt32BigEndian(span.Slice(offset, sizeof(int)), value);
    }

    public static string ReadString(this ReadOnlySpan<byte> span, int offset, int maxLength)
    {
        var subSpan = span.Slice(offset, maxLength);

        // Search for null terminator
        var nullIndex = subSpan.IndexOf((byte)0);
        if ( nullIndex > -1)
        {
            subSpan = subSpan[..nullIndex];
        }

        return Encoding.UTF8.GetString(subSpan);
    }

    public static void WriteString(this Span<byte> buffer, string value, int offset, int maxLength)
    {
        var dataBytes = Encoding.UTF8.GetBytes(value);
        var copyLength = Math.Min(dataBytes.Length, maxLength - 1);
        dataBytes[..copyLength].CopyTo(buffer.Slice(offset, copyLength));
        buffer[copyLength] = 0;
    }

    /// <summary>
    /// Extract individual flag components from a combined flag value
    /// </summary>
    /// <param name="value">Combined flag value</param>
    /// <returns>Array of individual flag values</returns>
    public static T[] GetComponents<T>(T value) where T : Enum
    {
        var result = new List<T>();
        var intValue = Convert.ToInt32(value);

        for (var next = 1; next <= intValue; next <<= 1)
        {
            if ((intValue & next) > 0)
            {
                result.Add((T)Enum.ToObject(typeof(T), next));
            }
        }

        return result.ToArray();
    }

    [Obsolete("Only used when the enum type is unknown, prefer using GetComponents<T>")]
    public static byte[] GetComponentsLegacy(byte value)
    {
        var result = new List<byte>();
        for (byte next = 1; next <= value; next <<= 1)
        {
            if ((value & next) > 0)
            {
                result.Add(next);
            }
        }

        return result.ToArray();
    }
}
