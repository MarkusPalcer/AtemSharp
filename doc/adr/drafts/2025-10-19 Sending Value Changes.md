# Sending Value Changes

## Context

TODO: Draft, looking for alternative options


## Problem

The consumer of the library wants a convenient way to send changes to the ATEM switcher

## Considered options

### Raw `SendCommand` on `Atem` class

| Advantages | Disadvantages |
|--|--|
| Minimal effort | Consumer needs to learn which command can change which part of the state |

### Like TypeScript implementation

The TS implementation has lots of `SetXYZ` methods on the root `Atem` object which wrap creating the commands

| Advantages | Disadvantages |
|--|--|
| Same API as TS implementation, no additional thinking required on both ends | |

### Setting State-Properties sends the command

As soon as a property is set by the consumer of the library, the change is sent to the ATEM switcher


| Advantages | Disadvantages |
|--|--|
| Most understandable for C#-Developers | Needs code to detect whether the change came from the consumer or the ATEM-Switcher |
| | May conflict with the wrapper-object solution in [Subscribing to State-changes](2025-10-19%20Subscribing%20to%20State-Changes.md) |
| | Sends multiple commands when multiple values have changed that could've been bundled into one command |

### Fluent-Like Syntax 

A method that starts state-changes, from where on the setter methods (like the TS-Implementation) can be called, but they are sorted in the same structure as the state objects. Each method returns the root object, so when multiple methods are called, a bulk-operation is created (i.e. multiple changes that can be sent in one command are sent as one command).

Example:
```C#
atem.ChangeState()
    .FairlightAudio().Master().SetFaderGain(10.0)
    .FairlightAudio().Master().Equalizer().Disable()
    .Media().Players[0].SetStillId(2)
    .Execute();
```


| Advantages | Disadvantages |
|--|--|
| Supports bulk operations | Not very C# like |
| Might be easier to learn than lots of setter methods | Lots of implementation effort (basically doubling the state structure)

## Decision

None decision made yet

## History

- Draft 2025-10-19