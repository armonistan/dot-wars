#region

using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    public class GreenOffensiveConquestGrunt : OffensiveConquestGrunt
    {
        public GreenOffensiveConquestGrunt(Vector2 p)
            : base("Dots/Green/grunt_green", p)
        {
            affiliation = AffliationTypes.green;
        }
    }
}