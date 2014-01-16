#region

using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    public class GreenDefensiveAssaultGrunt : DefensiveAssaultGrunt
    {
        public GreenDefensiveAssaultGrunt(Vector2 p)
            : base("Dots/Green/grunt_green", p)
        {
            affiliation = AffliationTypes.green;
        }
    }
}