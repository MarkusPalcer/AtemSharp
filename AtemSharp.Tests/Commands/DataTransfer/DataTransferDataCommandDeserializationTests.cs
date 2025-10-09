using AtemSharp.Commands.DataTransfer;

namespace AtemSharp.Tests.Commands.DataTransfer;

/// <summary>
/// Test deserialization functionality of DataTransferDataCommand using data-driven tests
/// </summary>
[TestFixture]
public class DataTransferDataCommandDeserializationTests : DeserializedCommandTestBase<DataTransferDataCommand, DataTransferDataCommandDeserializationTests.CommandData>
{
	public class CommandData : CommandDataBase
	{
		public ushort TransferId { get; set; }
		public string Body { get; set; } = "";
	}

	protected override void CompareCommandProperties(DataTransferDataCommand actualCommand, CommandData expectedData, TestCaseData testCase)
	{
		var failures = new List<string>();

		// Compare TransferId - exact match required
		if (!actualCommand.TransferId.Equals(expectedData.TransferId))
		{
			failures.Add($"TransferId: expected {expectedData.TransferId}, actual {actualCommand.TransferId}");
		}

		// Compare Body - convert expected Base64 to bytes and compare
		var expectedBodyBytes = Convert.FromBase64String(expectedData.Body);
		if (!actualCommand.Body.SequenceEqual(expectedBodyBytes))
		{
			failures.Add($"Body: expected {BitConverter.ToString(expectedBodyBytes)}, actual {BitConverter.ToString(actualCommand.Body)}");
		}

		// Assert results
		if (failures.Count > 0)
		{
			Assert.Fail($"Command deserialization property mismatch for version {testCase.FirstVersion}:\n" +
			            string.Join("\n", failures));
		}
	}
}