#region

using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    public class YellowOffensiveAssaultGrunt : OffensiveAssaultGrunt
    {
        public YellowOffensiveAssaultGrunt(Vector2 p)
            : base("Dots/Yellow/grunt_yellow", p)
        {
            affiliation = AffliationTypes.yellow;
        }
    }
}