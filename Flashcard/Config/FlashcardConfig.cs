using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flashcard.Config;
internal class FlashcardConfig {
    internal ConfigEntry<string> UPGRADES_EXTRA_CAMERA;


    internal ConfigEntry<float> RECORDING_CLIP_LENGTH;
    internal ConfigEntry<int> RECORDING_CLIP_FRAMERATE;
    internal ConfigEntry<string> RECORDING_CLIP_QUALITY;

    internal ConfigEntry<int> UPLOADING_PACKET_SIZE;
    internal ConfigEntry<float> UPLOADING_DELAY_BETWEEN_PACKETS;

    internal ConfigEntry<bool> DEBUGGING_VERBOSE_LOGGING;

    internal FlashcardConfig(ConfigFile cfgFile) {
        UPGRADES_EXTRA_CAMERA = cfgFile.Bind("Upgrades", "ExtraCamera", "disabled", new ConfigDescription("Should allow the Extra Camera Upgrade?", new AcceptableValueList<string>("disabled", "always")));

        RECORDING_CLIP_LENGTH = cfgFile.Bind("Recording", "ClipLength", 120f,
            "How many seconds should the camera be allowed to record?" + Environment.NewLine +
            "ContentWarning's default is 90."
        );

        RECORDING_CLIP_FRAMERATE = cfgFile.Bind("Recording", "ClipFramerate", 24, "What framerate should the camera record at? Might cause issues extracting if not everybody is at the same framereate.\nIt's not recommended to set this above 30, doing so will lead to crazy sizes.");

        RECORDING_CLIP_QUALITY = cfgFile.Bind("Recording", "ClipQuality", "512k", "What bitrate should the video get encoded in?\nThis drastically effects the file quality. Vanilla videos are about 4mb in size after completion, Flashcard defaults can increase it to about 8mb.\nSet to empty to disable change the quality.\nAnything above 2000k/2M is excessive and shouldn't be used.");

        UPLOADING_DELAY_BETWEEN_PACKETS = cfgFile.Bind("Uploading", "DelayBetweenPackets", 0.5f, "The delay between the packets for video chunk data being sent.");

        DEBUGGING_VERBOSE_LOGGING = cfgFile.Bind("Debugging", "VerboseLogging", false, "Whether flashcard should increase it's logs. Useful for debugging issues.");
    
    
    }
}
