using HarmonyLib;
using Photon.Pun;
using System;
using UnityEngine;

namespace Flashcard.Patches;
[HarmonyPatch(typeof(SurfaceNetworkHandler))]
internal static class SurfaceNetworkHandlerPatch {
    [HarmonyPostfix, HarmonyPatch(nameof(SurfaceNetworkHandler.InitSurface))]
    internal static void SpawnOnStartRun() {
        if(!PhotonNetwork.IsMasterClient) return;

        SpawnExtraCamera();
    }

    [HarmonyPostfix, HarmonyPatch(nameof(SurfaceNetworkHandler.OnSlept))]
    internal static void SpawnOnNewDay() {
        if(!PhotonNetwork.IsMasterClient) return;
        if(GameObject.FindObjectsOfType<VideoCamera>().Length >= 2) return;
        SpawnExtraCamera();
    }

    static void SpawnExtraCamera() {
        if(!PhotonNetwork.IsMasterClient) return;
        if(FlashcardPlugin.config.UPGRADES_EXTRA_CAMERA.Value == "disabled") return;
        FlashcardPlugin.Logger.LogInfo("Spawning extra camera!");
        PickupHandler.CreatePickup(
            1,
            new ItemInstanceData(Guid.NewGuid()),
            new Vector3(-14.842f, 2.418f, 8.776f),
            Quaternion.Euler(0, -67.18f, 0)
        );
    }
}
