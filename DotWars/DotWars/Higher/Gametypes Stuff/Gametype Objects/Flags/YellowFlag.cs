﻿#region

using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    public class YellowFlag : Flag
    {
        public YellowFlag(Vector2 p)
            : base("Objectives/flag_yellow", p, NPC.AffliationTypes.yellow)
        {
        }
    }
}