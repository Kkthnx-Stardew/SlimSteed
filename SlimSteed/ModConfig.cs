namespace SlimSteed
{
    public class ModConfig
    {
        /// <summary>Turn the whole mod on or off.</summary>
        public bool Enabled { get; set; } = true;

        /// <summary>Collision width in pixels, 32 to 96. Vanilla is 96, the farmer is 48, one tile is 64.</summary>
        public int Width { get; set; } = 48;

        /// <summary>Pixels to draw the horse and rider to the left while riding so they sit centered on the slim box. 0 turns it off.</summary>
        public int SpriteShift { get; set; } = 14;

        /// <summary>Also slim the horse while nobody is riding it. Off keeps a parked horse easy to click.</summary>
        public bool ThinWhenNotRidden { get; set; } = false;

        /// <summary>Stay out of the way when another thin-horse mod is installed.</summary>
        public bool YieldToOtherMods { get; set; } = true;
    }
}
