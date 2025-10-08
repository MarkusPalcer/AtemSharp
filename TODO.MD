# AtemSharp Implementation TODO

This document outlines what needs to be implemented in the C# AtemSharp library to achieve feature parity with the TypeScript implementation.

## Current State Analysis

The C# implementation (`AtemSharp/`) is currently in a very early stage compared to the comprehensive TypeScript implementation in `src/`. The TypeScript version is a full-featured ATEM control library, while the C# version has only basic connection infrastructure.

---

## Major Missing Components

### 1. Commands Architecture (Critical)

**Current State:** Only basic `CommandBase.cs` and minimal commands in `Commands/DeviceProfile/` and `Commands/MixEffects/`

**Missing:** Complete command system covering all ATEM functionality

**TypeScript Reference:** `src/commands/` (25+ command categories)

#### Specific Command Categories to Implement:

- **Audio Commands** (`src/commands/Audio/`)
  - AudioMixerHeadphonesCommand
  - AudioMixerInputCommand  
  - AudioMixerMasterCommand
  - AudioMixerMonitorCommand
  - AudioMixerPropertiesCommand
  - AudioMixerResetPeaksCommand

- **Data Transfer Commands** (`src/commands/DataTransfer/`)
  - DataTransferAckCommand
  - DataTransferCompleteCommand
  - DataTransferDataCommand
  - DataTransferDownloadRequestCommand
  - DataTransferErrorCommand
  - DataTransferFileDescriptionCommand
  - DataTransferUploadContinueCommand
  - DataTransferUploadRequestCommand
  - LockObtainedCommand
  - LockStateCommand

- **Display Clock Commands** (`src/commands/DisplayClock/`)
  - DisplayClockCurrentTimeCommand
  - DisplayClockPropertiesGetCommand
  - DisplayClockPropertiesSetCommand
  - DisplayClockRequestTimeCommand
  - DisplayClockStateSetCommand

- **Downstream Key Commands** (`src/commands/DownstreamKey/`)
  - DownstreamKeyAutoCommand
  - DownstreamKeyCutSourceCommand
  - DownstreamKeyFillSourceCommand
  - DownstreamKeyGeneralCommand
  - DownstreamKeyMaskCommand
  - DownstreamKeyOnAirCommand
  - DownstreamKeyPropertiesCommand
  - DownstreamKeyRateCommand
  - DownstreamKeySourcesCommand
  - DownstreamKeyStateCommand
  - DownstreamKeyTieCommand

- **Fairlight Audio Commands** (`src/commands/Fairlight/`)
  - 20+ commands for Fairlight audio mixing
  - AudioRouting commands
  - FairlightMixer* commands for all aspects of audio control

- **Input Commands** (`src/commands/Inputs/`)
  - InputPropertiesCommand

- **Macro Commands** (`src/commands/Macro/`)
  - MacroActionCommand
  - MacroAddTimedPauseCommand
  - MacroPropertiesCommand
  - MacroRecordCommand
  - MacroRecordingStatusCommand
  - MacroRunStatusCommand

- **Media Commands** (`src/commands/Media/`)
  - MediaPlayerSourceCommand
  - MediaPlayerStatusCommand
  - MediaPoolCaptureStillCommand
  - MediaPoolClearClipCommand
  - MediaPoolClearStillCommand
  - MediaPoolClipDescription
  - MediaPoolFrameDescription
  - MediaPoolSetClipCommand

- **Mix Effects Commands** (`src/commands/MixEffects/`)
  - AutoTransitionCommand
  - CutCommand
  - PreviewInputCommand  
  - ProgramInputCommand
  - FadeToBlack commands
  - Key commands (16+ commands for upstream keyers)
  - Transition commands (8+ commands for transitions)

- **Recording Commands** (`src/commands/Recording/`)
  - RecordingDiskCommand
  - RecordingDurationCommand
  - RecordingISOCommand
  - RecordingSettingsCommand
  - RecordingStatusCommand

- **Settings Commands** (`src/commands/Settings/`)
  - MediaPool commands
  - MultiViewer commands
  - VideoMode commands

- **Streaming Commands** (`src/commands/Streaming/`)
  - StreamingDurationCommand
  - StreamingServiceCommand
  - StreamingStatsCommand
  - StreamingStatusCommand
  - StreamingAudioBitratesCommand

- **SuperSource Commands** (`src/commands/SuperSource/`)
  - SuperSourceBoxParametersCommand
  - SuperSourcePropertiesCommand

