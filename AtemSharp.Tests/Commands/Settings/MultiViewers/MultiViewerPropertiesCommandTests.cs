using AtemSharp.Commands.Settings.MultiViewers;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.Settings.MultiViewers;

/// <summary>
/// Tests for MultiViewerPropertiesCommand
/// </summary>
[TestFixture]
public class MultiViewerPropertiesCommandTests : SerializedCommandTestBase<MultiViewerPropertiesCommand, MultiViewerPropertiesCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte MultiViewIndex { get; set; }
        public MultiViewerLayout Layout { get; set; }
        public bool ProgramPreviewSwapped { get; set; }
    }

    [Test]
    public void Constructor_ShouldInitializeFromCurrentState()
    {
        // Arrange
        var state = new MultiViewer
        {
            Index = 0,
            Properties =
            {
                Layout = MultiViewerLayout.Default,
                ProgramPreviewSwapped = false
            }
        };

        // Act
        var command = new MultiViewerPropertiesCommand(state);

        // Assert
        Assert.That(command.MultiViewerId, Is.EqualTo(0), "MultiViewer ID should be set correctly");
        Assert.That(command.Layout, Is.EqualTo(MultiViewerLayout.Default), "Layout should be initialized from state");
        Assert.That(command.ProgramPreviewSwapped, Is.False, "ProgramPreviewSwapped should be initialized from state");
    }

    [Test]
    public void SetLayout_ShouldSetFlagAutomatically()
    {
        // Arrange
        var command = new MultiViewerPropertiesCommand(new MultiViewer());

        // Act
        command.Layout = MultiViewerLayout.TopRightSmall;

        // Assert
        Assert.That(command.Layout, Is.EqualTo(MultiViewerLayout.TopRightSmall), "Layout should be updated");
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
        Assert.That(command.ProgramPreviewSwapped, Is.True, "ProgramPreviewSwapped should be updated");
        Assert.That((command.Flag & (1 << 1)) != 0, Is.True, "ProgramPreviewSwapped flag should be set automatically");
    }

    protected override MultiViewerPropertiesCommand CreateSut(TestCaseData testCase)
    {
        return new MultiViewerPropertiesCommand(new MultiViewer()
        {
            Index = testCase.Command.MultiViewIndex,
            Properties =
            {
                Layout = testCase.Command.Layout,
                ProgramPreviewSwapped = testCase.Command.ProgramPreviewSwapped
            }
        });
    }
}
