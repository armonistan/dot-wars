using Microsoft.Xna.Framework;

namespace DotWars
{
    public class YellowOffensiveConquestGrunt : OffensiveConquestGrunt
    {
        public YellowOffensiveConquestGrunt(Vector2 p)
            : base("Dots/Yellow/grunt_yellow", p)
        {
            affiliation = AffliationTypes.yellow;
        }
    }
}