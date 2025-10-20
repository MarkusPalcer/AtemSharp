namespace AtemSharp.Enums;

/// <summary>
/// Display clock state (stopped, running, or reset)
/// </summary>
public enum DisplayClockClockState : byte
{
	/// <summary>
	/// Clock is stopped
	/// </summary>
	Stopped = 0,

	/// <summary>
	/// Clock is running
	/// </summary>
	Running = 1,

	/// <summary>
	/// Clock is reset
	/// </summary>
	Reset = 2
}