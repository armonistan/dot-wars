#region

using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    public class BlueFlag : Flag
    {
        public BlueFlag(Vector2 p)
            : base("Objectives/flag_blue", p, NPC.AffliationTypes.blue)
        {
        }
    }
}