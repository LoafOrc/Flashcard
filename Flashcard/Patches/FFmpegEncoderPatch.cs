using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using UnityEngine.Rendering;
using Zorro.Recorder;

namespace Flashcard.Patches;
[HarmonyPatch(typeof(FfmpegEncoder))]
internal static class FFmpegEncoderPatch {
    [HarmonyPrefix, HarmonyPatch(nameof(FfmpegEncoder.Encode))]
    private static void UpdateEncoderFramerate(ref byte framerate) {
        framerate = (byte)FlashcardPlugin.config.RECORDING_CLIP_FRAMERATE.Value;
    }

    [HarmonyPrefix, HarmonyPatch("RunFfmpeg")]
    private static void UpdateQualityPreset(ref string arguments) {
        if(FlashcardPlugin.config.RECORDING_CLIP_QUALITY.Value == "") return;

        string bitrateFromConfig = FlashcardPlugin.config.RECORDING_CLIP_QUALITY.Value.ToLower();
        if(bitrateFromConfig.Contains("-") || bitrateFromConfig.Contains(" ")) {
            FlashcardPlugin.Logger.LogError("Invalid clip quality: " + bitrateFromConfig);
        }
        if(!bitrateFromConfig.EndsWith("k") && !bitrateFromConfig.EndsWith("m")) {
            FlashcardPlugin.Logger.LogError("Invalid clip quality: " + bitrateFromConfig);
        }

        if(int.TryParse(bitrateFromConfig.Replace("k", ""), out int bitrate)) {
            arguments = arguments.Replace("-preset ultrafast", $"-preset ultrafast -b:v {bitrate}k");
            FlashcardPlugin.Logger.LogInfo("Updated Ffmpeg arguments");
        } else if(int.TryParse(bitrateFromConfig.Replace("m", ""), out bitrate)) {
            arguments = arguments.Replace("-preset ultrafast", $"-preset ultrafast -b:v {bitrate}M");
            FlashcardPlugin.Logger.LogInfo("Updated Ffmpeg arguments");
        } else {
            FlashcardPlugin.Logger.LogError("Failed to parse clip quality as integer: " + bitrateFromConfig);
        }


    }
}