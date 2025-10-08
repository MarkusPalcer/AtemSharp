using AtemSharp.Commands;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects;

/// <summary>
/// Input source properties
/// </summary>
public record InputSource(int Source);

/// <summary>
/// Auto transition command for performing an automatic transition
/// </summary>
public class AutoTransitionCommand : BasicWritableCommand<object?>
{
    public static new string RawName { get; } = "DAut";

    public int MixEffect { get; }

    public AutoTransitionCommand(int mixEffect) : base(null)
    {
        MixEffect = mixEffect;
    }

    public override byte[] Serialize(ProtocolVersion version)
    {
        var buffer = new byte[4];
        buffer[0] = (byte)MixEffect;
        return buffer;
    }
}

/// <summary>
/// Program input command for setting the program source
/// </summary>
public class ProgramInputCommand : BasicWritableCommand<InputSource>
{
    public static new string RawName { get; } = "CPgI";

    public int MixEffect { get; }

    public ProgramInputCommand(int mixEffect, int source) : base(new InputSource(source))
    {
        MixEffect = mixEffect;
    }

    public override byte[] Serialize(ProtocolVersion version)
    {
        var buffer = new byte[4];
        buffer[0] = (byte)MixEffect;
        WriteUInt16BE(buffer, 2, (ushort)Properties.Source);
        return buffer;
    }

    private static void WriteUInt16BE(byte[] buffer, int offset, ushort value)
    {
        buffer[offset] = (byte)(value >> 8);
        buffer[offset + 1] = (byte)(value & 0xFF);
    }
}

/// <summary>
/// Program input update command (received from ATEM)
/// </summary>
public class ProgramInputUpdateCommand : DeserializedCommand<InputSource>
{
    public static new string RawName { get; } = "PrgI";

    public int MixEffect { get; }

    public ProgramInputUpdateCommand(int mixEffect, InputSource properties) : base(properties)
    {
        MixEffect = mixEffect;
    }

    public static ProgramInputUpdateCommand Deserialize(byte[] rawCommand)
    {
        var mixEffect = rawCommand[0];
        var source = ReadUInt16BE(rawCommand, 2);
        return new ProgramInputUpdateCommand(mixEffect, new InputSource(source));
    }

    public override string[] ApplyToState(AtemState state)
    {
        if (state.Info.Capabilities == null || MixEffect >= state.Info.Capabilities.MixEffects)
            throw new InvalidIdError("MixEffect", MixEffect);

        var mixEffect = AtemStateUtil.GetMixEffect(state, MixEffect);
        mixEffect.ProgramInput = Properties.Source;
        return new[] { $"video.mixEffects.{MixEffect}.programInput" };
    }

    private static int ReadUInt16BE(byte[] buffer, int offset)
    {
        return (buffer[offset] << 8) | buffer[offset + 1];
    }
}

/// <summary>
/// Preview input command for setting the preview source
/// </summary>
public class PreviewInputCommand : BasicWritableCommand<InputSource>
{
    public static new string RawName { get; } = "CPvI";

    public int MixEffect { get; }

    public PreviewInputCommand(int mixEffect, int source) : base(new InputSource(source))
    {
        MixEffect = mixEffect;
    }

    public override byte[] Serialize(ProtocolVersion version)
    {
        var buffer = new byte[4];
        buffer[0] = (byte)MixEffect;
        WriteUInt16BE(buffer, 2, (ushort)Properties.Source);
        return buffer;
    }

    private static void WriteUInt16BE(byte[] buffer, int offset, ushort value)
    {
        buffer[offset] = (byte)(value >> 8);
        buffer[offset + 1] = (byte)(value & 0xFF);
    }
}

/// <summary>
/// Preview input update command (received from ATEM)
/// </summary>
public class PreviewInputUpdateCommand : DeserializedCommand<InputSource>
{
    public static new string RawName { get; } = "PrvI";

    public int MixEffect { get; }

    public PreviewInputUpdateCommand(int mixEffect, InputSource properties) : base(properties)
    {
        MixEffect = mixEffect;
    }

    public static PreviewInputUpdateCommand Deserialize(byte[] rawCommand)
    {
        var mixEffect = rawCommand[0];
        var source = ReadUInt16BE(rawCommand, 2);
        return new PreviewInputUpdateCommand(mixEffect, new InputSource(source));
    }

    public override string[] ApplyToState(AtemState state)
    {
        if (state.Info.Capabilities == null || MixEffect >= state.Info.Capabilities.MixEffects)
            throw new InvalidIdError("MixEffect", MixEffect);

        var mixEffect = AtemStateUtil.GetMixEffect(state, MixEffect);
        mixEffect.PreviewInput = Properties.Source;
        return new[] { $"video.mixEffects.{MixEffect}.previewInput" };
    }

    private static int ReadUInt16BE(byte[] buffer, int offset)
    {
        return (buffer[offset] << 8) | buffer[offset + 1];
    }
}