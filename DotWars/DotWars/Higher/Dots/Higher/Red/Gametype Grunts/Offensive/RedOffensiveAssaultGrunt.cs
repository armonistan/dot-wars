#region

using Microsoft.Xna.Framework;

#endregion

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