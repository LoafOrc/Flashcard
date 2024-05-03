using HarmonyLib;

namespace Flashcard.Patches;

[HarmonyPatch(typeof(RecordingsHandler))]
static class RecordingHandlerPatch {
	[HarmonyPatch(nameof(RecordingsHandler.StartRecording)), HarmonyPostfix]
	static void LogStartRecording() {
		FlashcardPlugin.LogVerbose("RecordingHandlerPatch", "RecordingsHandler.StartRecording!!!");
	}
}