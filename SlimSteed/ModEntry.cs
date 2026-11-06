using System;
using System.Linq;
using HarmonyLib;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Characters;
using SlimSteed.Patches;

namespace SlimSteed
{
    public class ModEntry : Mod
    {
        private const int MinWidth = 32;
        private const int MaxWidth = 96;

        // Mods that already change the horse's collision box.
        private static readonly string[] OverlappingMods =
        {
            "jorgamun.HorseSqueeze",
            "Goldenrevolver.HorseOverhaul",
            "Sagittaeri.SmartHorses",
            "BadNetCode.PonyWeightLossProgram"
        };

        public override void Entry(IModHelper helper)
        {
            ModConfig config;
            try
            {
                config = helper.ReadConfig<ModConfig>() ?? new ModConfig();
            }
            catch (Exception ex)
            {
                // a config.json that cannot be read must not stop the mod, and the file is left alone
                this.Monitor.Log($"Could not read config.json, so the default settings are used. Fix it or delete it. ({ex.Message})", LogLevel.Warn);
                config = new ModConfig();
            }

            if (!config.Enabled)
            {
                this.Monitor.Log("Disabled in config.json, no patches applied.", LogLevel.Info);
                return;
            }

            // SMAPI registers mods one at a time as they load, so inside Entry only the mods that loaded before this one can be seen.
            // GameLaunched comes after every mod has loaded, which makes the check the same whatever the folder names sort to.
            helper.Events.GameLoop.GameLaunched += (_, _) => this.Apply(helper, config);
        }

        private void Apply(IModHelper helper, ModConfig config)
        {
            if (config.YieldToOtherMods)
            {
                string[] found = OverlappingMods.Where(this.Helper.ModRegistry.IsLoaded).ToArray();
                if (found.Length > 0)
                {
                    this.Monitor.Log(
                        $"Found {string.Join(", ", found)}. Skipping patches so the horse box is only changed once. Set YieldToOtherMods to false to apply anyway.",
                        LogLevel.Warn);
                    return;
                }
            }

            HorsePatch.Width = Math.Clamp(config.Width, MinWidth, MaxWidth);
            HorsePatch.ThinWhenNotRidden = config.ThinWhenNotRidden;
            RidingVisualPatch.Shift = Math.Clamp(config.SpriteShift, 0, 32);

            try
            {
                new Harmony(this.ModManifest.UniqueID).PatchAll();
            }
            catch (Exception ex)
            {
                this.Monitor.Log($"Could not patch Horse.GetBoundingBox, the horse is unchanged.\n{ex}", LogLevel.Error);
                return;
            }

            if (RidingVisualPatch.Shift > 0)
                helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;

            this.Monitor.Log($"Horse collision width set to {HorsePatch.Width}px.", LogLevel.Debug);
        }

        private void OnUpdateTicked(object? sender, UpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            foreach (Farmer farmer in Game1.getOnlineFarmers())
            {
                if (farmer.mount != null)
                    RidingVisualPatch.Tick(farmer.mount);
            }

            var characters = Game1.currentLocation?.characters;
            if (characters == null)
                return;

            for (int i = 0; i < characters.Count; i++)
            {
                if (characters[i] is Horse horse)
                    RidingVisualPatch.Tick(horse);
            }
        }
    }
}
