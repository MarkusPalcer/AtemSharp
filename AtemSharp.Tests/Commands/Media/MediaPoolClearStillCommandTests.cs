using AtemSharp.Commands.Media;
using AtemSharp.State.Media;

namespace AtemSharp.Tests.Commands.Media;

public class MediaPoolClearStillCommandTests : SerializedCommandTestBase<MediaPoolClearStillCommand, MediaPoolClearStillCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
    }

    protected override MediaPoolClearStillCommand CreateSut(TestCaseData testCase)
    {
        return new MediaPoolClearStillCommand(new Still { Id = testCase.Command.Index });
    }
}
