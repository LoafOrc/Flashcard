using MyceliumNetworking;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Steamworks;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Yoga;
using Zorro.Core.Serizalization;
using Zorro.PhotonUtility;
using static UnityEngine.Rendering.ReloadAttribute;

namespace Flashcard.Networking;
internal class FlashcardVideoUploader {
    uint NetworkID => FlashcardPlugin.networkID;

    internal static FlashcardVideoUploader Instance;

    internal FlashcardVideoUploader() {
        MyceliumNetwork.RegisterNetworkObject(this, NetworkID);
        Instance = this;
    }

    internal IEnumerator SendVideoChunks(List<byte[]> videoChunks, ClipID clipID, VideoHandle videoID, ContentBuffer contentBuffer, bool isReRequest) {
        FlashcardPlugin.LogVerbose("Uploading", $"Sending Video Chunks! VideoID: {clipID.id}");
        for(ushort chunkIndex = 0; chunkIndex < videoChunks.Count; chunkIndex++) {
            byte[] chunk = videoChunks[chunkIndex];
            SendVideoChunkPackage package = new SendVideoChunkPackage {
                ChunkCount = (ushort)videoChunks.Count,
                VideoChunkData = chunk,
                ChunkIndex = chunkIndex,
                VideoHandle = videoID,
                ClipID = clipID
            };
            if(chunkIndex == 0) {
                BinarySerializer binarySerializer = new BinarySerializer(512, Allocator.Persistent);
                contentBuffer.Serialize(binarySerializer);
                package.ContentEventData = binarySerializer.buffer;
            }

            NativeArray<byte> buffer = package.Serialize().buffer;
            byte[] data = new byte[buffer.Length];
            ByteArrayConvertion.MoveToByteArray(ref buffer, ref data);
            buffer.Dispose();

            for (int i = 0; i < MyceliumNetwork.Players.Length; i++) {
                if(MyceliumNetwork.Players[i] == SteamUser.GetSteamID()) continue;
                
                MyceliumNetwork.RPCTarget(NetworkID, nameof(RPC_RecieveChunk),  MyceliumNetwork.Players[i], ReliableType.Reliable, data);
            }

            FlashcardPlugin.LogVerbose("Uploading", $"Uploaded a chunk! VideoID: {clipID.id}, chunkIndex: {chunkIndex}");
            yield return new WaitForSeconds(FlashcardPlugin.config.UPLOADING_DELAY_BETWEEN_PACKETS.Value);
        }
    }

    [CustomRPC]
    void RPC_RecieveChunk(byte[] data) {
        FlashcardPlugin.LogVerbose("Uploading", "Recieved Video Chunk! Deserializing and giving it back to PhotonSendVideoHandler.");

        NativeArray<byte> nativeArray = new NativeArray<byte>(data.Length, Allocator.Temp, NativeArrayOptions.ClearMemory);
        ByteArrayConvertion.MoveFromByteArray(ref data, ref nativeArray);
        BinaryDeserializer binaryDeserializer = new BinaryDeserializer(nativeArray);
        SendVideoChunkPackage sendVideoChunkPackage = new SendVideoChunkPackage();
        sendVideoChunkPackage.DeserializeData(binaryDeserializer);
        binaryDeserializer.Dispose();
        RetrievableSingleton<PhotonSendVideoHandler>.Instance.RecieveClipChunk(sendVideoChunkPackage);
    }
}
