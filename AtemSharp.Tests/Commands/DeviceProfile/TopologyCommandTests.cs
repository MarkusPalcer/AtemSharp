using AtemSharp.Commands.DeviceProfile;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.DeviceProfile;

[TestFixture]
public class TopologyCommandTests : DeserializedCommandTestBase<TopologyCommand, TopologyCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int MixEffectBlocks { get; set; }
        public int VideoSources { get; set; }
        public int DownstreamKeyers { get; set; }
        public int Auxiliaries { get; set; }
        public int MixMinusOutputs { get; set; }
        public int MediaPlayers { get; set; }
        public int Multiviewers { get; set; } = -1;
        public int SerialPort { get; set; }
        public int HyperDecks { get; set; }

        // ReSharper disable once InconsistentNaming - Domain Specific Acronym
        public int DVE { get; set; }
        public int Stingers { get; set; }
        public int SuperSource { get; set; }
        public int TalkbackChannels { get; set; }
        public bool CameraControl { get; set; }
        public bool AdvancedChromaKeyers { get; set; }
        public bool OnlyConfigurableOutputs { get; set; }
    }

    protected override void CompareCommandProperties(TopologyCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        var failures = new List<string>();

        // Always compare basic properties that exist in all versions
        if (actualCommand.MixEffects != expectedData.MixEffectBlocks)
            failures.Add($"MixEffects: expected {expectedData.MixEffectBlocks}, actual {actualCommand.MixEffects}");

        if (actualCommand.Sources != expectedData.VideoSources)
            failures.Add($"Sources: expected {expectedData.VideoSources}, actual {actualCommand.Sources}");

        if (actualCommand.DownstreamKeyers != expectedData.DownstreamKeyers)
            failures.Add($"DownstreamKeyers: expected {expectedData.DownstreamKeyers}, actual {actualCommand.DownstreamKeyers}");

        if (actualCommand.Auxiliaries != expectedData.Auxiliaries)
            failures.Add($"Auxiliaries: expected {expectedData.Auxiliaries}, actual {actualCommand.Auxiliaries}");

        if (actualCommand.MixMinusOutputs != expectedData.MixMinusOutputs)
            failures.Add($"MixMinusOutputs: expected {expectedData.MixMinusOutputs}, actual {actualCommand.MixMinusOutputs}");

        if (actualCommand.MediaPlayers != expectedData.MediaPlayers)
            failures.Add($"MediaPlayers: expected {expectedData.MediaPlayers}, actual {actualCommand.MediaPlayers}");

        if (actualCommand.SerialPorts != expectedData.SerialPort)
            failures.Add($"SerialPorts: expected {expectedData.SerialPort}, actual {actualCommand.SerialPorts}");

        if (actualCommand.MaxHyperdecks != expectedData.HyperDecks)
            failures.Add($"MaxHyperdecks: expected {expectedData.HyperDecks}, actual {actualCommand.MaxHyperdecks}");

        if (actualCommand.DVEs != expectedData.DVE)
            failures.Add($"DVEs: expected {expectedData.DVE}, actual {actualCommand.DVEs}");

        if (actualCommand.Stingers != expectedData.Stingers)
            failures.Add($"Stingers: expected {expectedData.Stingers}, actual {actualCommand.Stingers}");

        if (actualCommand.SuperSources != expectedData.SuperSource)
            failures.Add($"SuperSources: expected {expectedData.SuperSource}, actual {actualCommand.SuperSources}");

        // Only compare extended properties for newer protocol versions
        var isNewProtocol = testCase.FirstVersion > ProtocolVersion.V8_0_1;

        if (isNewProtocol)
        {
            if (actualCommand.Multiviewers != expectedData.Multiviewers)
                failures.Add($"Multiviewers: expected {expectedData.Multiviewers}, actual {actualCommand.Multiviewers}");

            if (actualCommand.TalkbackChannels != expectedData.TalkbackChannels)
                failures.Add($"TalkbackChannels: expected {expectedData.TalkbackChannels}, actual {actualCommand.TalkbackChannels}");

            if (actualCommand.CameraControl != expectedData.CameraControl)
                failures.Add($"CameraControl: expected {expectedData.CameraControl}, actual {actualCommand.CameraControl}");

            if (actualCommand.AdvancedChromaKeyers != expectedData.AdvancedChromaKeyers)
                failures.Add($"AdvancedChromaKeyers: expected {expectedData.AdvancedChromaKeyers}, actual {actualCommand.AdvancedChromaKeyers}");

            if (actualCommand.OnlyConfigurableOutputs != expectedData.OnlyConfigurableOutputs)
                failures.Add($"OnlyConfigurableOutputs: expected {expectedData.OnlyConfigurableOutputs}, actual {actualCommand.OnlyConfigurableOutputs}");
        }
        else
        {
            // For older protocol versions, these should be their default values
            if (actualCommand.Multiviewers != -1)
                failures.Add($"Multiviewers: expected -1 (older protocol), actual {actualCommand.Multiviewers}");

            if (actualCommand.TalkbackChannels != 0)
                failures.Add($"TalkbackChannels: expected 0 (older protocol), actual {actualCommand.TalkbackChannels}");

            if (actualCommand.CameraControl)
                failures.Add($"CameraControl: expected false (older protocol), actual {actualCommand.CameraControl}");

            if (actualCommand.AdvancedChromaKeyers)
                failures.Add($"AdvancedChromaKeyers: expected false (older protocol), actual {actualCommand.AdvancedChromaKeyers}");

            if (actualCommand.OnlyConfigurableOutputs)
                failures.Add($"OnlyConfigurableOutputs: expected false (older protocol), actual {actualCommand.OnlyConfigurableOutputs}");
        }

        if (failures.Count > 0)
        {
            Assert.Fail($"Command property comparison failed:\n{string.Join("\n", failures)}");
        }
    }

    [Test]
    public void Deserialize_WithOlderProtocolVersion_ShouldNotIncludeMultiviewers()
    {
        // Arrange - Create test data for older protocol version (V8_0_1 and earlier)
        var data = new byte[]
        {
            2,   // MixEffects (offset 0)
            16,  // Sources (offset 1)
            2,   // DownstreamKeyers (offset 2)
            4,   // Auxiliaries (offset 3)
            0,   // MixMinusOutputs (offset 4)
            2,   // MediaPlayers (offset 5)
            4,   // SerialPorts (offset 6)
            4,   // MaxHyperdecks (offset 7)
            1,   // DVEs (offset 8)
            1,   // Stingers (offset 9)
            1,   // SuperSources (offset 10)
            0,   // Unused (offset 11)
            16,  // TalkbackChannels (offset 12)
            0, 0, 0, 0, // Padding to reach offset 17
            1,   // CameraControl (offset 17)
        };

        using var stream = new MemoryStream(data);

        // Act
        var command = TopologyCommand.Deserialize(stream, ProtocolVersion.V8_0_1);

        // Assert
        Assert.That(command.MixEffects, Is.EqualTo(2));
        Assert.That(command.Sources, Is.EqualTo(16));
        Assert.That(command.DownstreamKeyers, Is.EqualTo(2));
        Assert.That(command.Auxiliaries, Is.EqualTo(4));
        Assert.That(command.MixMinusOutputs, Is.EqualTo(0));
        Assert.That(command.MediaPlayers, Is.EqualTo(2));
        Assert.That(command.Multiviewers, Is.EqualTo(-1)); // Should remain -1 for older versions
        Assert.That(command.SerialPorts, Is.EqualTo(4));
        Assert.That(command.MaxHyperdecks, Is.EqualTo(4));
        Assert.That(command.DVEs, Is.EqualTo(1));
        Assert.That(command.Stingers, Is.EqualTo(1));
        Assert.That(command.SuperSources, Is.EqualTo(1));
        Assert.That(command.TalkbackChannels, Is.EqualTo(16));
        Assert.That(command.CameraControl, Is.True);
        Assert.That(command.AdvancedChromaKeyers, Is.False); // Default for shorter buffer
        Assert.That(command.OnlyConfigurableOutputs, Is.False); // Default for shorter buffer
    }

    [Test]
    public void Deserialize_WithNewerProtocolVersion_ShouldIncludeMultiviewers()
    {
        // Arrange - Create test data for newer protocol version (after V8_0_1)
        var data = new byte[]
        {
            2,   // MixEffects (offset 0)
            20,  // Sources (offset 1)
            4,   // DownstreamKeyers (offset 2)
            6,   // Auxiliaries (offset 3)
            2,   // MixMinusOutputs (offset 4)
            2,   // MediaPlayers (offset 5)
            2,   // Multiviewers (offset 6) - new field
            4,   // SerialPorts (offset 7)
            8,   // MaxHyperdecks (offset 8)
            2,   // DVEs (offset 9)
            2,   // Stingers (offset 10)
            2,   // SuperSources (offset 11)
            0,   // Unused (offset 12)
            32,  // TalkbackChannels (offset 13)
            0, 0, 0, 0, // Padding to reach offset 18
            1,   // CameraControl (offset 18)
        };

        using var stream = new MemoryStream(data);

        // Act
        var command = TopologyCommand.Deserialize(stream, ProtocolVersion.V8_1_1);

        // Assert
        Assert.That(command.MixEffects, Is.EqualTo(2));
        Assert.That(command.Sources, Is.EqualTo(20));
        Assert.That(command.DownstreamKeyers, Is.EqualTo(4));
        Assert.That(command.Auxiliaries, Is.EqualTo(6));
        Assert.That(command.MixMinusOutputs, Is.EqualTo(2));
        Assert.That(command.MediaPlayers, Is.EqualTo(2));
        Assert.That(command.Multiviewers, Is.EqualTo(2)); // Should be read for newer versions
        Assert.That(command.SerialPorts, Is.EqualTo(4));
        Assert.That(command.MaxHyperdecks, Is.EqualTo(8));
        Assert.That(command.DVEs, Is.EqualTo(2));
        Assert.That(command.Stingers, Is.EqualTo(2));
        Assert.That(command.SuperSources, Is.EqualTo(2));
        Assert.That(command.TalkbackChannels, Is.EqualTo(32));
        Assert.That(command.CameraControl, Is.True);
        Assert.That(command.AdvancedChromaKeyers, Is.False); // Default for shorter buffer
        Assert.That(command.OnlyConfigurableOutputs, Is.False); // Default for shorter buffer
    }

    [Test]
    public void Deserialize_WithExtendedBuffer_ShouldIncludeAdvancedFeatures()
    {
        // Arrange - Create test data with extended buffer for advanced features
        var data = new byte[]
        {
            4,   // MixEffects (offset 0)
            40,  // Sources (offset 1)
            4,   // DownstreamKeyers (offset 2)
            8,   // Auxiliaries (offset 3)
            4,   // MixMinusOutputs (offset 4)
            4,   // MediaPlayers (offset 5)
            4,   // Multiviewers (offset 6) - newer protocol
            8,   // SerialPorts (offset 7)
            8,   // MaxHyperdecks (offset 8)
            4,   // DVEs (offset 9)
            4,   // Stingers (offset 10)
            4,   // SuperSources (offset 11)
            0,   // Unused (offset 12)
            64,  // TalkbackChannels (offset 13)
            0, 0, 0, 0, // Padding to reach offset 18
            1,   // CameraControl (offset 18)
            0, 0, 0, // More padding to reach advanced features
            1,   // AdvancedChromaKeyers (offset 22)
            1,   // OnlyConfigurableOutputs (offset 23)
        };

        using var stream = new MemoryStream(data);

        // Act
        var command = TopologyCommand.Deserialize(stream, ProtocolVersion.V9_4);

        // Assert
        Assert.That(command.MixEffects, Is.EqualTo(4));
        Assert.That(command.Sources, Is.EqualTo(40));
        Assert.That(command.DownstreamKeyers, Is.EqualTo(4));
        Assert.That(command.Auxiliaries, Is.EqualTo(8));
        Assert.That(command.MixMinusOutputs, Is.EqualTo(4));
        Assert.That(command.MediaPlayers, Is.EqualTo(4));
        Assert.That(command.Multiviewers, Is.EqualTo(4));
        Assert.That(command.SerialPorts, Is.EqualTo(8));
        Assert.That(command.MaxHyperdecks, Is.EqualTo(8));
        Assert.That(command.DVEs, Is.EqualTo(4));
        Assert.That(command.Stingers, Is.EqualTo(4));
        Assert.That(command.SuperSources, Is.EqualTo(4));
        Assert.That(command.TalkbackChannels, Is.EqualTo(64));
        Assert.That(command.CameraControl, Is.True);
        Assert.That(command.AdvancedChromaKeyers, Is.True);
        Assert.That(command.OnlyConfigurableOutputs, Is.True);
    }

    [Test]
    public void ApplyToState_WithBasicTopology_ShouldUpdateCapabilities()
    {
        // Arrange
        var state = new AtemState();
        var command = new TopologyCommand
        {
            MixEffects = 2,
            Sources = 16,
            DownstreamKeyers = 2,
            Auxiliaries = 4,
            MixMinusOutputs = 0,
            MediaPlayers = 2,
            Multiviewers = -1, // Older protocol version
            SerialPorts = 4,
            MaxHyperdecks = 4,
            DVEs = 1,
            Stingers = 1,
            SuperSources = 1,
            TalkbackChannels = 16,
            CameraControl = true,
            AdvancedChromaKeyers = false,
            OnlyConfigurableOutputs = false
        };

        // Act
        command.ApplyToState(state);

        // Assert
        Assert.That(state.Info.Capabilities, Is.Not.Null);
        Assert.That(state.Info.Capabilities.MixEffects, Is.EqualTo(2));
        Assert.That(state.Info.Capabilities.Sources, Is.EqualTo(16));
        Assert.That(state.Info.Capabilities.DownstreamKeyers, Is.EqualTo(2));
        Assert.That(state.Info.Capabilities.Auxiliaries, Is.EqualTo(4));
        Assert.That(state.Info.Capabilities.MixMinusOutputs, Is.EqualTo(0));
        Assert.That(state.Info.Capabilities.MediaPlayers, Is.EqualTo(2));
        Assert.That(state.Info.Capabilities.MultiViewers, Is.EqualTo(-1));
        Assert.That(state.Info.Capabilities.SerialPorts, Is.EqualTo(4));
        Assert.That(state.Info.Capabilities.MaxHyperdecks, Is.EqualTo(4));
        Assert.That(state.Info.Capabilities.DVEs, Is.EqualTo(1));
        Assert.That(state.Info.Capabilities.Stingers, Is.EqualTo(1));
        Assert.That(state.Info.Capabilities.SuperSources, Is.EqualTo(1));
        Assert.That(state.Info.Capabilities.TalkbackChannels, Is.EqualTo(16));
        Assert.That(state.Info.Capabilities.CameraControl, Is.True);
        Assert.That(state.Info.Capabilities.AdvancedChromaKeyers, Is.False);
        Assert.That(state.Info.Capabilities.OnlyConfigurableOutputs, Is.False);
        Assert.That(state.Info.MultiViewer.Count, Is.EqualTo(0));
        Assert.That(state.Info.MultiViewer.WindowCount, Is.EqualTo(0));
    }

    [Test]
    public void ApplyToState_WithMultiviewers_ShouldUpdateCapabilitiesAndMultiviewer()
    {
        // Arrange
        var state = new AtemState();
        var command = new TopologyCommand
        {
            MixEffects = 4,
            Sources = 40,
            DownstreamKeyers = 4,
            Auxiliaries = 8,
            MixMinusOutputs = 4,
            MediaPlayers = 4,
            Multiviewers = 2, // Newer protocol with multiviewers
            SerialPorts = 8,
            MaxHyperdecks = 8,
            DVEs = 4,
            Stingers = 4,
            SuperSources = 4,
            TalkbackChannels = 64,
            CameraControl = true,
            AdvancedChromaKeyers = true,
            OnlyConfigurableOutputs = true
        };

        // Act
        command.ApplyToState(state);

        // Assert - Capabilities
        Assert.That(state.Info.Capabilities, Is.Not.Null);
        Assert.That(state.Info.Capabilities.MixEffects, Is.EqualTo(4));
        Assert.That(state.Info.Capabilities.Sources, Is.EqualTo(40));
        Assert.That(state.Info.Capabilities.MultiViewers, Is.EqualTo(2));
        Assert.That(state.Info.Capabilities.AdvancedChromaKeyers, Is.True);
        Assert.That(state.Info.Capabilities.OnlyConfigurableOutputs, Is.True);

        // Assert - Multiviewer info should be created
        Assert.That(state.Info.MultiViewer, Is.Not.Null);
        Assert.That(state.Info.MultiViewer.Count, Is.EqualTo(2));
        Assert.That(state.Info.MultiViewer.WindowCount, Is.EqualTo(10)); // Default value
    }

    [Test]
    public void ApplyToState_WithZeroMultiviewers_ShouldNotCreateMultiviewerInfo()
    {
        // Arrange
        var state = new AtemState();
        var command = new TopologyCommand
        {
            MixEffects = 1,
            Sources = 8,
            DownstreamKeyers = 1,
            Auxiliaries = 2,
            MixMinusOutputs = 0,
            MediaPlayers = 1,
            Multiviewers = 0, // Zero multiviewers
            SerialPorts = 2,
            MaxHyperdecks = 2,
            DVEs = 0,
            Stingers = 0,
            SuperSources = 0,
            TalkbackChannels = 8,
            CameraControl = false,
            AdvancedChromaKeyers = false,
            OnlyConfigurableOutputs = false
        };

        // Act
        command.ApplyToState(state);

        // Assert
        Assert.That(state.Info.Capabilities, Is.Not.Null);
        Assert.That(state.Info.Capabilities.MultiViewers, Is.EqualTo(0));
        Assert.That(state.Info.MultiViewer.Count, Is.EqualTo(0));
        Assert.That(state.Info.MultiViewer.WindowCount, Is.EqualTo(0));
    }
}
