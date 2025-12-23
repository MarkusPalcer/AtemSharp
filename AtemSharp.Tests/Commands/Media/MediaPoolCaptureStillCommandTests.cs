using AtemSharp.Commands.Media;

namespace AtemSharp.Tests.Commands.Media;

public class MediaPoolCaptureStillCommandTests : SerializedCommandTestBase<MediaPoolCaptureStillCommand, MediaPoolCaptureStillCommandTests.CommandData>
{
    public class CommandData : CommandDataBase;

    protected override MediaPoolCaptureStillCommand CreateSut(TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        return new MediaPoolCaptureStillCommand();
    }
}
