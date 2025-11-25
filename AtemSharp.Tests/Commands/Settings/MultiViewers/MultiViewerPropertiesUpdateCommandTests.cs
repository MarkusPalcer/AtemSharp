using AtemSharp.Commands.Settings.MultiViewers;
using AtemSharp.Enums;
using AtemSharp.State;
using AtemSharp.State.Info;
using AtemSharp.Tests.TestUtilities;

namespace AtemSharp.Tests.Commands.Settings.MultiViewers;

[TestFixture]
public class MultiViewerPropertiesUpdateCommandTests : DeserializedCommandTestBase<MultiViewerPropertiesUpdateCommand,
    MultiViewerPropertiesUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte MultiviewIndex { get; set; }
        public MultiViewerLayout Layout { get; set; }
        public bool ProgramPreviewSwapped { get; set; }
    }

    [Test]
    public void Deserialize_ShouldHandleProgramPreviewSwappedValues()
    {
        var testCases = new[]
        {
            ((byte)0, false),
            ((byte)1, true),
            ((byte)255, true) // Any non-zero value should be true
        };

        foreach (var (swapByte, expectedSwapped) in testCases)
        {
            // Arrange
            Span<byte> testData = [0, 0, swapByte, 0];

            // Act
            var command = MultiViewerPropertiesUpdateCommand.Deserialize(testData, ProtocolVersion.V8_0)
                                                            .As<MultiViewerPropertiesUpdateCommand>();

            // Assert
            Assert.That(command.ProgramPreviewSwapped, Is.EqualTo(expectedSwapped),
                        $"Byte value {swapByte} should deserialize to {expectedSwapped}");
        }
    }

    [Test]
    public void ApplyToState_ShouldUpdateMultiViewerProperties()
    {
        // Arrange
        var state = new AtemState();
        state.Settings.MultiViewers.ExpandToFit(1);

        var command = new MultiViewerPropertiesUpdateCommand
        {
            MultiViewerId = 1,
            Layout = MultiViewerLayout.TopRightSmall,
            ProgramPreviewSwapped = true
        };

        // Act
        command.ApplyToState(state);

        // Assert
        Assert.That(state.Settings.MultiViewers[1].Properties, Is.Not.Null,
                    "Properties should be created");
        Assert.That(state.Settings.MultiViewers[1].Properties.Layout, Is.EqualTo(MultiViewerLayout.TopRightSmall),
                    "Layout should be updated in state");
        Assert.That(state.Settings.MultiViewers[1].Properties.ProgramPreviewSwapped, Is.True,
                    "ProgramPreviewSwapped should be updated in state");
    }

    protected override void CompareCommandProperties(MultiViewerPropertiesUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.MultiViewerId, Is.EqualTo(expectedData.MultiviewIndex));
        Assert.That(actualCommand.Layout, Is.EqualTo(expectedData.Layout));
        Assert.That(actualCommand.ProgramPreviewSwapped, Is.EqualTo(expectedData.ProgramPreviewSwapped));
    }
}
