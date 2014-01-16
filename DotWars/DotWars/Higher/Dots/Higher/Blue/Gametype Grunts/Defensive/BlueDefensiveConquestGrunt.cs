#region

using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    public class BlueDefensiveConquestGrunt : DefensiveConquestGrunt
    {
        public BlueDefensiveConquestGrunt(Vector2 p)
            : base("Dots/Blue/grunt_blue", p)
        {
            affiliation = AffliationTypes.blue;
        }
    }
}