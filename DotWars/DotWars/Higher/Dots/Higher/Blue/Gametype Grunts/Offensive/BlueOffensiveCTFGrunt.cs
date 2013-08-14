using Microsoft.Xna.Framework;

namespace DotWars
{
    public class BlueOffensiveCTFGrunt : OffensiveCTFGrunt
    {
        public BlueOffensiveCTFGrunt(Vector2 p)
            : base("Dots/Blue/grunt_blue", p)
        {
            affiliation = AffliationTypes.blue;
        }
    }
}