- **Root Level Commands**
  - AuxSourceCommand ✓ (exists but needs verification)
  - CameraControlCommand ✓ (exists but needs verification)
  - ColorGeneratorCommand ✓ (exists but needs verification)
  - InitCompleteCommand ✓ (exists but needs verification)
  - PowerStatusCommand ✓ (exists but needs verification)
  - StartupStateCommand ✓ (exists but needs verification)
  - TallyBySourceCommand ✓ (exists but needs verification)
  - TimeCommand ✓ (exists but needs verification)
  - TimeConfigCommand ✓ (exists but needs verification)

### 2. State Management System (Critical)

**Current State:** Only basic `AtemState.cs` class

**Missing:** Complete state model covering all ATEM device state

**TypeScript Reference:** `src/state/` (10+ state modules)

#### State Modules to Implement:

- **Audio State** (`src/state/audio.ts`)
  - AtemClassicAudioState
  - ClassicAudioChannel
  - ClassicAudioMasterChannel
  - ClassicAudioMonitorChannel
  - ClassicAudioHeadphoneOutputChannel

- **Color State** (`src/state/color.ts`)
  - ColorGeneratorState ✓ (mentioned in current code)

- **Display Clock State** (`src/state/displayClock.ts`)
  - DisplayClockState

- **Fairlight State** (`src/state/fairlight.ts`)
  - AtemFairlightAudioState
  - 10+ Fairlight-specific state interfaces

- **Info State** (`src/state/info.ts`)
  - DeviceInfo
  - TimeInfo
  - ProtocolVersion info

- **Input State** (`src/state/input.ts`)
  - InputChannel

- **Levels State** (`src/state/levels.ts`)
  - SomeAtemAudioLevels
  - Audio level monitoring

- **Macro State** (`src/state/macro.ts`)
  - MacroState
  - MacroPlayerState

- **Media State** (`src/state/media.ts`)
  - MediaState
  - MediaPlayer
  - MediaPlayerSource

- **Recording State** (`src/state/recording.ts`)
  - RecordingState
  - RecordingStateProperties

- **Settings State** (`src/state/settings.ts`)
  - SettingsState
  - MultiViewerPropertiesState

- **Streaming State** (`src/state/streaming.ts`)
  - StreamingState
  - StreamingServiceProperties

- **Video State** (`src/state/video/`)
  - AtemVideoState
  - MixEffects state
  - UpstreamKeyers state
  - DownstreamKeyers state
  - SuperSource state
  - Transition state

- **State Utilities** (`src/state/util.ts`)
  - AtemStateUtil.Create() ✓ (referenced in current code)
  - State manipulation utilities

### 3. Data Transfer System (Critical)

**Current State:** No data transfer implementation

**Missing:** Complete data transfer manager and related functionality

**TypeScript Reference:** `src/dataTransfer/` (10+ data transfer modules)

#### Data Transfer Components to Implement:

- **Core Data Transfer** (`src/dataTransfer/dataTransfer.ts`)
  - DataTransferManager ✓ (referenced in current code but not implemented)

- **Download Operations**
  - dataTransferDownloadMacro.ts
  - dataTransferDownloadStill.ts

- **Upload Operations**
  - dataTransferUploadAudio.ts
  - dataTransferUploadBuffer.ts
  - dataTransferUploadClip.ts
  - dataTransferUploadMacro.ts
  - dataTransferUploadMultiViewerLabel.ts
  - dataTransferUploadStill.ts

- **Queue Management**
  - dataTransferQueue.ts

### 4. Enums and Type Definitions (High Priority)

**Current State:** Partial enums in `Enums/` (10 files)

**Missing:** Complete enum definitions matching TypeScript

**TypeScript Reference:** `src/enums/index.ts` (comprehensive enum definitions)

#### Enum Categories to Review/Complete:
- Audio.cs ✓ (exists but needs verification)
- DVEEffect.cs ✓ (exists but needs verification)  
- Effects.cs ✓ (exists but needs verification)
- Fairlight.cs ✓ (exists but needs verification)
- Model.cs ✓ (exists but needs verification)
- Ports.cs ✓ (exists but needs verification)
- Streaming.cs ✓ (exists but needs verification)
- System.cs ✓ (exists but needs verification)
- Transitions.cs ✓ (exists but needs verification)
- Video.cs ✓ (exists but needs verification)

**Action Required:** Compare TypeScript enum definitions with existing C# enums and identify gaps.

### 5. Utility Libraries (Medium Priority)

**Current State:** Basic networking in `Net/` folder

