using AtemSharp.Commands.DeviceProfile;

namespace AtemSharp.Tests.Commands.DeviceProfile;

[TestFixture]
public class SuperSourceConfigCommandV8Tests : DeserializedCommandTestBase<SuperSourceConfigCommandV8, SuperSourceConfigCommandV8Tests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int SsrcId { get; set; }
        public byte BoxCount { get; set; }

        // For older protocol versions that may use "Boxes" property in test data
        public byte Boxes
        {
            get => BoxCount;
            set => BoxCount = value;
        }
    }

    protected override void CompareCommandProperties(SuperSourceConfigCommandV8 actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.SuperSourceId, Is.EqualTo(expectedData.SsrcId));
        Assert.That(actualCommand.BoxCount, Is.EqualTo(expectedData.BoxCount));
    }
}
