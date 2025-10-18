# Bundled Parameters in Commands

__Extends [Command Properties are Attached to the Command Directly](2025-10-10 Command Properties are Attached to the Command Directly.md)__

This ADR adds an exception to how properties are handled in commands:
When multiple commands work on the same type of object but in different scopes and thus represent the same operation on different scopes **and** their parameters are the same, they are bundled in a reusable parameter object which will be a helper for the serialized and deserialized commands. Thus it

* holds the values
* does the flag calculation (only used by serialized commands)
* apply values to the object type it modifies (only used by deserialized commands)

Serialized commands will override the calculated flags when tests have set flags directly on them.


## Context

In the Atem protocol, many commands operate on similar data structures but in different scopes. For example, Fairlight audio commands for both the Master and individual inputs share the same set of properties (such as Limiter, Compressor, Expander, etc.). Previously, each command defined its own set of parameters, leading to significant code duplication and increased maintenance overhead. As the number of commands and supported features grows, this duplication becomes harder to manage and more error-prone. There was a need for a more maintainable and reusable approach to handling command parameters that are structurally identical across different command types.

## Problem

When implementing the Fairlight commands, it was noted that the structure for the Master properties and the properties of each individual input on a source were the same:
* Limiter
* Compressor
* Expander
* ...

Thus the commands would be a lot of copy and paste which should be avoided.

## Considered options

Several options were considered:

1. **Inline parameters in each command:** Continue defining the same set of properties directly within each command class, accepting the duplication.
2. **Inheritance:** Create a base command class with shared properties, and have specific commands inherit from it. However, this does not work as serialized commands already use a base class
3. **Composition with reusable parameter objects (chosen):** Extract the shared parameters into a separate, reusable object that can be included in any command needing those parameters. This allows for code reuse without forcing an inheritance hierarchy.
4. **Code generation:** Use code generation tools to produce the repeated code, but this adds complexity and reduces clarity for contributors.

## Decision

The decision was made to bundle shared parameters into reusable parameter objects. This approach minimizes code duplication, improves maintainability, and makes it easier to update or extend the set of parameters in the future. By encapsulating flag calculation and value application logic within the parameter object, the codebase becomes more modular and testable. 

## History

- Proposed 2025-10-18
- Accepted 2025-10-18