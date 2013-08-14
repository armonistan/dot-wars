using Microsoft.Xna.Framework;

namespace DotWars
{
    public class YellowDefensiveAssaultGrunt : DefensiveAssaultGrunt
    {
        public YellowDefensiveAssaultGrunt(Vector2 p)
            : base("Dots/Yellow/grunt_yellow", p)
        {
            affiliation = AffliationTypes.yellow;
        }
    }
}