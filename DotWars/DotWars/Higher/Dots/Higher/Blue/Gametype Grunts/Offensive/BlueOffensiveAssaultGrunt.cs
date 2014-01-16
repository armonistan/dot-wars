#region

using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    public class BlueOffensiveAssaultGrunt : OffensiveAssaultGrunt
    {
        public BlueOffensiveAssaultGrunt(Vector2 p)
            : base("Dots/Blue/grunt_blue", p)
        {
            affiliation = AffliationTypes.blue;
        }
    }
}