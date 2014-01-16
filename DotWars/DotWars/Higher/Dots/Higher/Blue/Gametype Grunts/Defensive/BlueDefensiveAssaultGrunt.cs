#region

using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    public class BlueDefensiveAssaultGrunt : DefensiveAssaultGrunt
    {
        public BlueDefensiveAssaultGrunt(Vector2 p)
            : base("Dots/Blue/grunt_blue", p)
        {
            affiliation = AffliationTypes.blue;
        }
    }
}