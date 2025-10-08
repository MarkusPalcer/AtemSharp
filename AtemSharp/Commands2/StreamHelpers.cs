namespace AtemSharp.Commands2;

public static class StreamHelpers
{
	public static void WriteUInt16BE(this BinaryWriter self, ushort value)
	{
		self.Write((byte)(value >> 8));
		self.Write((byte)(value & 0xFF));
	}

	public static void WriteInt16BE(this BinaryWriter self, short value)
	{
		self.Write((byte)(value >> 8));
		self.Write((byte)(value & 0xFF));
	}
	
	public static ushort ReadUInt16BE(this BinaryReader self)
	{
		var high = self.ReadByte();
		var low = self.ReadByte();
		return (ushort)((high << 8) | low);
	}
}