using AtemSharp.Commands.Recording;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.Recording;

public class RecordingSettingsCommandTests : SerializedCommandTestBase<RecordingSettingsCommand, RecordingSettingsCommandTests.CommandData>
{
    public class CommandData: CommandDataBase
    {
        public string Filename { get; set; } = string.Empty;
        public uint WorkingSet1DiskId { get; set; }
        public uint WorkingSet2DiskId { get; set; }
        public bool RecordInAllCameras { get; set; }
    }

    protected override RecordingSettingsCommand CreateSut(TestCaseData testCase)
    {
        return new RecordingSettingsCommand(new AtemState
        {
            Recording =
            {
                FileName = testCase.Command.Filename,
                WorkingSet1DiskId = testCase.Command.WorkingSet1DiskId,
                WorkingSet2DiskId = testCase.Command.WorkingSet2DiskId,
                RecordInAllCameras = testCase.Command.RecordInAllCameras
            }
        });
    }
}
