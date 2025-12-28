using AtemSharp.State.Settings;

namespace AtemSharp.Extensions;

public static class VideoModeExtensions
{
    public static ushort FramesPerSecond(this VideoMode videoMode)
    {
        return videoMode switch
        {
            VideoMode.N525i5994NTSC => 60,
            VideoMode.P625i50PAL => 50,
            VideoMode.N525i5994169 => 60,
            VideoMode.P625i50169 => 50,
            VideoMode.P720p50 => 50,
            VideoMode.N720p5994 => 60,
            VideoMode.P1080i50 => 50,
            VideoMode.N1080i5994 => 60,
            VideoMode.N1080p2398 => 24,
            VideoMode.N1080p24 => 24,
            VideoMode.P1080p25 => 25,
            VideoMode.N1080p2997 => 30,
            VideoMode.P1080p50 => 50,
            VideoMode.N1080p5994 => 60,
            VideoMode.N4KHDp2398 => 24,
            VideoMode.N4KHDp24 => 24,
            VideoMode.P4KHDp25 => 25,
            VideoMode.N4KHDp2997 => 30,
            VideoMode.P4KHDp5000 => 50,
            VideoMode.N4KHDp5994 => 60,
            VideoMode.N8KHDp2398 => 24,
            VideoMode.N8KHDp24 => 24,
            VideoMode.P8KHDp25 => 25,
            VideoMode.N8KHDp2997 => 30,
            VideoMode.P8KHDp50 => 50,
            VideoMode.N8KHDp5994 => 60,
            VideoMode.N1080p30 => 30,
            VideoMode.N1080p60 => 60,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
