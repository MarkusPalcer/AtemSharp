using AtemSharp.Commands.Recording;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.Recording;

internal class RecordingSettingsUpdateCommandTests : DeserializedCommandTestBase<RecordingSettingsUpdateCommand, RecordingSettingsUpdateCommandTests.CommandData>
{
    public class CommandData: CommandDataBase
    {
        public string Filename { get; set; } = string.Empty;
        public uint WorkingSet1DiskId { get; set; }
        public uint WorkingSet2DiskId { get; set; }
        public bool RecordInAllCameras { get; set; }
    }

    internal override void CompareCommandProperties(RecordingSettingsUpdateCommand actualCommand, CommandData expectedData)
    {
        Assert.That(actualCommand.FileName, Is.EqualTo(expectedData.Filename));
        Assert.That(actualCommand.WorkingSet1DiskId, Is.EqualTo(expectedData.WorkingSet1DiskId));
        Assert.That(actualCommand.WorkingSet2DiskId, Is.EqualTo(expectedData.WorkingSet2DiskId));
        Assert.That(actualCommand.RecordInAllCameras, Is.EqualTo(expectedData.RecordInAllCameras));
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        Assert.That(state.Recording.FileName, Is.EqualTo(expectedData.Filename));
        Assert.That(state.Recording.WorkingSet1DiskId, Is.EqualTo(expectedData.WorkingSet1DiskId));
        Assert.That(state.Recording.WorkingSet2DiskId, Is.EqualTo(expectedData.WorkingSet2DiskId));
        Assert.That(state.Recording.RecordInAllCameras, Is.EqualTo(expectedData.RecordInAllCameras));
    }
}
