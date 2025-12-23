using System.Buffers.Binary;
using System.Text;

namespace AtemSharp.Commands;

internal static class SerializationExtensions
{
    /// <summary>
    /// Convert balance value to integer
    /// </summary>
    /// <param name="input">Balance value (-50 to +50)</param>
    /// <returns>Integer value</returns>
    public static short BalanceToInt16(this double input)
    {
        return (short)Math.Round(input * 200);
    }

    public static void WriteUInt8(this byte[] self, byte value, int offset)
    {
        self[offset] = value;
    }

    public static void WriteUInt16BigEndian(this byte[] self, ushort value, int offset)
    {
        BinaryPrimitives.WriteUInt16BigEndian(self.AsSpan(offset), value);
    }

    public static void WriteInt64BigEndian(this byte[] self, long value, int offset)
    {
        BinaryPrimitives.WriteInt64BigEndian(self.AsSpan(offset), value);
    }

    public static void WriteUInt64BigEndian(this byte[] self, ulong value, int offset)
    {
        BinaryPrimitives.WriteUInt64BigEndian(self.AsSpan(offset), value);
    }

    public static void WriteUInt32BigEndian(this byte[] self, uint value, int offset)
    {
        BinaryPrimitives.WriteUInt32BigEndian(self.AsSpan(offset), value);
    }

    public static void WriteInt32BigEndian(this byte[] self, int value, int offset)
    {
        BinaryPrimitives.WriteInt32BigEndian(self.AsSpan(offset), value);
    }

    public static void WriteInt16BigEndian(this byte[] self, short value, int offset)
    {
        BinaryPrimitives.WriteInt16BigEndian(self.AsSpan(offset), value);
    }

    public static void WriteBoolean(this byte[] self, bool value, int offset) => WriteUInt8(self, (byte)(value ? 1 : 0), offset);

    public static void WriteString(this byte[] self, string value, int offset)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        Array.Copy(bytes, 0, self, offset, bytes.Length);
    }

    public static void WriteString(this byte[] buffer, string? value, int offset, int maxLength)
    {
        var tempQualifier = buffer.AsSpan().Slice(offset, maxLength);
        var dataBytes = Encoding.UTF8.GetBytes(value ?? string.Empty);
        var copyLength = Math.Min(dataBytes.Length, maxLength - 1);
        dataBytes[..copyLength].CopyTo(tempQualifier[..copyLength]);
        tempQualifier[copyLength] = 0;
    }

    public static void WriteBlob(this byte[] buffer, byte[] decodedHash, int offset, int maxLength)
    {
        var copyLength = Math.Min(decodedHash.Length, maxLength);
        var hashBytes = buffer.AsSpan().Slice(offset, maxLength);
        decodedHash.AsSpan()[..copyLength].CopyTo(hashBytes);
    }

    /// <summary>
    /// Convert decibel value to UInt16 big-endian
    /// </summary>
    /// <param name="input">Decibel value</param>
    /// <returns>UInt16 value</returns>
    public static ushort DecibelToUInt16(this double input)
    {
        return (ushort)Math.Floor(Math.Pow(10, input / 20) * 32768);
    }

    public static int PadToMultiple(int value, int multiple)
    {
        var remainder = value % multiple;
        if (remainder == 0)
        {
            return value;
        }

        return value + multiple - remainder;
    }
}
