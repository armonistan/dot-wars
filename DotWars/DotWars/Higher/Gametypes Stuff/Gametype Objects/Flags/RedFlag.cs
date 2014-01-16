#region

using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    public class RedFlag : Flag
    {
        public RedFlag(Vector2 p)
            : base("Objectives/flag_red", p, NPC.AffliationTypes.red)
        {
        }
    }
}