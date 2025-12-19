using HarmonyLib;
using StardewModdingAPI;
using SlimSteed.Patches;

namespace SlimSteed
{
    public class ModEntry : Mod
    {
        public override void Entry(IModHelper helper)
        {
            // 1. Read the config file (creates one if it doesn't exist)
            var config = helper.ReadConfig<ModConfig>();

            // 2. Pass the config value to our static Patch class
            HorsePatch.WidthSetting = config.HorseWidth;

            // 3. Apply Patches
            var harmony = new Harmony(this.ModManifest.UniqueID);
            harmony.PatchAll();

            this.Monitor.Log($"SlimSteed loaded. Width set to: {config.HorseWidth}px", LogLevel.Debug);
        }
    }
}