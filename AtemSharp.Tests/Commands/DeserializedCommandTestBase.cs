using System.Reflection;
using AtemSharp.Commands;
using AtemSharp.Communication;
using AtemSharp.State;
using AtemSharp.State.Info;
using AtemSharp.Tests.TestUtilities.CommandTests;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace AtemSharp.Tests.Commands;

internal abstract class DeserializedCommandTestBase<TCommand, TTestData>
    where TCommand : IDeserializedCommand
    where TTestData : DeserializedCommandTestBase<TCommand, TTestData>.CommandDataBase, new()
{
    private class TestStateHolder : IStateHolder
    {
        public AtemState State { get; } = new();
    }

    [UsedImplicitly(ImplicitUseTargetFlags.Members | ImplicitUseTargetFlags.WithInheritors)]
    public abstract class CommandDataBase : TestUtilities.CommandTests.CommandDataBase
    {
        // Base class for test data - derived classes add specific properties
    }

    public static IEnumerable<TestCaseData> GetTestCases()
    {
        var testCases = Helper.GetTestCases<TCommand, TTestData>().ToArray();
        Assert.That(testCases.Length, Is.GreaterThan(0), "No test cases found");
        return testCases;
    }

    [Test, TestCaseSource(nameof(GetTestCases))]
    public void TestDeserialization(TestUtilities.CommandTests.TestCaseData<TTestData> testCase)
    {
        if (testCase.Command.UnknownProperties.Count != 0)
        {
            Assert.Fail("Unprocessed test data:\n" + JsonConvert.SerializeObject(testCase.Command.UnknownProperties));
        }

        // Arrange - Extract command payload from the full packet
        var commandPayload = testCase.Payload;

        // Act - Deserialize the command
        var actualCommand = DeserializeCommand(commandPayload, testCase.FirstVersion);

        // Assert - Compare properties
        Assert.Multiple(() => CompareCommandProperties(actualCommand, testCase.Command));

        if (TestApplyToState(testCase.Command))
        {
            var stateHolder = new TestStateHolder();

            PrepareState(stateHolder, testCase.Command);

            actualCommand.Apply(stateHolder);

            Assert.Multiple(() => CompareStateProperties(stateHolder, testCase.Command));
        }
    }


    internal abstract void CompareCommandProperties(TCommand actualCommand, TTestData expectedData);

    protected virtual bool TestApplyToState(TTestData testData) => true;

    protected virtual void PrepareState(AtemState state, TTestData expectedData)
    {
    }

    protected virtual void PrepareState(IStateHolder stateHolder, TTestData testData)
    {
        PrepareState(stateHolder.State, testData);
    }

    protected abstract void CompareStateProperties(AtemState state, TTestData expectedData);

    protected virtual void CompareStateProperties(IStateHolder stateHolder, TTestData testData)
    {
        CompareStateProperties(stateHolder.State, testData);
    }


    private static TCommand DeserializeCommand(byte[] payload, ProtocolVersion protocolVersion)
    {
        // Use reflection to call the static Deserialize method
        var deserializeMethod = typeof(TCommand).GetMethod("Deserialize",
                                                           BindingFlags.Public | BindingFlags.Static,
                                                           null,
                                                           [typeof(ReadOnlySpan<Byte>), typeof(ProtocolVersion)],
                                                           null);

        if (deserializeMethod == null)
        {
            throw new InvalidOperationException(
                $"Command {typeof(TCommand).Name} is missing public static IDeserializedCommand Deserialize(ReadOnlySpan<byte> data, ProtocolVersion version)");
        }

        var result = deserializeMethod.CreateDelegate<CommandParser.DeserializeCommand>()(payload.AsSpan(), protocolVersion);

        if (result is not TCommand command)
        {
            throw new InvalidOperationException($"Deserialize method did not return a {typeof(TCommand).Name}");
        }

        return command;
    }
}