**Missing:** Various utility functions and helpers

**TypeScript Reference:** `src/lib/` (8+ utility modules)

#### Utility Modules to Implement:

- **Command Parser** (`src/lib/atemCommandParser.ts`)
  - CommandParser ✓ (exists in Net/CommandParser.cs but needs verification)

- **Socket Management** (`src/lib/atemSocket.ts`, `src/lib/atemSocketChild.ts`)
  - AtemSocket ✓ (exists but needs feature comparison)
  - AtemSocketChild ✓ (exists but needs feature comparison)

- **Packet Building** (`src/lib/packetBuilder.ts`)
  - PacketBuilder ✓ (exists in Net/PacketBuilder.cs but needs verification)

- **ATEM Utilities** (`src/lib/atemUtil.ts`)
  - General utility functions

- **Tally Management** (`src/lib/tally.ts`)
  - listVisibleInputs and tally utilities

- **Video Mode** (`src/lib/videoMode.ts`)
  - VideoModeInfo and video mode utilities

- **MultiView Label** (`src/lib/multiviewLabel.ts`)
  - Font rendering and label generation

- **Type Definitions** (`src/lib/types.ts`)
  - Common type utilities

- **Converters** (`src/lib/converters/`)
  - Audio format converters
  - Image format converters (RLE, YUV422 to RGBA)

### 6. Extended ATEM Class Features (Medium Priority)

**Current State:** Basic `BasicAtem` class, empty `Atem` class

**Missing:** Full-featured ATEM class with all convenience methods

**TypeScript Reference:** `src/atem.ts` (1200+ lines of functionality)

#### Features to Implement:

- **Video Mode Management**
  - `get videoMode()` property
  - Video mode utilities

- **Data Transfer Integration**  
  - DataTransferManager integration ✓ (referenced but not implemented)
  - Upload/download methods for:
    - Still images
    - Audio clips  
    - Macros
    - MultiViewer labels

- **Audio Management**
  - Audio level monitoring
  - Fairlight audio controls
  - Classic audio controls

- **Macro Management**
  - Macro recording
  - Macro playback
  - Macro manipulation

- **Media Management**
  - Media pool operations
  - Media player controls

- **Event Handling**
  - levelChanged events ✓ (missing)
  - updatedTime events ✓ (missing)
  - Enhanced state change events

### 7. Examples and Documentation (Low Priority)

**Current State:** Basic examples in `Examples/` folder

**Missing:** Comprehensive examples and documentation

**TypeScript Reference:** `examples/` folder

#### Examples to Implement:
- Basic connection example
- State monitoring example  
- Command sending example
- Data transfer examples
- Audio control examples
- Video switching examples

---

## Implementation Priority

### Phase 1: Core Infrastructure (Critical)
1. Complete command architecture with all missing command categories
2. Implement comprehensive state management system
3. Verify and complete enum definitions

### Phase 2: Advanced Features (High Priority)  
1. Implement data transfer system
2. Complete utility libraries
3. Add missing networking features

### Phase 3: Extended Functionality (Medium Priority)
1. Implement full ATEM class with convenience methods
2. Add comprehensive event handling
3. Audio and video management features

### Phase 4: Polish and Documentation (Low Priority)
1. Add comprehensive examples
2. Improve documentation
3. Performance optimizations

---

## Technical Notes

- The TypeScript implementation supports approximately **200+ commands** across 15+ categories
- The current C# implementation has **< 10% coverage** of the TypeScript functionality
- The TypeScript implementation is production-ready and actively maintained
- The C# implementation appears to be an early prototype or conversion attempt

## Files to Study for Implementation Reference

Key TypeScript files to reference during C# implementation:

1. **Core Architecture:**
   - `src/atem.ts` - Main ATEM class
   - `src/index.ts` - Public API surface
   - `src/commands/index.ts` - Command exports

2. **State Management:**
   - `src/state/index.ts` - State structure
   - `src/state/util.ts` - State utilities

3. **Command System:**
   - `src/commands/CommandBase.ts` - Base command classes
   - Individual command files in subfolders

4. **Data Transfer:**
   - `src/dataTransfer/index.ts` - Data transfer exports
   - `src/dataTransfer/dataTransfer.ts` - Core manager

5. **Networking:**
   - `src/lib/atemSocket.ts` - Socket implementation
   - `src/lib/atemCommandParser.ts` - Command parsing

This TODO represents a significant development effort estimated at **several months of full-time development** to achieve feature parity with the TypeScript implementation.