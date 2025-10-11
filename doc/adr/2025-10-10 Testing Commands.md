# Testing Commands

- Serialization is usually tested by having the test class inherit from SerializedCommandTestBase
- Deserialization is usually tested by having the test class inherit from DeserializedCommandTestBase
- These pull test data from the TypeScript implementation to ensure that the C# implementation behaves as the TypeScript implementation
- If that works, no additional tests for serialization/deserialization are required
- If the base class complains that there are no test data for the command, have the test class not inherit from the base class and implement serialization/deserialization tests manually and write a comment on the test class to explain this
- Manual tests are added for methods other than serialization/deserialization
- When wanting to set specific flags, for example to test ApplyToState, use the internal setter of the Flags property after setting all properties to test values.


## Context

Command testing in AtemSharp requires verification that C# implementations behave identically to the reference TypeScript implementation, particularly for serialization and deserialization operations that must maintain byte-level compatibility with ATEM devices.

## Problem

Manual implementation of serialization and deserialization tests for every command was time-consuming, error-prone, and difficult to maintain. Additionally, ensuring byte-for-byte compatibility with the TypeScript reference implementation required access to comprehensive test data that was already available in the TypeScript codebase but not being leveraged effectively in C#.

Without standardized testing patterns, developers had to:
- Manually create test data for each command
- Implement repetitive serialization/deserialization test logic
- Ensure compatibility with TypeScript without automated verification
- Write extensive boilerplate code for common testing scenarios

## Considered options

1. **Manual test implementation for every command**: Write custom serialization and deserialization tests for each command with hand-crafted test data
2. **Base class with automatic test data loading**: Create base test classes that automatically load test data from the TypeScript implementation and provide standardized test patterns
3. **Hybrid approach**: Use base classes when test data is available, fall back to manual tests when needed
4. **Code generation**: Generate test classes automatically from TypeScript test data

## Decision

We selected the **hybrid approach** (#3) with base test classes as the primary mechanism:

- Commands inherit from `SerializedCommandTestBase` or `DeserializedCommandTestBase` to automatically load test data from the TypeScript `libatem-data.json` file
- Base classes provide standardized serialization/deserialization testing with byte-level comparison
- When test data is unavailable, test classes can opt out of inheritance and implement manual tests
- Additional manual tests are written only for command-specific logic beyond serialization/deserialization
- The `Flag` property's internal setter allows precise control over flag combinations for testing specific scenarios

This approach maximizes automation while providing flexibility for edge cases and ensures perfect compatibility with the TypeScript reference implementation.

## History

- Proposed 2025-10-10
- Approved 2025-10-10
