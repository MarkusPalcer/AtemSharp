# Subscribin to State-Changes

## Context

TODO: Draft, looking for alternative options

## Problem

The programmer consuming this library wants to know when something changed in the state.
They want to be informed of a specific state change.

## Considered options

### How information is conveyed

#### Use TypeScript implementation 

**Example:**
```csharp
// Subscribe to property change event with string path
atem.PropertyChanged += (sender, e) =>
{
	if (e.PropertyName == "state.audio.master.faderGain" && 
		atem.State?.Audio is FairlightAudioState fairlight)
	{
		double newValue = fairlight.Master.FaderGain;
		// Handle the new value
	}
};
```

The TypeScript implementation uses a single event which is similar to WPFs `PropertyChanged`-event, conveying the full path to the property that has changed as string.

| Advantages | Disadvantages |
|--|--|
| Same API as TS implementation, no additional thinking required on both ends | Uses magic strings - the same downside to the WPF mechanism |
| Easy to implement and extend | Harder to refactor, risk of typos |
| Simple event signature | No compile-time safety |
| No extra dependencies | |

#### Values wrapped in class with event

The actual value holding property is wrapped in a class which can be implicitely converted to the underlying type and offers event handling.
| Advantages | Disadvantages |
|--|--|
| Change-Detection can be wrapped inside that class (i.e. only send an event when the new value is different from the old) | Unconventional mechanism might confuse new developers |
| | Conflicts with sending updates to the ATEM whenever setting the state, because it removes the advantage of the normal C#-Like syntax of just setting the property (`operator=` can't be overridden like in C++) |
| Can encapsulate validation and transformation logic | May require custom serialization |
| Can be reused for other stateful values | Can increase memory usage |
| No extra dependencies | |

**Example:**
```csharp
// Value wrapper with event
fairlight.Master.FaderGain.ValueChanged += (sender, args) =>
{
	// args.NewValue contains the new FaderGain
	// args.OldValue contains the previous value
	// Optionally check type if needed
};
```

#### Strongly-Typed Event Arguments
Instead of using strings or wrapping values, define custom event argument classes for each state change type, so events carry strongly-typed data.
| Advantages | Disadvantages |
|--|--|
| Type safety, IDE support, refactoring-friendly | Can lead to many event classes, more boilerplate |
| No magic strings, easier to discover usage | Less flexible for dynamic or generic state changes |
| Clear contract for event consumers | Can be verbose for large state models |
| Easy to document and test | |
| No extra dependencies | |

**Example:**
```csharp
// Strongly-typed event for FaderGain change
atem.MasterFaderGainChanged += (sender, e) =>
{
    double newValue = e.NewFaderGain;
    // Handle the new value
};
```

#### Change Notification Interface (e.g., INotifyPropertyChanged)
Implement a standard interface like `INotifyPropertyChanged` on state objects, allowing consumers to subscribe to property changes.
| Advantages | Disadvantages |
|--|--|
| Familiar to .NET developers, supported by many frameworks | Only works for property changes, not for more complex state transitions |
| Integrates with data binding | May require refactoring existing state objects |
| Supported by UI frameworks (e.g., WPF, WinForms) | Can be noisy if many properties change frequently |
| Well-documented pattern | |
| No extra dependencies (built-in to .NET) | |

**Example:**
```csharp
// INotifyPropertyChanged on FairlightAudioState
if (atem.State?.Audio is FairlightAudioState fairlight)
{
	fairlight.Master.PropertyChanged += (sender, e) =>
	{
		if (e.PropertyName == nameof(FairlightAudioState.Master.FaderGain))
		{
			double newValue = fairlight.Master.FaderGain;
			// Handle the new value
		}
	};
}
```

#### Event Aggregator / Message Bus
Use a central event aggregator or message bus to publish and subscribe to state change events.
| Advantages | Disadvantages |
|--|--|
| Decouples publishers and subscribers, supports multiple event types | Adds architectural complexity, may be overkill for simple scenarios |
| Scalable for large applications | Debugging can be harder due to indirection |
| Can support cross-module communication | May require learning curve for contributors |
| Can buffer or replay events | Requires a library (e.g., MediatR) |
| | |

**Example:**
```csharp
// Subscribe to FaderGainChanged message
eventAggregator.Subscribe<FaderGainChangedMessage>(msg =>
{
    double newValue = fairlight.Master.FaderGain;
    // Handle the new value
});
```

### Technical implementation of events

All these examples showcase subscribing to events that are implemented like the TypeScript interface is.
If for the above section a different solution is chosen, it needs to be adapted accordingly.

#### C#-Events
Classical C#-Events where the consumer can add handlers to

**Example:**
```csharp
atem.State.StateChanged += (sender, args) => {
    if (args.PropertyName.StartsWith("Fairlight.Master.FaderGain")) {
        double newValue = args.NewValue;
        // ...
    }
};
```

| Advantages | Disadvantages |
|--|--|
| Well known mechanism |  |
| Built-in to the language | Can be hard to unsubscribe safely |
| No extra dependencies | Not suitable for complex event flows |
| Simple for small projects | |



#### IObservable, ReactiveExtensions
Events are conveyed through IObservable-interface which can be subscribed to

**Example:**
```csharp
// IObservable subscription (using System.Reactive)
IDisposable subscription = atem.State.Changes.Where(x => x.Property.StartsWith("Fairlight.Master.FaderGain")).Subscribe(x => {
    double newValue = x.NewValue;
    // ...
});
```

| Advantages | Disadvantages |
|--|--|
| Modern approach | Mechanism not well known to most programmers |
| Events can easily be filtered with LINQ | |
| Supports async and complex event composition | Can be overkill for simple needs |
| Powerful for chaining and transformation | |
| Requires a library (e.g., System.Reactive) | |

#### Delegate Callbacks
Allow consumers to register delegate callbacks directly on state objects or the library.

**Example:**
```csharp
// Registering a delegate callback
atem.State.ChangeCallback = (property, newValue) =>
{
    if (!property.StartsWith("Fairlight.Master.FaderGain")) return;
	if (newValue is double doubleValue) return;
    // ...
};
```


| Advantages | Disadvantages |
|--|--|
| Simple, flexible, no need for event keyword | Less discoverable, can be harder to manage lifecycle |
| Can pass any data structure | No built-in support for event patterns |
| No extra dependencies | Can lead to memory leaks if not managed |
| Easy to implement | |

#### Custom Event Dispatcher
Implement a custom event dispatcher that allows subscribing, unsubscribing, and dispatching events with custom logic (e.g., priorities, async).

**Example:**
```csharp
// Custom event dispatcher
eventDispatcher.Subscribe("Fairlight.Master.FaderGainChanged", (args) =>
{
	double newValue = (double)args.NewValue;
	// Handle the new value
});
```

| Advantages | Disadvantages |
|--|--|
| Full control over event flow, can add advanced features | More code to maintain, reinvents existing mechanisms |
| Can support advanced features (e.g., priorities, async) | May require a third-party library or custom implementation |
| Can be tailored to project needs | |
| Needs additional library (e.g. Prism.Events (for WPF)) | |

## Decision

No decision made yet

## History

- Draft started 2025-10-19