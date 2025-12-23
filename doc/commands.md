# Alphabetical list of commands

This list contains all the commands you can send to your ATEM device
as well as an explanation what each command is used for.
You can use this list to find the command that changes what you want until the
new, convenient state-change interface is implemented and you can change the state
by setting properties and execute actions by calling methods

Most of the commands has a corresponding `UpdateCommand` which is sent by the
device to inform the client about changes.

- AudioMixerHeadphonesCommand \
Used to update the properties of the headphone output on the classic audio mixer

AudioMixerInputCommand \
Used to update the properties of an input on the classic audio mixer

- AudioMixerMasterCommand \
Used to update the properties of the master channel on the classic audio mixer

- AudioMixerMonitorCommand \
Used to update the properties of the monitor output on the classic audio mixer

- AudioMixerPropertiesCommand \
Used to update whether audio follows the current video selection on the classic audio mixer

- AudioMixerResetPeaksCommand \
Used to reset the peak indicators for a given channel on the classic audio mixer.

- AudioRoutingOutputCommand \
Used to set the source and name for an audio routing entry on the fairlight mixer \
**NOTE:** The audio routing mechanism has yet to be reviewed and validated with real hardware.

- AudioRoutingSourceCommand \
Used to set the name for an audio routing entry on the fairlight mixer \
**NOTE:** The audio routing mechanism has yet to be reviewed and validated with real hardware

- FairlightMixerMasterCommand \
Used to enable the equalizer and set gains for the master channel on the fairlight mixer

- FairlightMixerMasterCompressorCommand \
Used to configure the compressor on the master channel of the fairlight mixer

- FairlightMixerMasterDynamicsResetCommand \
Used to reset a combination of the four dynamics components on the master channel of the fairlight mixer

- FairlightMixerMasterEqualizerBandCommand \
Used to set the properties of one equalizer band on the master channel of the fairlight mixer

- FairlightMixerMasterEqualizerResetCommand \
Used to reset the propertries of one equalizer band on the master channel of the fairlight mixer

- FairlightMixerMasterLimiterCommand \
Used to set the properties of the limiter on the master channel of the fairlight mixer

- FairlightMixerMasterPropertiesCommand \
Used to set whether audio follows video for the master channel of the fairlight mixer

- FairlightMixerMonitorCommand \
Used to set the gains and mute properties for the monitor channel of the fairlight mixer

- FairlightMixerMonitorSoloCommand \
Used to set the solo properties of the fairlight mixer

- FairlightMixerSourceCommand \
Used to set the properties of a fairlight mixer source

- FairlightMixerSourceCompressorCommand \
Used to set the properties of the compressor of a fairlight mixer source

- FairlightMixerSourceDynamicsResetCommand \
Used to reset a combination of the four dynamics components of a fairlight source

- FairlightMixerSourceEqualizerBandCommand \
Used to set the properties of an equalizer band on a fairlight source

- FairlightMixerSourceEqualizerResetCommand \
Used to reset the settings of an equalizer band of a fairlight source

- FairlightMixerSourceExpanderCommand \
Used to set the expander properties of a fairlight source

- FairlightMixerSourceLimiterCommand \
Used to set the properties of the limiter of a fairlight mixer source

- FairlightMixerSourceResetPeakLevelsCommand \
Used to reset the peaks for a fairlight mixer source

- FairlightMixerInputCommand \ / FairlightMixerInputV811Command \
Used to configure an input of the fairlight mixer \
The `V811` version is to be used for protocol versions 8.1.1 and higher and offers the configuration options added in that protocol version

- FairlightMixerResetPeakLevelsCommand \
Used to reset the peaks of all channels and/or the master channel of the fairlight mixer

- FairlightMixerSendLevelsCommand \
Used to tell the ATEM device whether to send the levels of the channels

- ColorGeneratorCommand \
Used to set the color a color generator generates

- DataTransferAckCommand \
Used to acknowledge receipt of data transfer

- DataTransferDataSendCommand \
Used to send data transfer content

- DataTransferDownloadRequestCommand \
Used to request a data transfer download from the ATEM device

- DataTransferFileDescriptionCommand \
Used to send file description for data transfer operations

- DataTransferUploadRequestCommand \
Used to request a data transfer upload to the ATEM device

- LockStateCommand \
Used to set the lock state for a data transfer

- DisplayClockPropertiesSetCommand \
Used to set display clock properties

- DisplayClockRequestTimeCommand \
Used to request the current display clock time

- DisplayClockStateSetCommand \
Used to start, stop or reset the display clock

- DownstreamKeyAutoCommand \ & DownstreamKeyAutoCommand \V801
Used to trigger an auto transition on a downstream keyer

- DownstreamKeyCutSourceCommand \
Used to set the cut source input for a downstream keyer

- DownstreamKeyFillSourceCommand \
Used to set the fill source input for a downstream keyer

- DownstreamKeyGeneralCommand \
Used to update downstream keyer general properties (pre-multiply, clip, gain, invert)

- DownstreamKeyMaskCommand \
Used to update downstream keyer mask properties

- DownstreamKeyOnAirCommand \
Used to set the on-air state of a downstream keyer

- DownstreamKeyRateCommand \
Used to set the transition- and animation- duration of a downstream keyer in frames

