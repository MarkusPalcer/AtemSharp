namespace AtemSharp.Commands;

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

	/// <summary>
	/// Read a 16-bit unsigned integer from the stream in big-endian byte order
	/// </summary>
	/// <param name="self">The BinaryReader instance to read from</param>
	/// <returns>The 16-bit unsigned integer value read from the stream</returns>
	public static ushort ReadUInt16BigEndian(this BinaryReader self)
	{
		var high = self.ReadByte();
		var low = self.ReadByte();
		return (ushort)((high << 8) | low);
	}

	/// <summary>
	/// Read a 16-bit signed integer from the stream in big-endian byte order
	/// </summary>
	/// <param name="self">The BinaryReader instance to read from</param>
	/// <returns>The 16-bit signed integer value read from the stream</returns>
	public static short ReadInt16BigEndian(this BinaryReader self)
	{
		var high = self.ReadByte();
		var low = self.ReadByte();
		return (short)((high << 8) | low);
	}

	/// <summary>
	/// Read a 32-bit unsigned integer from the stream in big-endian byte order
	/// </summary>
	/// <param name="self">The BinaryReader instance to read from</param>
	/// <returns>The 32-bit unsigned integer value read from the stream</returns>
	public static uint ReadUInt32BigEndian(this BinaryReader self)
	{
		var byte1 = self.ReadByte();
		var byte2 = self.ReadByte();
		var byte3 = self.ReadByte();
		var byte4 = self.ReadByte();
		return (uint)((byte1 << 24) | (byte2 << 16) | (byte3 << 8) | byte4);
	}

	/// <summary>
	/// Read a 32-bit signed integer from the stream in big-endian byte order
	/// </summary>
	/// <param name="self">The BinaryReader instance to read from</param>
	/// <returns>The 32-bit signed integer value read from the stream</returns>
	public static int ReadInt32BigEndian(this BinaryReader self)
	{
		var byte1 = self.ReadByte();
		var byte2 = self.ReadByte();
		var byte3 = self.ReadByte();
		var byte4 = self.ReadByte();
		return (int)((byte1 << 24) | (byte2 << 16) | (byte3 << 8) | byte4);
	}
	
	// ReadBoolean is already defined on the BinaryReader class
}