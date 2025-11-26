using AtemSharp.Commands.Settings.MultiViewers;
using AtemSharp.State.Settings.MultiViewer;

namespace AtemSharp.Tests.Commands.Settings.MultiViewers;

/// <summary>
/// Tests for MultiViewerPropertiesCommand
/// </summary>
[TestFixture]
public class MultiViewerPropertiesCommandTests : SerializedCommandTestBase<MultiViewerPropertiesCommand,
    MultiViewerPropertiesCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte MultiViewIndex { get; set; }
        public MultiViewerLayout Layout { get; set; }
        public bool ProgramPreviewSwapped { get; set; }
    }

    protected override MultiViewerPropertiesCommand CreateSut(TestCaseData testCase)
    {
        return new MultiViewerPropertiesCommand(new MultiViewer
        {
            Id = testCase.Command.MultiViewIndex,
            Properties =
            {
                Layout = testCase.Command.Layout,
                ProgramPreviewSwapped = testCase.Command.ProgramPreviewSwapped
            }
        });
    }

    [Test]
    public void SetLayout_ShouldSetFlagAutomatically()
    {
        // Arrange
        var command = new MultiViewerPropertiesCommand(new MultiViewer());

        // Act
        command.Layout = MultiViewerLayout.TopRightSmall;

        // Assert
        Assert.That((command.Flag & (1 << 0)) != 0, Is.True, "Layout flag should be set automatically");
    }

    [Test]
    public void SetProgramPreviewSwapped_ShouldSetFlagAutomatically()
    {
        // Arrange
        var command = new MultiViewerPropertiesCommand(new MultiViewer());

        // Act
        command.ProgramPreviewSwapped = true;

        // Assert
        Assert.That((command.Flag & (1 << 1)) != 0, Is.True, "ProgramPreviewSwapped flag should be set automatically");
    }
}
