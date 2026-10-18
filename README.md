# SlimSteed

Slims the horse's collision box so a ridden horse fits through single-tile gaps, anywhere the farmer fits.

The collision box keeps its vanilla position and height, and only its width changes. Mounting and dismounting use the game's normal logic.

## Config

`config.json` is created on first launch.

| Option | Default | Meaning |
| --- | --- | --- |
| `Enabled` | `true` | Turn the mod on or off. |
| `Width` | `48` | Collision width in pixels, 32 to 96. Vanilla is 96, the farmer is 48, one tile is 64. |
| `SpriteShift` | `14` | Pixels to draw the horse and rider to the left while riding, so they sit centered on the slim box. 0 turns it off. |
| `ThinWhenNotRidden` | `false` | Also slim a parked horse. Off keeps it easy to click and mount. |
| `YieldToOtherMods` | `true` | Skip patching when Horse Squeeze, Horse Overhaul, Smart Horses or Pony Weight Loss Program is installed. |

## Behavior notes

- The horse collides like the farmer does. A 1-tile-wide passage that stopped the horse now lets it through, in any direction.
- Houses, coops, the bus, minecarts and mine ladders still need the rider to dismount. That is vanilla logic based on riding state, not box size, and this mod does not touch it.
- Closed gates still block. Open gates still work.
- The shift is drawing only. It eases in once the rider is fully on the horse and eases out on dismount, and it never changes a position.
- The tile the game treats as "in front" of a rider is still worked out from the vanilla box, so interaction reach while riding is unchanged.
- The horse is much longer than it is wide, so riding sideways its head or rump can still overlap a wall ahead by about half a tile. The collision box is the same size either way, so it never blocks a gap.
