using Flashcard.Config;
using Flashcard.Networking;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace Flashcard.Patches;
[HarmonyPatch(typeof(PhotonSendVideoHandler))]
internal class PhotonSendVideoHandlerPatch {
    [HarmonyPrefix, HarmonyPatch(nameof(PhotonSendVideoHandler.SendVideoThroughPhoton))]
    internal static void ReallyReallyForceVideoSharingOverSteamNetwork(PhotonSendVideoHandler __instance, ref bool isReRequest) {
        isReRequest = false;
        __instance.m_UseSteamNetwork = true;
    }

    [HarmonyPrefix, HarmonyPatch(nameof(PhotonSendVideoHandler.RecieveClipChunk))]
    internal static void LogRecieveClipChunk(SendVideoChunkPackage package) {
        FlashcardPlugin.LogVerbose(nameof(PhotonSendVideoHandler.RecieveClipChunk), $"Recieved a chunk of a video! Chunk '{package.ChunkIndex}' for clip '{package.ClipID}'");
    }

    // who needs compatibility anyway?
    [HarmonyPostfix, HarmonyPatch(nameof(PhotonSendVideoHandler.SendVideoChunks))]
    internal static void FlashcardSendVideoChunks(ref IEnumerator __result, List<byte[]> videoChunks, ClipID clipID, VideoHandle videoID, ContentBuffer contentBuffer, bool isReRequest) {
        __result = FlashcardVideoUploader.Instance.SendVideoChunks(videoChunks, clipID, videoID, contentBuffer, isReRequest);
    }
}
