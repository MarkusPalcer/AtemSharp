using System;

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
    public static double UInt16ToDecibel(ushort input)
    {
        // 0 = -inf, 32768 = 0, 65381 = +6db
        return Math.Round(Math.Log10(input / 32768.0) * 20 * 100) / 100;
    }

    /// <summary>
    /// Convert decibel value to UInt16 big-endian
    /// </summary>
    /// <param name="input">Decibel value</param>
    /// <returns>UInt16 value</returns>
    public static ushort DecibelToUInt16(double input)
    {
        return (ushort)Math.Floor(Math.Pow(10, input / 20) * 32768);
    }

    /// <summary>
    /// Convert integer value to balance
    /// </summary>
    /// <param name="input">Integer value</param>
    /// <returns>Balance value (-50 to +50)</returns>
    public static double Int16ToBalance(short input)
    {
        // -100000 = -50, 0x0000 = 0, 0x2710 = +50
        return input / 200.0;
    }

    /// <summary>
    /// Convert balance value to integer
    /// </summary>
    /// <param name="input">Balance value (-50 to +50)</param>
    /// <returns>Integer value</returns>
    public static short BalanceToInt16(double input)
    {
        return (short)Math.Round(input * 200);
    }

    /// <summary>
    /// Pad value to multiple
    /// </summary>
    /// <param name="val">Value to pad</param>
    /// <param name="multiple">Multiple to pad to</param>
    /// <returns>Padded value</returns>
    public static int PadToMultiple(int val, int multiple)
    {
        var r = val % multiple;
        if (r == 0)
        {
            return val;
        }
        else
        {
            return val + (multiple - r);
        }
    }
}