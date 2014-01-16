#region

using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    public class YellowOffensiveConquestGrunt : OffensiveConquestGrunt
    {
        public YellowOffensiveConquestGrunt(Vector2 p)
            : base("Dots/Yellow/grunt_yellow", p)
        {
            affiliation = AffliationTypes.yellow;
        }
    }
}