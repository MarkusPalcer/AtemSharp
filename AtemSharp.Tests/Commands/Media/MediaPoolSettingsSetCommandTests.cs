using AtemSharp.Commands.Media;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.Media;

public class MediaPoolSettingsSetCommandTests : SerializedCommandTestBase<MediaPoolSettingsSetCommand, MediaPoolSettingsSetCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort[] MaxFrames { get; set; } = [];
    }

    protected override MediaPoolSettingsSetCommand CreateSut(TestCaseData testCase)
    {
        return new MediaPoolSettingsSetCommand(new MediaPoolSettings()
        {
            MaxFrames = testCase.Command.MaxFrames
        });
    }
}
