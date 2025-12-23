using AtemSharp.State.Info;

namespace AtemSharp.Commands;

/// <summary>
/// Used to request the current time
/// </summary>
[Command("TiRq", ProtocolVersion.V8_0)]
public class TimeRequestCommand : EmptyCommand;
