using AtemSharp.Commands.DeviceProfile;
using AtemSharp.State;
using AtemSharp.State.Info;
using AtemSharp.Tests.TestUtilities;

namespace AtemSharp.Tests.Commands.DeviceProfile;

internal class MultiviewerConfigCommandTests : DeserializedCommandTestBase<MultiviewerConfigCommand,
    MultiviewerConfigCommandTests.CommandData>
{
    [MaxProtocolVersion(ProtocolVersion.V8_0_1)]
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

    internal override void CompareCommandProperties(MultiviewerConfigCommand actualCommand, CommandData expectedData)
    {
        Assert.That(actualCommand.Count, Is.EqualTo(expectedData.Count));
        Assert.That(actualCommand.WindowCount, Is.EqualTo(expectedData.WindowCount));
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        Assert.That(state.Info.MultiViewer.Count, Is.EqualTo(expectedData.Count));
        Assert.That(state.Info.MultiViewer.WindowCount, Is.EqualTo(expectedData.WindowCount));
    }
}
