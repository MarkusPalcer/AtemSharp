using AtemSharp.Commands.Fairlight;
using AtemSharp.Enums;
using AtemSharp.Enums.Fairlight;
using AtemSharp.State.Audio.Fairlight;
using AtemSharp.Tests.TestUtilities;

namespace AtemSharp.Tests.Commands.Fairlight;

public class FairlightMixerInputCommandTests : SerializedCommandTestBase<FairlightMixerInputCommand, FairlightMixerInputCommandTests.CommandData>
{
    [MaxProtocolVersion(ProtocolVersion.V8_0_1)]
    public class CommandData : CommandDataBase
    {
        public ushort Index { get; set; }
        public bool RcaToXlrEnabled { get; set; }
        public FairlightInputConfiguration ActiveConfiguration { get; set; }
    }

    protected override FairlightMixerInputCommand CreateSut(TestCaseData testCase)
    {
        var input = new FairlightAudioInput
        {
            Id = testCase.Command.Index,
            Properties =
            {
                ActiveConfiguration = testCase.Command.ActiveConfiguration,
                RcaToXlrEnabled = testCase.Command.RcaToXlrEnabled
            }
        };

        return new FairlightMixerInputCommand(input);
    }
}