- DownstreamKeyTieCommand \
  Used to change whether the DSK should be tied to (=affected by) the next transition

- InputPropertiesCommand \
  Used to update input channel properties

- MacroActionCommand \
Used to run, stop, delete, etc. a macro

- MacroAddTimedPauseCommand \
Used to add a pause to a macro that's being recorded

- MacroPropertiesCommand \
Used to change the name and description of a macro after recording it

- MacroRecordCommand \
Used to start recording a macro

- MacroRunStatusCommand \
Used to set whether the macro player should play looped or not

- MediaPlayerSourceCommand \
Used to switch the media player to a certain still or clip

- MediaPlayerStatusCommand \
Used to control how the mediaplayer plays a clip

- MediaPoolCaptureStillCommand \
Used to tell the media player to capture the current program frame as a still

- MediaPoolClearClipCommand \
Used to delete a clip from the media pool

- MediaPoolClearStillCommand \
Used to remove a still from the media pool

- MediaPoolSetClipCommand \
Used to set the name of a clip in the media pool

- FadeToBlackAutoCommand \
Used to toggle FadeToBlack

- FadeToBlackRateCommand \
Used to set the duration of the FadeToBlack effect in frames

- MixEffectKeyAdvancedChromaPropertiesCommand \
Used to update advanced chroma key properties for an upstream keyer

- MixEffectKeyAdvancedChromaSampleCommand \
Used to update advanced chroma key sample settings for an upstream keyer

- MixEffectKeyAdvancedChromaSampleResetCommand \
Used to set the chroma sample settings

- MixEffectKeyCutSourceSetCommand \
  Used to set the source for the transparency channel of the upstream keyer

- MixEffectKeyDigitalVideoEffectsCommand \
Used to update DVE settings for an upstream keyer

- MixEffectKeyFillSourceSetCommand \
Used to set the source of the content for an upstream keyer

- MixEffectKeyFlyKeyframeCommand \
Used to set the properties of a keyframe for the UpstreamKeyer fly animation

- MixEffectKeyFlyKeyframeStoreCommand \
Used to store the current UpstreamKeyer settings in a keyframe

- MixEffectKeyLumaCommand \
Used to update luma key settings for an upstream keyer

- MixEffectKeyMaskSetCommand \
Used to set the mask properties for an UpstreamKeyer

- MixEffectKeyOnAirCommand \
Used to set the on-air state of an upstream keyer

- MixEffectKeyPatternCommand \
Used to set the pattern settings of an UpstreamKeyer

- MixEffectKeyRunToCommand \
Used to animate the upstream keyer to a specific position (predefined or keyframe)

- MixEffectKeyTypeSetCommand \
Used to choose which type an UpstreamKeyer uses and whether Fly Animations are enabled

- AutoTransitionCommand \
Used to trigger the selected transition effect linear over the configured time

- CutCommand \
Used to trigger a cut-transition

- PreviewTransitionCommand \
Used to set transition preview state for a mix effect

- TransitionDigitalVideoEffectCommand \
Used to set DVE transition settings for a mix effect

- TransitionDipCommand \
Used to set dip transition settings for a mix effect

- TransitionMixCommand \
Used to set the duration of the mix transition for a MixEffect

- TransitionPositionCommand \
Used to set the position of the current transition for a mix effect

- TransitionPropertiesCommand \
Used to set which transition to use and which keyers to add in with the next transition for a MixEffect

- TransitionStingerCommand \
Used to set the properties of the stinger transition for a mix effect

- TransitionWipeCommand \
Used to set the settings of the  wipe transition for a mix effect

- PreviewInputCommand \
Used to set the preview input source for a mix effect

- ProgramInputCommand \
Used to set the program input source for a mix effect

- RecordingIsoCommand \
Used to set whether all inputs are to be recorded

- RecordingRequestDurationCommand \
Used to request the duration of the current recording

- RecordingRequestSwitchDiskCommand \
Used to request recording to switch between disks

- RecordingSettingsCommand \
Used to set the settings of the recorder

- RecordingStatusCommand \
Used to enable/disable recording

- MultiViewerSourceCommand \
Used to set the video source for a specific MultiViewer window

- MultiViewerVuOpacityCommand \
Used to set MultiViewer VU opacity level

- MultiViewerWindowVuMeterCommand \
Used to enable or disable VU meter display for a specific MultiViewer window

- TimeConfigCommand \
Used to set the time configuration mode for the ATEM device

- VideoModeCommand \
Used to set the video mode of the ATEM device

- StartupStateClearCommand \
Used to reset the startup state to defaults

- StartupStateSaveCommand \
Used to save the current state as startup state

- StreamingAudioBitratesCommand \
Used to set the low and high bitrate for streaming

- StreamingRequestDurationCommand \
Used to request the streaming duration update

- StreamingServiceCommand \
Used to configure the streaming service

- StreamingStatusCommand \
Used to start/stop streaming

- SuperSourceBorderCommand \
Used to change the border properties of a super source

- SuperSourceBoxParametersCommand & SuperSourceBoxParametersV8Command \
Used to set the parameters of a super source box

- SuperSourcePropertiesCommand \
Used to set the general properties of a super source

- AuxSourceCommand \
Used to set the source for an auxiliary output

- TimeCommand \
Used to set the current time

- TimeRequestCommand \
Used to request the current time

