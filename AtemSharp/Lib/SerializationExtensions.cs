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

    public static void WriteUInt16BigEndian(this byte[] self, double value, int offset)
    {
        BinaryPrimitives.WriteUInt16BigEndian(self.AsSpan(offset), (ushort)value);
    }
}
