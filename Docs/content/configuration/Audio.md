---
title: Audio Options
weight: 2
chapter: false
---

### General Options ###

<div class="imgBox"><div>
	<img src="/images/AudioOptions_General.png" />
	<span>General Options</span>
</div></div>

**Audio Device**: Selects which device is used for audio output (e.g computer speakers, or a headset)

**Sample Rate**: Selects the sample rate for the audio output -- typically, computers output at 44,100Hz or 48,000Hz, so they usually offer the best sound quality.

**Latency**: This represents the length of the buffer used in audio processing. A smaller value results in less delay between the audio and video, however, depending on the hardware used, a value that is too small may cause sound problems.

In addition, the **volume** and **panning** of each sound channel can be adjusted. For more control over the actual sound, the equalizer can be used to alter the relative strength of specific frequencies -- with work, this can be used to make the audio sound more like an actual NES would.

A number of audio effects are available in the `Effects` tab -- the Stereo Delay effect, in particular, produces a relatively nice fake stereo effect.

<div class="clear"></div>

### Advanced Options ###

<div class="imgBox"><div>
	<img src="/images/AudioOptions_Advanced.png" />
	<span>Advanced Options</span>
</div></div>

Unlike all the other options before it, the options in this section affect the way the sound is emulated.

* **Mute ultrasonic frequencies on the triangle channel**: This option mutes the triangle channel under certain conditions, which prevents it from causing popping sounds.

* **Reduce popping sounds on the DMC channel**: Similar to the previous option, but for the DMC channel -- this option prevents games from changing the output of the DMC channel too abruptly, which often causes popping sounds.

* **Swap square channel duty cycles**: This option is to mimic some older NES clones that had incorrect sound output for both of the square channels.  It greatly alters the sound in some games, and shouldn't be enabled unless you want this behavior.

* **Disable noise channel mode flag**: Very early Famicom models did not implement this feature, so this option is available to mimic early Famicom consoles. It changes the sound output of the noise channel in some games, and shouldn't be enabled unless you want this behavior.
