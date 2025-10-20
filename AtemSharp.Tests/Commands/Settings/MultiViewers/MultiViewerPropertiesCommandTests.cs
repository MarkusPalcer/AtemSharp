using AtemSharp.Commands.Settings.MultiViewers;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.Settings.MultiViewers;

/// <summary>
/// Tests for MultiViewerPropertiesCommand
/// </summary>
[TestFixture]
public class MultiViewerPropertiesCommandTests
{
    private static AtemState CreateMinimalStateForTesting()
    {
        return new AtemState
        {
            Info = new DeviceInfo
            {
                MultiViewer = new MultiViewerInfo
                {
                    Count = 2,
                    WindowCount = 10
                }
            },
            Settings = new SettingsState
            {
                MultiViewers = new Dictionary<int, MultiViewer>
                {
                    [0] = new(0) { Properties = new MultiViewerPropertiesState { Layout = MultiViewerLayout.Default, ProgramPreviewSwapped = false } },
                    [1] = new(1) { Properties = new MultiViewerPropertiesState { Layout = MultiViewerLayout.TopLeftSmall, ProgramPreviewSwapped = true } }
                }
            }
        };
    }

    [Test]
    public void Constructor_ShouldInitializeFromCurrentState()
    {
        // Arrange
        var state = CreateMinimalStateForTesting();
        
        // Act
        var command = new MultiViewerPropertiesCommand(0, state);
        
        // Assert
        Assert.That(command.MultiViewerId, Is.EqualTo(0), "MultiViewer ID should be set correctly");
        Assert.That(command.Layout, Is.EqualTo(MultiViewerLayout.Default), "Layout should be initialized from state");
        Assert.That(command.ProgramPreviewSwapped, Is.False, "ProgramPreviewSwapped should be initialized from state");
    }

    [Test]
    public void Constructor_ShouldThrowExceptionForInvalidMultiViewerId()
    {
        // Arrange
        var state = CreateMinimalStateForTesting();
        
        // Act & Assert
        Assert.Throws<InvalidIdError>(() => _ = new MultiViewerPropertiesCommand(5, state), 
                                     "Should throw exception for invalid MultiViewer ID");
    }

    [Test]
    public void Constructor_ShouldUseDefaultsWhenNoExistingProperties()
    {
        // Arrange
        var state = new AtemState
        {
            Info = new DeviceInfo
            {
                MultiViewer = new MultiViewerInfo { Count = 1, WindowCount = 10 }
            },
            Settings = new SettingsState
            {
                MultiViewers = new Dictionary<int, MultiViewer>
                {
                    [0] = new(0)
                }
            }
        };
        
        // Act
        var command = new MultiViewerPropertiesCommand(0, state);
        
        // Assert
        Assert.That(command.Layout, Is.EqualTo(MultiViewerLayout.Default), "Layout should default to Default");
        Assert.That(command.ProgramPreviewSwapped, Is.False, "ProgramPreviewSwapped should default to false");
    }

    [Test]
    public void SetLayout_ShouldSetFlagAutomatically()
    {
        // Arrange
        var state = CreateMinimalStateForTesting();
        var command = new MultiViewerPropertiesCommand(0, state);
        
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
        var state = CreateMinimalStateForTesting();
        var command = new MultiViewerPropertiesCommand(0, state);
        
        // Act
        command.ProgramPreviewSwapped = true;
        
        // Assert
        Assert.That(command.ProgramPreviewSwapped, Is.True, "ProgramPreviewSwapped should be updated");
        Assert.That((command.Flag & (1 << 1)) != 0, Is.True, "ProgramPreviewSwapped flag should be set automatically");
    }

    [Test]
    public void Serialize_ShouldWriteCorrectData()
    {
        // Arrange
        var state = CreateMinimalStateForTesting();
        var command = new MultiViewerPropertiesCommand(1, state);
        command.Layout = MultiViewerLayout.ProgramBottom;
        command.ProgramPreviewSwapped = true;
        
        // Act
        var result = command.Serialize(ProtocolVersion.V8_0);
        
        // Assert
        Assert.That(result.Length, Is.EqualTo(4), "Serialized command should be 4 bytes");
        Assert.That(result[0], Is.EqualTo(3), "First byte should be the flag (both flags set: 1 | 2 = 3)");
        Assert.That(result[1], Is.EqualTo(1), "Second byte should be the MultiViewer ID");
        Assert.That(result[2], Is.EqualTo((byte)MultiViewerLayout.ProgramBottom), "Third byte should be the layout value");
        Assert.That(result[3], Is.EqualTo(1), "Fourth byte should be 1 for programPreviewSwapped=true");
    }

    [Test]
    public void Serialize_ShouldHandleFalseValues()
    {
        // Arrange
        var state = CreateMinimalStateForTesting();
        var command = new MultiViewerPropertiesCommand(0, state);
        command.Layout = MultiViewerLayout.Default;
        command.ProgramPreviewSwapped = false;
        
        // Act
        var result = command.Serialize(ProtocolVersion.V8_0);
        
        // Assert
        Assert.That(result.Length, Is.EqualTo(4), "Serialized command should be 4 bytes");
        Assert.That(result[0], Is.EqualTo(3), "First byte should be the flag (both flags set: 1 | 2 = 3)");
        Assert.That(result[1], Is.EqualTo(0), "Second byte should be the MultiViewer ID");
        Assert.That(result[2], Is.EqualTo((byte)MultiViewerLayout.Default), "Third byte should be the layout value");
        Assert.That(result[3], Is.EqualTo(0), "Fourth byte should be 0 for programPreviewSwapped=false");
    }

    [Test]
    public void Serialize_ShouldHandleVariousLayoutValues()
    {
        var testCases = new[]
        {
            (MultiViewerLayout.Default, (byte)0),
            (MultiViewerLayout.TopLeftSmall, (byte)1),
            (MultiViewerLayout.TopRightSmall, (byte)2),
            (MultiViewerLayout.ProgramBottom, (byte)3),
            (MultiViewerLayout.BottomLeftSmall, (byte)4),
            (MultiViewerLayout.ProgramRight, (byte)5),
            (MultiViewerLayout.BottomRightSmall, (byte)8),
            (MultiViewerLayout.ProgramLeft, (byte)10),
            (MultiViewerLayout.ProgramTop, (byte)12)
        };

        var state = CreateMinimalStateForTesting();

        foreach (var (layout, expectedByte) in testCases)
        {
            // Arrange
            var command = new MultiViewerPropertiesCommand(0, state);
            command.Layout = layout;
            
            // Act
            var result = command.Serialize(ProtocolVersion.V8_0);
            
            // Assert
            Assert.That(result[2], Is.EqualTo(expectedByte), 
                       $"Layout {layout} should serialize to byte value {expectedByte}");
        }
    }
}