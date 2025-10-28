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
    /// Convert coordinate value to signed 16-bit integer
    /// </summary>
    /// <param name="input">Coordinate value</param>
    /// <returns>Signed 16-bit integer value</returns>
    public static short CoordinateToInt16(this double input)
    {
        return (short)Math.Round(input * 1000);
    }

    /// <summary>
    /// Convert signed 16-bit integer to coordinate value
    /// </summary>
    /// <param name="input">Signed 16-bit integer value</param>
    /// <returns>Coordinate value</returns>
    public static double Int16ToCoordinate(this short input)
    {
        return input / 1000.0;
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

        for (int next = 1; next <= intValue; next <<= 1)
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

    /// <summary>
    /// Combine individual flag components into a single value
    /// </summary>
    /// <param name="values">Array of flag values to combine</param>
    /// <returns>Combined flag value</returns>
    public static T CombineComponents<T>(T[] values) where T : Enum
    {
        int result = 0;
        foreach (var value in values)
        {
            result |= Convert.ToInt32(value);
        }
        return (T)Enum.ToObject(typeof(T), result);
    }

    /// <summary>
    /// Write a string to a byte array with null termination and padding
    /// </summary>
    /// <param name="text">String to write</param>
    /// <param name="buffer">Target byte array</param>
    /// <param name="startIndex">Starting index in the buffer</param>
    /// <param name="maxLength">Maximum length to write (including null terminator)</param>
    public static void WriteNullTerminatedString(string text, byte[] buffer, int startIndex, int maxLength)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(text);
        var lengthToCopy = Math.Min(bytes.Length, maxLength - 1); // Leave space for null terminator

        // Copy string bytes
        Array.Copy(bytes, 0, buffer, startIndex, lengthToCopy);

        // Fill remaining space with zeros (including null terminator)
        for (int i = startIndex + lengthToCopy; i < startIndex + maxLength; i++)
        {
            buffer[i] = 0;
        }
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
