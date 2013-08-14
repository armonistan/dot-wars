using Microsoft.Xna.Framework;

namespace DotWars
{
    public class BlueGrunt : Grunt
    {
        public BlueGrunt(Vector2 p)
            : base("Dots/Blue/grunt_blue", p)
        {
            affiliation = AffliationTypes.blue;
        }
    }
}