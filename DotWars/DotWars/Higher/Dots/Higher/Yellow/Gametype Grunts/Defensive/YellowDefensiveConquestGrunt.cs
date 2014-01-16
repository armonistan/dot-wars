#region

using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    public class YellowDefensiveConquestGrunt : DefensiveConquestGrunt
    {
        public YellowDefensiveConquestGrunt(Vector2 p)
            : base("Dots/Yellow/grunt_yellow", p)
        {
            affiliation = AffliationTypes.yellow;
        }
    }
}