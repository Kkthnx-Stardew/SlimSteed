using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Characters;

namespace SlimSteed.Patches
{
    /// <summary>
    /// Narrows the horse's collision box so it fits wherever the farmer fits.
    /// Only the box width changes. X, Y and height stay vanilla, so mounting,
    /// dismounting and drawing keep using their normal math.
    /// </summary>
    [HarmonyPatch(typeof(Horse), nameof(Horse.GetBoundingBox))]
    internal static class HorsePatch
    {
        public static int Width = 48;
        public static bool ThinWhenNotRidden;

        /// <summary>While above zero, the box is left at its vanilla size. Used for the "in front of me" lookups.</summary>
        internal static int VanillaDepth;

        /// <summary>Whether this horse should currently use the slim box.</summary>
        internal static bool IsSlim(Horse horse)
        {
            return horse.rider != null || ThinWhenNotRidden;
        }

        [HarmonyPostfix]
        private static void Postfix(Horse __instance, ref Rectangle __result)
        {
            if (__result.IsEmpty || VanillaDepth > 0)
                return;

            if (!IsSlim(__instance))
                return;

            // Never widen. Vanilla already shrinks the box further while squeezing through a gate.
            if (__result.Width > Width)
                __result.Width = Width;
        }
    }
}
