using Microsoft.Xna.Framework;

namespace DotWars
{
    public class RedGrunt : Grunt
    {
        public RedGrunt(Vector2 p)
            : base("Dots/Red/grunt_red", p)
        {
            affiliation = AffliationTypes.red;
        }
    }
}