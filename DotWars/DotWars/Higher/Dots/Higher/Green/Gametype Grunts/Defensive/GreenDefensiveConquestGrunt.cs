#region

using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    public class GreenDefensiveConquestGrunt : DefensiveConquestGrunt
    {
        public GreenDefensiveConquestGrunt(Vector2 p)
            : base("Dots/Green/grunt_green", p)
        {
            affiliation = AffliationTypes.green;
        }
    }
}