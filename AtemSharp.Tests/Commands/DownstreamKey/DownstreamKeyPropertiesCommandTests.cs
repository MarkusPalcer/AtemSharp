using AtemSharp.Commands.DownstreamKey;
using AtemSharp.State;
using AtemSharp.Tests.TestUtilities;

namespace AtemSharp.Tests.Commands.DownstreamKey;

[TestFixture]
public class DownstreamKeyPropertiesCommandTests : DeserializedCommandTestBase<DownstreamKeyPropertiesCommand, DownstreamKeyPropertiesCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int Index { get; set; }
        public bool Tie { get; set; }
        public int Rate { get; set; }
        public bool PreMultipliedKey { get; set; }
        public double Clip { get; set; }
        public double Gain { get; set; }
        public bool Invert { get; set; }
        public bool MaskEnabled { get; set; }
        public double MaskTop { get; set; }
        public double MaskBottom { get; set; }
        public double MaskLeft { get; set; }
        public double MaskRight { get; set; }
    }

    protected override void CompareCommandProperties(DownstreamKeyPropertiesCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        var failures = new List<string>();

        // Compare downstream keyer index
        if (actualCommand.DownstreamKeyerId != expectedData.Index)
        {
            failures.Add($"DownstreamKeyerId: expected {expectedData.Index}, actual {actualCommand.DownstreamKeyerId}");
        }

        // Compare Tie property
        if (actualCommand.Properties.Tie != expectedData.Tie)
        {
            failures.Add($"Tie: expected {expectedData.Tie}, actual {actualCommand.Properties.Tie}");
        }

        // Compare Rate property
        if (actualCommand.Properties.Rate != expectedData.Rate)
        {
            failures.Add($"Rate: expected {expectedData.Rate}, actual {actualCommand.Properties.Rate}");
        }

        // Compare PreMultiply property (note: test data uses "PreMultipliedKey")
        if (actualCommand.Properties.PreMultiply != expectedData.PreMultipliedKey)
        {
            failures.Add($"PreMultiply: expected {expectedData.PreMultipliedKey}, actual {actualCommand.Properties.PreMultiply}");
        }

        // Compare Clip property (with 1 decimal place precision)
        if (!Utilities.AreApproximatelyEqual(actualCommand.Properties.Clip, expectedData.Clip, 1))
        {
            failures.Add($"Clip: expected {expectedData.Clip}, actual {actualCommand.Properties.Clip}");
        }

        // Compare Gain property (with 1 decimal place precision)
        if (!Utilities.AreApproximatelyEqual(actualCommand.Properties.Gain, expectedData.Gain, 1))
        {
            failures.Add($"Gain: expected {expectedData.Gain}, actual {actualCommand.Properties.Gain}");
        }

        // Compare Invert property
        if (actualCommand.Properties.Invert != expectedData.Invert)
        {
            failures.Add($"Invert: expected {expectedData.Invert}, actual {actualCommand.Properties.Invert}");
        }

        // Compare Mask Enabled property
        if (actualCommand.Properties.Mask.Enabled != expectedData.MaskEnabled)
        {
            failures.Add($"Mask.Enabled: expected {expectedData.MaskEnabled}, actual {actualCommand.Properties.Mask.Enabled}");
        }

        // Compare Mask Top property (with 3 decimal place precision for coordinates)
        if (!Utilities.AreApproximatelyEqual(actualCommand.Properties.Mask.Top, expectedData.MaskTop, 3))
        {
            failures.Add($"Mask.Top: expected {expectedData.MaskTop}, actual {actualCommand.Properties.Mask.Top}");
        }

        // Compare Mask Bottom property (with 3 decimal place precision for coordinates)
        if (!Utilities.AreApproximatelyEqual(actualCommand.Properties.Mask.Bottom, expectedData.MaskBottom, 3))
        {
            failures.Add($"Mask.Bottom: expected {expectedData.MaskBottom}, actual {actualCommand.Properties.Mask.Bottom}");
        }

        // Compare Mask Left property (with 3 decimal place precision for coordinates)
        if (!Utilities.AreApproximatelyEqual(actualCommand.Properties.Mask.Left, expectedData.MaskLeft, 3))
        {
            failures.Add($"Mask.Left: expected {expectedData.MaskLeft}, actual {actualCommand.Properties.Mask.Left}");
        }

        // Compare Mask Right property (with 3 decimal place precision for coordinates)
        if (!Utilities.AreApproximatelyEqual(actualCommand.Properties.Mask.Right, expectedData.MaskRight, 3))
        {
            failures.Add($"Mask.Right: expected {expectedData.MaskRight}, actual {actualCommand.Properties.Mask.Right}");
        }

        if (failures.Count > 0)
        {
            Assert.Fail($"Command property comparison failed:\n{string.Join("\n", failures)}");
        }
    }

    [Test]
    public void ApplyToState_WithValidData_ShouldUpdateDownstreamKeyerProperties()
    {
        // Arrange
        var state = new AtemState();
        state.Video.DownstreamKeyers = AtemStateUtil.CreateArray<DownstreamKeyer>(2);

        var command = new DownstreamKeyPropertiesCommand
        {
            DownstreamKeyerId = 0,
            Properties = new DownstreamKeyerProperties
            {
                Tie = true,
                Rate = 25,
                PreMultiply = false,
                Clip = 50.0,
                Gain = 75.0,
                Invert = true,
                Mask = new DownstreamKeyerMask
                {
                    Enabled = true,
                    Top = -5.0,
                    Bottom = 5.0,
                    Left = -10.0,
                    Right = 10.0
                }
            }
        };

        // Act
        command.ApplyToState(state);

        // Assert
        Assert.That(state.Video, Is.Not.Null);
        Assert.That(state.Video.DownstreamKeyers.Count, Is.GreaterThan(0));
        Assert.That(state.Video.DownstreamKeyers[0], Is.Not.Null);
        Assert.That(state.Video.DownstreamKeyers[0].Properties, Is.Not.Null);

        var properties = state.Video.DownstreamKeyers[0].Properties!;
        Assert.That(properties.Tie, Is.EqualTo(true));
        Assert.That(properties.Rate, Is.EqualTo(25));
        Assert.That(properties.PreMultiply, Is.EqualTo(false));
        Assert.That(properties.Clip, Is.EqualTo(50.0));
        Assert.That(properties.Gain, Is.EqualTo(75.0));
        Assert.That(properties.Invert, Is.EqualTo(true));

        Assert.That(properties.Mask.Enabled, Is.EqualTo(true));
        Assert.That(properties.Mask.Top, Is.EqualTo(-5.0));
        Assert.That(properties.Mask.Bottom, Is.EqualTo(5.0));
        Assert.That(properties.Mask.Left, Is.EqualTo(-10.0));
        Assert.That(properties.Mask.Right, Is.EqualTo(10.0));
    }

    [Test]
    public void ApplyToState_WithDifferentDownstreamKeyerId_ShouldUpdateCorrectIndex()
    {
        // Arrange
        var state = new AtemState();
        state.Video.DownstreamKeyers = AtemStateUtil.CreateArray<DownstreamKeyer>(4);

        var command = new DownstreamKeyPropertiesCommand
        {
            DownstreamKeyerId = 2,
            Properties = new DownstreamKeyerProperties
            {
                Tie = false,
                Rate = 50,
                PreMultiply = true,
                Clip = 25.0,
                Gain = 30.0,
                Invert = false,
                Mask = new DownstreamKeyerMask
                {
                    Enabled = false,
                    Top = 0.0,
                    Bottom = 0.0,
                    Left = 0.0,
                    Right = 0.0
                }
            }
        };

        // Act
        command.ApplyToState(state);

        // Assert
        Assert.That(state.Video, Is.Not.Null);
        Assert.That(state.Video.DownstreamKeyers[2], Is.Not.Null);
        Assert.That(state.Video.DownstreamKeyers[2].Properties, Is.Not.Null);

        var properties = state.Video.DownstreamKeyers[2].Properties!;
        Assert.That(properties.Tie, Is.EqualTo(false));
        Assert.That(properties.Rate, Is.EqualTo(50));
        Assert.That(properties.PreMultiply, Is.EqualTo(true));
        Assert.That(properties.Clip, Is.EqualTo(25.0));
        Assert.That(properties.Gain, Is.EqualTo(30.0));
        Assert.That(properties.Invert, Is.EqualTo(false));
        Assert.That(properties.Mask.Enabled, Is.EqualTo(false));
    }

    [Test]
    public void ApplyToState_WithInvalidDownstreamKeyerId_ShouldThrowInvalidIdError()
    {
        // Arrange
        var state = new AtemState();
        state.Video.DownstreamKeyers = AtemStateUtil.CreateArray<DownstreamKeyer>(2);

        var command = new DownstreamKeyPropertiesCommand
        {
            DownstreamKeyerId = 5, // Invalid - beyond available keyers
            Properties = new DownstreamKeyerProperties()
        };

        // Act & Assert
        Assert.Throws<IndexOutOfRangeException>(() => command.ApplyToState(state));
    }

    [Test]
    public void ApplyToState_WithNullCapabilities_ShouldThrow()
    {
        // Arrange
        var state = new AtemState();

        var command = new DownstreamKeyPropertiesCommand
        {
            DownstreamKeyerId = 0,
            Properties = new DownstreamKeyerProperties()
        };

        // Act & Assert
        Assert.Throws<IndexOutOfRangeException>(() => command.ApplyToState(state));
    }

    [Test]
    public void ApplyToState_WithExistingDownstreamKeyer_ShouldOverwriteProperties()
    {
        // Arrange
        var state = new AtemState();

        state.Video.DownstreamKeyers = AtemStateUtil.CreateArray<DownstreamKeyer>(2);
        state.Video.DownstreamKeyers[0] = new DownstreamKeyer
        {
            Properties = new DownstreamKeyerProperties
            {
                Tie = false,
                Rate = 100,
                PreMultiply = false,
                Clip = 10.0,
                Gain = 20.0,
                Invert = false,
                Mask = new DownstreamKeyerMask { Enabled = false }
            }
        };

        var command = new DownstreamKeyPropertiesCommand
        {
            DownstreamKeyerId = 0,
            Properties = new DownstreamKeyerProperties
            {
                Tie = true,
                Rate = 30,
                PreMultiply = true,
                Clip = 80.0,
                Gain = 90.0,
                Invert = true,
                Mask = new DownstreamKeyerMask { Enabled = true, Top = -1.0, Bottom = 1.0, Left = -2.0, Right = 2.0 }
            }
        };

        // Act
        command.ApplyToState(state);

        // Assert
        var properties = state.Video.DownstreamKeyers[0].Properties!;
        Assert.That(properties.Tie, Is.EqualTo(true));
        Assert.That(properties.Rate, Is.EqualTo(30));
        Assert.That(properties.PreMultiply, Is.EqualTo(true));
        Assert.That(properties.Clip, Is.EqualTo(80.0));
        Assert.That(properties.Gain, Is.EqualTo(90.0));
        Assert.That(properties.Invert, Is.EqualTo(true));
        Assert.That(properties.Mask.Enabled, Is.EqualTo(true));
        Assert.That(properties.Mask.Top, Is.EqualTo(-1.0));
        Assert.That(properties.Mask.Bottom, Is.EqualTo(1.0));
        Assert.That(properties.Mask.Left, Is.EqualTo(-2.0));
        Assert.That(properties.Mask.Right, Is.EqualTo(2.0));
    }

    [Test]
    public void ApplyToState_ValidIndex_ShouldSucceed()
    {
        // Arrange
        var state = new AtemState();
        state.Video.DownstreamKeyers = AtemStateUtil.CreateArray<DownstreamKeyer>(2);

        var command = new DownstreamKeyPropertiesCommand
        {
            DownstreamKeyerId = 0,
            Properties = new()
            {
                Tie = false,
                Rate = 25,
                PreMultiply = false
            }
        };

        // Act & Assert
        Assert.DoesNotThrow(() => command.ApplyToState(state));
        Assert.That(state.Video.DownstreamKeyers[0].Properties, Is.Not.Null);
    }

    [Test]
    public void ApplyToState_InvalidIndex_ShouldThrow()
    {
        // Arrange
        var state = new AtemState();
        state.Info.Capabilities = new AtemCapabilities
        {
            DownstreamKeyers = 2 // 0-1 valid
        };

        var command = new DownstreamKeyPropertiesCommand
        {
            DownstreamKeyerId = 3, // Invalid - only 0-1 are valid
            Properties = new()
            {
                Tie = false,
                Rate = 25,
                PreMultiply = false
            }
        };

        // Act & Assert
        Assert.Throws<IndexOutOfRangeException>(() => command.ApplyToState(state));
    }
}
