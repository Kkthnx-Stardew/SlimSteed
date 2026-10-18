using System;
using HarmonyLib;
using StardewValley;

namespace SlimSteed.Patches
{
    /// <summary>
    /// The game works out the tile "in front of me" from the rider's box. Keep that on the vanilla
    /// box so interaction reach while riding stays exactly as it was.
    /// </summary>
    [HarmonyPatch]
    internal static class GrabTilePatch
    {
        [HarmonyPatch(typeof(Character), nameof(Character.GetGrabTile))]
        [HarmonyPrefix]
        private static void GrabPrefix() => HorsePatch.VanillaDepth++;

        [HarmonyPatch(typeof(Character), nameof(Character.GetGrabTile))]
        [HarmonyFinalizer]
        private static void GrabFinalizer() => HorsePatch.VanillaDepth = Math.Max(0, HorsePatch.VanillaDepth - 1);

        [HarmonyPatch(typeof(Character), nameof(Character.GetDropLocation))]
        [HarmonyPrefix]
        private static void DropPrefix() => HorsePatch.VanillaDepth++;

        [HarmonyPatch(typeof(Character), nameof(Character.GetDropLocation))]
        [HarmonyFinalizer]
        private static void DropFinalizer() => HorsePatch.VanillaDepth = Math.Max(0, HorsePatch.VanillaDepth - 1);
    }
}
