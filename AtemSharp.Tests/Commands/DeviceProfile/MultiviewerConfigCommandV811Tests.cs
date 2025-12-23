using AtemSharp.Commands.DeviceProfile;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.DeviceProfile;

internal class MultiviewerConfigCommandV811Tests : DeserializedCommandTestBase<MultiviewerConfigCommandV811,
    MultiviewerConfigCommandV811Tests.CommandData>
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

    internal override void CompareCommandProperties(MultiviewerConfigCommandV811 actualCommand, CommandData expectedData, TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        Assert.That(actualCommand.WindowCount, Is.EqualTo(expectedData.WindowCount));
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        Assert.That(state.Info.MultiViewer.WindowCount, Is.EqualTo(expectedData.WindowCount));
    }
}
