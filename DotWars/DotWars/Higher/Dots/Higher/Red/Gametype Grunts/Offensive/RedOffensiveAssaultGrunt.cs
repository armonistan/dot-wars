using Microsoft.Xna.Framework;

namespace DotWars
{
    public class RedOffensiveAssaultGrunt : OffensiveAssaultGrunt
    {
        public RedOffensiveAssaultGrunt(Vector2 p)
            : base("Dots/Red/grunt_red", p)
        {
            affiliation = AffliationTypes.red;
        }
    }
}