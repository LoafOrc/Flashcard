using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Zorro.Recorder;

namespace Flashcard.Patches;
[HarmonyPatch(typeof(VideoCamera))]
public class VideoCameraPatch {

    [HarmonyTranspiler, HarmonyPatch(nameof(VideoCamera.ConfigItem))]
    static IEnumerable<CodeInstruction> UpdateFilmAmount(IEnumerable<CodeInstruction> instructions) {
        return new CodeMatcher(instructions)
            .MatchForward(false,
                new CodeMatch(OpCodes.Ldc_R4, 90f),
                new CodeMatch(OpCodes.Stfld, AccessTools.Field(typeof(VideoInfoEntry), "timeLeft"))
            )
            .SetOperandAndAdvance(FlashcardPlugin.config.RECORDING_CLIP_LENGTH.Value)
            .MatchForward(false,
                new CodeMatch(OpCodes.Ldc_R4, 90f),
                new CodeMatch(OpCodes.Stfld, AccessTools.Field(typeof(VideoInfoEntry), "maxTime"))
            )
            .SetOperandAndAdvance(FlashcardPlugin.config.RECORDING_CLIP_LENGTH.Value)
            .InstructionEnumeration();
    }

    [HarmonyTranspiler, HarmonyPatch(nameof(VideoCamera.StartRecording))]
    static IEnumerable<CodeInstruction> UpdateVideoResolution(IEnumerable<CodeInstruction> instructions) {
        return new CodeMatcher(instructions)
            .MatchForward(false,
                new CodeMatch(OpCodes.Ldc_I4_S, null),
                new CodeMatch(OpCodes.Stfld, AccessTools.Field(typeof(VideoRecorder), "frameRate"))
            )
            .SetOperandAndAdvance(FlashcardPlugin.config.RECORDING_CLIP_FRAMERATE.Value)
            .InstructionEnumeration();
    }
}
