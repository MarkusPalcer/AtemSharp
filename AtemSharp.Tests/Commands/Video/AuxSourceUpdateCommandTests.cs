using AtemSharp.Commands.Video;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.Video;

[TestFixture]
public class AuxSourceUpdateCommandTests : DeserializedCommandTestBase<AuxSourceUpdateCommand,
    AuxSourceUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int Id { get; set; }
        public int Source { get; set; }
    }

    protected override void CompareCommandProperties(AuxSourceUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.AuxId, Is.EqualTo(expectedData.Id),
                    $"AuxId should match expected value for test case {testCase.Name}");
        Assert.That(actualCommand.Source, Is.EqualTo(expectedData.Source),
                    $"Source should match expected value for test case {testCase.Name}");
    }

    [Test]
    public void ApplyToState_WithValidAuxiliary_UpdatesSource()
    {
        // Arrange
        var state = new AtemState
        {
            Video = new VideoState
            {
                Auxiliaries = AtemStateUtil.CreateArray<AuxiliaryOutput>(2)
            },
        };

        var command = new AuxSourceUpdateCommand
        {
            AuxId = 1,
            Source = 2000
        };

        // Act
        command.ApplyToState(state);

        // Assert
        Assert.That(state.Video.Auxiliaries[1].Source, Is.EqualTo(2000));
    }

    [Test]
    public void ApplyToState_ValidIndex_ShouldSucceed()
    {
        // Arrange
        var state = new AtemState
        {
            Video =
            {
                Auxiliaries = AtemStateUtil.CreateArray<AuxiliaryOutput>(3)
            }
        };

        var command = new AuxSourceUpdateCommand
        {
            AuxId = 2,
            Source = 3000
        };

        // Act & Assert
        Assert.DoesNotThrow(() => command.ApplyToState(state));
        Assert.That(state.Video.Auxiliaries[2].Source, Is.EqualTo(3000));
    }
}
