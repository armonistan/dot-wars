using Microsoft.Xna.Framework;

namespace DotWars
{
    public class GreenGrunt : Grunt
    {
        public GreenGrunt(Vector2 p)
            : base("Dots/Green/grunt_green", p)
        {
            affiliation = AffliationTypes.green;
        }
    }
}