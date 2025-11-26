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

        public bool CanRouteInputs { get; set; }

        public bool CanSwapPreviewProgram { get; set; }

        public bool CanToggleSafeArea { get; set; }

        public bool SupportsVuMeters { get; set; }

        public bool SupportsQuadrants { get; set; }

        public bool CanChangeLayout { get; set; }
    }

    protected override void CompareCommandProperties(MultiviewerConfigCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.Count, Is.EqualTo(expectedData.Count));
        Assert.That(actualCommand.WindowCount, Is.EqualTo(expectedData.WindowCount));
    }
}
