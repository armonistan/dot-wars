using Microsoft.Xna.Framework;

namespace DotWars
{
    public class GreenOffensiveConquestGrunt : OffensiveConquestGrunt
    {
        public GreenOffensiveConquestGrunt(Vector2 p)
            : base("Dots/Green/grunt_green", p)
        {
            affiliation = AffliationTypes.green;
        }
    }
}