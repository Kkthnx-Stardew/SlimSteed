using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Characters;

namespace SlimSteed.Patches
{
    [HarmonyPatch(typeof(Horse), nameof(Horse.GetBoundingBox))]
    internal static class HorsePatch
    {
        // This variable is set by ModEntry when the game loads
        public static int WidthSetting = 40;

        static void Postfix(Horse __instance, ref Rectangle __result)
        {
            if (__instance == null)
                return;

            // Use the width from config (clamped to prevent breaking things)
            // Minimum 16px, Max 64px (Vanilla)
            int width = System.Math.Clamp(WidthSetting, 16, 64);

            // Center the new hitbox
            // Formula: CenterOffset = (StandardTile / 2) - (NewWidth / 2)
            // Note: Use 32f as the center point of a 64px tile
            float centerOffset = 32f - (width / 2f);

            __result = new Rectangle(
                (int)(__instance.Position.X + centerOffset),
                __result.Y,
                width,
                __result.Height
            );
        }
    }
}