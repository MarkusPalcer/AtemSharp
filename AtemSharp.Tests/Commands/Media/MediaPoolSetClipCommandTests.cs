using AtemSharp.Commands.Media;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.Media;

public class MediaPoolSetClipCommandTests : SerializedCommandTestBase<MediaPoolSetClipCommand, MediaPoolSetClipCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte B0 { get; set; }
        public byte Index { get; set; }
        public string Name { get; set; } = string.Empty;
        public ushort Frames { get; set; }
    }

    protected override MediaPoolSetClipCommand CreateSut(TestCaseData testCase)
    {
        return new MediaPoolSetClipCommand(new MediaPoolEntry()
        {
            Id = testCase.Command.Index,
            Name = testCase.Command.Name,
            FrameCount = testCase.Command.Frames,
        });
    }
}
