using Microsoft.Xna.Framework;

namespace DotWars
{
    public class BlueSniper : Sniper
    {
        public BlueSniper(Vector2 p)
            : base("Dots/Blue/sniper_blue", p)
        {
            affiliation = AffliationTypes.blue;
        }
    }
}