using System.Runtime.CompilerServices;
using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Characters;

namespace SlimSteed.Patches
{
    /// <summary>
    /// Draws the horse and its rider a few pixels to the left while riding, so the sprites sit centered
    /// on the slimmed collision box instead of hanging out over the right side of a gap.
    /// This only changes where things are drawn. Positions and collision are untouched.
    /// </summary>
    [HarmonyPatch(typeof(Character), nameof(Character.getLocalPosition))]
    internal static class RidingVisualPatch
    {
        /// <summary>Pixels to shift the drawing to the left. Zero turns this off.</summary>
        public static int Shift = 14;

        private const float StepPerTick = 1.5f;

        private sealed class Eased
        {
            public float Value;

            /// <summary>The game tick this horse was last advanced on, so a horse that is both a rider's mount and in the location's list moves once a tick.</summary>
            public int LastTick = -1;
        }

        private static readonly ConditionalWeakTable<Horse, Eased> State = new();

        /// <summary>The current eased shift for a horse, in pixels.</summary>
        public static float Current(Horse horse) => State.TryGetValue(horse, out var s) ? s.Value : 0f;

        /// <summary>Advance one horse toward its target. Called once per game tick.</summary>
        public static void Tick(Horse horse)
        {
            float target = 0f;
            if (Shift > 0 && HorsePatch.IsSlim(horse))
            {
                var rider = horse.rider;
                bool settled = rider == null || (!horse.mounting.Value && !horse.dismounting.Value && !rider.isAnimatingMount);
                if (settled)
                    target = Shift;
            }

            var eased = State.GetOrCreateValue(horse);
            if (eased.LastTick == Game1.ticks)
                return;
            eased.LastTick = Game1.ticks;

            if (eased.Value < target) eased.Value = System.Math.Min(target, eased.Value + StepPerTick);
            else if (eased.Value > target) eased.Value = System.Math.Max(target, eased.Value - StepPerTick);
        }

        [HarmonyPostfix]
        private static void Postfix(Character __instance, ref Vector2 __result)
        {
            Horse? horse = __instance as Horse ?? (__instance as Farmer)?.mount;
            if (horse == null || !State.TryGetValue(horse, out var eased) || eased.Value == 0f)
                return;

            __result.X -= eased.Value;
        }
    }
}
