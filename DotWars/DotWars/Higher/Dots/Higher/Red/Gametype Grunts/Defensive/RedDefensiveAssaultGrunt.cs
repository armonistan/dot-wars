#region

using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    public class RedDefensiveAssaultGrunt : DefensiveAssaultGrunt
    {
        public RedDefensiveAssaultGrunt(Vector2 p)
            : base("Dots/Red/grunt_red", p)
        {
            affiliation = AffliationTypes.red;
        }
    }
}