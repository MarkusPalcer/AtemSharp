using System.Buffers.Binary;

namespace AtemSharp.Lib;

public static class SerializationExtensions
{
	/// <summary>
	/// Add padding bytes to the given stream
	/// </summary>
	/// <param name="self">The BinaryWriter instance to write to</param>
	/// <param name="length">The number of zero bytes to write for padding</param>
	public static void Pad(this BinaryWriter self, uint length)
	{
		for (var i = 0; i < length; i++)
		{
			self.Write((byte)0);
		}
	}

	/// <summary>
	/// Write a 32-bit unsigned integer in big-endian byte order
	/// </summary>
	/// <param name="self">The BinaryWriter instance to write to</param>
	/// <param name="value">The 32-bit unsigned integer value to write</param>
	public static void WriteUInt32BigEndian(this BinaryWriter self, uint value)
	{
		self.Write((byte)(value >> 24));
		self.Write((byte)(value >> 16));
		self.Write((byte)(value >> 8));
		self.Write((byte)(value & 0xFF));
	}

	/// <summary>
	/// Write a 32-bit signed integer in big-endian byte order
	/// </summary>
	/// <param name="self">The BinaryWriter instance to write to</param>
	/// <param name="value">The 32-bit signed integer value to write</param>
	public static void WriteInt32BigEndian(this BinaryWriter self, int value)
	{
		self.Write((byte)(value >> 24));
		self.Write((byte)(value >> 16));
		self.Write((byte)(value >> 8));
		self.Write((byte)(value & 0xFF));
	}

	/// <summary>
	/// Write a 16-bit unsigned integer in big-endian byte order
	/// </summary>
	/// <param name="self">The BinaryWriter instance to write to</param>
	/// <param name="value">The 16-bit unsigned integer value to write</param>
	public static void WriteUInt16BigEndian(this BinaryWriter self, ushort value)
	{
		self.Write((byte)(value >> 8));
		self.Write((byte)(value & 0xFF));
	}

	/// <summary>
	/// Write a 16-bit signed integer in big-endian byte order
	/// </summary>
	/// <param name="self">The BinaryWriter instance to write to</param>
	/// <param name="value">The 16-bit signed integer value to write</param>
	public static void WriteInt16BigEndian(this BinaryWriter self, short value)
	{
		self.Write((byte)(value >> 8));
		self.Write((byte)(value & 0xFF));
	}

	/// <summary>
	/// Write a boolean value to the stream.
	/// Writes 1 for true, 0 for false.
	/// </summary>
	/// <param name="self">The BinaryWriter instance</param>
	/// <param name="value">The boolean value to write</param>
	public static void WriteBoolean(this BinaryWriter self, bool value)
	{
		var byteValue = value ? (byte)1 : (byte)0;
		self.Write(byteValue);
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
        var bytes = System.Text.Encoding.UTF8.GetBytes(value);
        Array.Copy(bytes, 0, self, offset, bytes.Length);
    }

    public static void WriteString(this byte[] buffer, string? value, int offset, int maxLength)
    {
        buffer.AsSpan().Slice(offset, maxLength).WriteString(value ?? string.Empty, 0, maxLength);
    }

    public static void WriteBlob(this byte[] buffer, byte[] decodedHash, int offset, int maxLength)
    {
        var copyLength = Math.Min(decodedHash.Length, maxLength);
        var hashBytes = buffer.AsSpan().Slice(offset, maxLength);
        decodedHash.AsSpan()[..copyLength].CopyTo(hashBytes);
    }
}
