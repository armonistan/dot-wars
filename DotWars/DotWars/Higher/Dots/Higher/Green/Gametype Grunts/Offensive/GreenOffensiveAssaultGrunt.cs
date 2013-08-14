using Microsoft.Xna.Framework;

namespace DotWars
{
    public class GreenOffensiveAssaultGrunt : OffensiveAssaultGrunt
    {
        public GreenOffensiveAssaultGrunt(Vector2 p)
            : base("Dots/Green/grunt_green", p)
        {
            affiliation = AffliationTypes.green;
        }
    }
}