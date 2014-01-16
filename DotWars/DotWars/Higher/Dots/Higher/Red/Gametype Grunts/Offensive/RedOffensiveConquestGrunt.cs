#region

using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    public class RedOffensiveConquestGrunt : OffensiveConquestGrunt
    {
        public RedOffensiveConquestGrunt(Vector2 p)
            : base("Dots/Red/grunt_red", p)
        {
            affiliation = AffliationTypes.red;
        }
    }
}