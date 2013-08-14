using Microsoft.Xna.Framework;

namespace DotWars
{
    public class YellowGrunt : Grunt
    {
        public YellowGrunt(Vector2 p)
            : base("Dots/Yellow/grunt_yellow", p)
        {
            affiliation = AffliationTypes.yellow;
        }
    }
}