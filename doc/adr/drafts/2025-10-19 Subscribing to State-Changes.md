# Subscribin to State-Changes

## Context

TODO: Draft, looking for alternative options

## Problem

The programmer consuming this library wants to know when something changed in the state.
They want to be informed of a specific state change.

## Considered options

### How information is conveyed

#### Use TypeScript implementation 
The TypeScript implementation uses a single event which is similar to WPFs `PropertyChanged`-event, conveying the full path to the property that has changed as string.

| Advantages | Disadvantages |
|--|--|
| Same API as TS implementation, no additional thinking required on both ends | Uses magic strings - the same downside to the WPF mechanism |

#### Values wrapped in class with event

The actual value holding property is wrapped in a class which can be implicitely converted to the underlying type and offers event handling.

| Advantages | Disadvantages |
|--|--|
| Change-Detection can be wrapped inside that class (i.e. only send an event when the new value is different from the old) | Unconventional mechanism might confuse new developers |


### Technical implementation of events

#### C#-Events

Classical C#-Events where the consumer can add handlers to

| Advantages | Disadvantages |
|--|--|
| Well known mechanism |  |

#### IObservable, ReactiveExtensions

Events are conveyed through IObservable-interface which can be subscribed to

| Advantages | Disadvantages |
|--|--|
| Modern approach | Mechanism not well known to most programmers |
| Events can easily be filtered with LINQ | |

## Decision

No decision made yet

## History

- Draft started 2025-10-19