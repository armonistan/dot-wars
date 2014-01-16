#region

using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    public class GreenFlag : Flag
    {
        public GreenFlag(Vector2 p)
            : base("Objectives/flag_green", p, NPC.AffliationTypes.green)
        {
        }
    }
}