using AtemSharp.Communication;
using AtemSharp.Constants;

/*
 * This program shows a list of all commands
 * and their payload in a packet that has been
 * sniffed with Wireshark
 */

Console.WriteLine("Paste packet hex or write exit to close");

while (true)
{
    Console.Write(">");
    var line = Console.ReadLine()?.ToLowerInvariant();
    if (line is null or "exit")
    {
        return;
    }

    var bytes = Convert.FromHexString(line);

    var packet = AtemPacket.FromBytes(bytes);

    Console.WriteLine($"Flags: {packet.Flags}");
    var payload = packet.Payload;
        var offset = 0;

        // Parse all commands in the packet payload
        while (offset + AtemConstants.CommandHeaderSize <= payload.Length)
        {
            // Extract command header (8 bytes: length, reserved, rawName)
            var commandLength = (payload[offset] << 8) | payload[offset + 1]; // Big-endian 16-bit
            // Skip reserved bytes (offset + 2, offset + 3)
            var rawName = System.Text.Encoding.ASCII.GetString(payload, offset + 4, 4);

            // Validate command length
            if (commandLength < AtemConstants.CommandHeaderSize)
            {
                // Commands are never less than 8 bytes (header size)
                break;
            }

            if (offset + commandLength > payload.Length)
            {
                // Command extends beyond payload - malformed packet
                break;
            }

            // Extract command data (excluding the 8-byte header)
            var commandDataStart = offset + AtemConstants.CommandHeaderSize;
            var commandDataLength = commandLength - AtemConstants.CommandHeaderSize;
            var commandData = new Span<byte>(payload, commandDataStart, commandDataLength);

            Console.WriteLine($"Command: {rawName} - {BitConverter.ToString(commandData.ToArray())}");
            offset += commandLength;
        }
}
