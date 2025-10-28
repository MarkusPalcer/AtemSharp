using AtemSharp.Commands.Recording;

namespace AtemSharp.Tests.Commands.Recording;

public class RecordingSettingsUpdateCommandTests : DeserializedCommandTestBase<RecordingSettingsUpdateCommand, RecordingSettingsUpdateCommandTests.CommandData>
{
    public class CommandData: CommandDataBase
    {
        public string Filename { get; set; } = string.Empty;
        public uint WorkingSet1DiskId { get; set; }
        public uint WorkingSet2DiskId { get; set; }
        public bool RecordInAllCameras { get; set; }
    }

    protected override void CompareCommandProperties(RecordingSettingsUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.FileName, Is.EqualTo(expectedData.Filename));
        Assert.That(actualCommand.WorkingSet1DiskId, Is.EqualTo(expectedData.WorkingSet1DiskId));
        Assert.That(actualCommand.WorkingSet2DiskId, Is.EqualTo(expectedData.WorkingSet2DiskId));
        Assert.That(actualCommand.RecordInAllCameras, Is.EqualTo(expectedData.RecordInAllCameras));
    }
}
