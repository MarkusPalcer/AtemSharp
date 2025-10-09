namespace AtemSharp.Commands;

public static class SerializationExtensions
{
	/// <summary>
	/// Add padding bytes to the given stream
	/// </summary>
	public static void Pad(this BinaryWriter self, uint length)
	{
		for (var i = 0; i < length; i++)
		{
			self.Write((byte)0);
		}
	}
	
	public static void WriteUInt16(this BinaryWriter self, ushort value)
	{
		self.Write((byte)(value >> 8));
		self.Write((byte)(value & 0xFF));
	}

	public static void WriteInt16(this BinaryWriter self, short value)
	{
		self.Write((byte)(value >> 8));
		self.Write((byte)(value & 0xFF));
	}
	
	public static ushort ReadUInt16(this BinaryReader self)
	{
		var high = self.ReadByte();
		var low = self.ReadByte();
		return (ushort)((high << 8) | low);
	}
	
	public static short ReadInt16(this BinaryReader self)
	{
		var high = self.ReadByte();
		var low = self.ReadByte();
		return (short)((high << 8) | low);
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
}