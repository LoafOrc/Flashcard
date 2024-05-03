using BepInEx;
using BepInEx.Logging;
using Flashcard.Config;
using Flashcard.Networking;
using HarmonyLib;
using Steamworks;
using System.Reflection;

namespace Flashcard;
[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency("RugbugRedfern.MyceliumNetworking")]
public class FlashcardPlugin : BaseUnityPlugin {
    public static FlashcardPlugin Instance { get; private set; }
    internal new static ManualLogSource Logger { get; private set; }
    internal static Harmony Harmony { get; private set; }

    internal static FlashcardConfig config { get; private set; }

    internal static uint networkID = 102873; // I LOVE MAGIC VALUES!!!

    private void Awake() {
        Logger = base.Logger;
        Instance = this;

        Logger.LogInfo("Initing Config.");
        config = new(Config);

        Logger.LogInfo("Running Harmony Patches");
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), MyPluginInfo.PLUGIN_GUID);

        Logger.LogInfo("Setting up FlashcardVideoUploader");
        new FlashcardVideoUploader();

        Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID}:{MyPluginInfo.PLUGIN_VERSION} has loaded!");
        LogVerbose(nameof(Awake), "Flashcard loaded with verbose logging!");
    }

    internal static void LogVerbose(string origin, string message) {
        if(config.DEBUGGING_VERBOSE_LOGGING.Value)
            Logger.LogInfo($"[{origin}] {message}");
    }
}
