using System.Text;

namespace AtemSharp.Lib;

/// <summary>
/// Utility functions for ATEM data conversions
/// </summary>
public static class AtemUtil
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
    /// Convert decibel value to UInt16 big-endian
    /// </summary>
    /// <param name="input">Decibel value</param>
    /// <returns>UInt16 value</returns>
    public static ushort DecibelToUInt16(this double input)
    {
        return (ushort)Math.Floor(Math.Pow(10, input / 20) * 32768);
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
    /// Convert balance value to integer
    /// </summary>
    /// <param name="input">Balance value (-50 to +50)</param>
    /// <returns>Integer value</returns>
    public static short BalanceToInt16(this double input)
    {
        return (short)Math.Round(input * 200);
    }

    /// <summary>
    /// Extract a null-terminated string from a byte buffer at the specified position and length
    /// </summary>
    /// <param name="buffer">Byte buffer containing the string data</param>
    /// <param name="offset">Starting position in the buffer</param>
    /// <param name="length">Maximum length to read</param>
    /// <returns>Extracted string</returns>
    public static string BufferToNullTerminatedString(byte[] buffer, int offset, int length)
    {
        // Find the null terminator or use the full length
        var endIndex = offset + length;
        var nullIndex = Array.IndexOf(buffer, (byte)0, offset, length);
        if (nullIndex >= 0)
        {
            endIndex = nullIndex;
        }

        // Extract the string from offset to endIndex
        var stringLength = endIndex - offset;
        return stringLength > 0 ? Encoding.UTF8.GetString(buffer, offset, stringLength) : string.Empty;
    }
}