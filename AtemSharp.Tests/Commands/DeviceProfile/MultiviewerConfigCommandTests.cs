using AtemSharp.Commands.DeviceProfile;

namespace AtemSharp.Tests.Commands.DeviceProfile;

[TestFixture]
public class MultiviewerConfigCommandTests : DeserializedCommandTestBase<MultiviewerConfigCommand,
    MultiviewerConfigCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int Count { get; set; }
        public int WindowCount { get; set; }

        // TODO: Check how this is deserialized and applied
        public bool CanRouteInputs { get; set; }

        // TODO: Check how this is deserialized and applied
        public bool CanSwapPreviewProgram { get; set; }

        // TODO: Check how this is deserialized and applied
        public bool CanToggleSafeArea { get; set; }

        // TODO: Check how this is deserialized and applied
        public bool SupportsVuMeters { get; set; }

        // TODO: Check how this is deserialized and applied
        public bool SupportsQuadrants { get; set; }

        // TODO: Check how this is deserialized and applied
        public bool CanChangeLayout { get; set; }
    }

    protected override void CompareCommandProperties(MultiviewerConfigCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.Count, Is.EqualTo(expectedData.Count));
        Assert.That(actualCommand.WindowCount, Is.EqualTo(expectedData.WindowCount));
    }
}
