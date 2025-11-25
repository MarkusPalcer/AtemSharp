using AtemSharp.Commands.Media;
using AtemSharp.State.Media;

namespace AtemSharp.Tests.Commands.Media;

public class MediaPoolClearClipCommandTests : SerializedCommandTestBase<MediaPoolClearClipCommand, MediaPoolClearClipCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
    }

    protected override MediaPoolClearClipCommand CreateSut(TestCaseData testCase)
    {
        return new MediaPoolClearClipCommand(new MediaPoolEntry { Id = testCase.Command.Index });
    }
}
