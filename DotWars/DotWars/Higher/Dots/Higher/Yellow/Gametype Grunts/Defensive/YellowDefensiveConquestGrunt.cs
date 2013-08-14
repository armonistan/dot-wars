using Microsoft.Xna.Framework;

namespace DotWars
{
    public class YellowDefensiveConquestGrunt : DefensiveConquestGrunt
    {
        public YellowDefensiveConquestGrunt(Vector2 p)
            : base("Dots/Yellow/grunt_yellow", p)
        {
            affiliation = AffliationTypes.yellow;
        }
    }
}