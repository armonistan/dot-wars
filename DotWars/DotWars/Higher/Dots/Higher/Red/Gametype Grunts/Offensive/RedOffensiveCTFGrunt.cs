using Microsoft.Xna.Framework;

namespace DotWars
{
    public class RedOffensiveCTFGrunt : OffensiveCTFGrunt
    {
        public RedOffensiveCTFGrunt(Vector2 p)
            : base("Dots/Red/grunt_red", p)
        {
            affiliation = AffliationTypes.red;
        }
    }
}