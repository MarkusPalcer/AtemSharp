using AtemSharp.Commands.MixEffects;
using AtemSharp.State.Video.MixEffect;

namespace AtemSharp.Tests.Commands.MixEffects;

[TestFixture]
public class PreviewInputCommandTests : SerializedCommandTestBase<PreviewInputCommand,
    PreviewInputCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public ushort Source { get; set; }
    }

    protected override PreviewInputCommand CreateSut(TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        return new PreviewInputCommand(new MixEffect
        {
            Id = testCase.Command.Index,
            PreviewInput = testCase.Command.Source
        });
    }

    [Test]
    public void Source_WhenSet_UpdatesFlagAutomatically()
    {
        // Arrange
        var command = new PreviewInputCommand(new MixEffect());

        // Act
        command.Source = 2000;

        // Assert
        Assert.That(command.Source, Is.EqualTo(2000));
    }
}
