using Microsoft.Xna.Framework;

namespace DotWars
{
    public class RedDefensiveConquestGrunt : DefensiveConquestGrunt
    {
        public RedDefensiveConquestGrunt(Vector2 p)
            : base("Dots/Red/grunt_red", p)
        {
            affiliation = AffliationTypes.red;
        }
    }
}