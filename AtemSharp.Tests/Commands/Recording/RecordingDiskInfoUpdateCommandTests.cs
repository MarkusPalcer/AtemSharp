using AtemSharp.Commands.Recording;
using AtemSharp.State;
using AtemSharp.State.Recording;

namespace AtemSharp.Tests.Commands.Recording;

public class RecordingDiskInfoUpdateCommandTests : DeserializedCommandTestBase<RecordingDiskInfoUpdateCommand, RecordingDiskInfoUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public uint DiskId { get; set; }
        public bool IsDelete { get; set; }
        public RecordingDiskStatus Status { get; set; }
        public uint RecordingTimeAvailable { get; set; }
        public string VolumeName { get; set; } = string.Empty;
    }

    protected override void CompareCommandProperties(RecordingDiskInfoUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.DiskId, Is.EqualTo(expectedData.DiskId));
        Assert.That(actualCommand.IsDelete, Is.EqualTo(expectedData.IsDelete));
        Assert.That(actualCommand.Status, Is.EqualTo(expectedData.Status));
        Assert.That(actualCommand.RecordingTimeAvailable, Is.EqualTo(expectedData.RecordingTimeAvailable));
        Assert.That(actualCommand.Name, Is.EqualTo(expectedData.VolumeName));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        // If we test deletion, create a disk to be deleted for the test
        if (expectedData.IsDelete)
        {
            state.Recording.Disks.GetOrCreate(expectedData.DiskId);
        }
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        if (expectedData.IsDelete)
        {
            Assert.That(state.Recording.Disks, Does.Not.ContainKey(expectedData.DiskId));
        }
        else
        {
            Assert.That(state.Recording.Disks[expectedData.DiskId].DiskId, Is.EqualTo(expectedData.DiskId));
            Assert.That(state.Recording.Disks[expectedData.DiskId].Status, Is.EqualTo(expectedData.Status));
            Assert.That(state.Recording.Disks[expectedData.DiskId].RecordingTimeAvailable, Is.EqualTo(expectedData.RecordingTimeAvailable));
            Assert.That(state.Recording.Disks[expectedData.DiskId].Name, Is.EqualTo(expectedData.VolumeName));
        }
    }
}
