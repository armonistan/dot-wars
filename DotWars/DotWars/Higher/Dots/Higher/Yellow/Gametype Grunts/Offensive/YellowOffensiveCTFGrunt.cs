﻿#region

using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    public class YellowOffensiveCTFGrunt : OffensiveCTFGrunt
    {
        public YellowOffensiveCTFGrunt(Vector2 p)
            : base("Dots/Yellow/grunt_yellow", p)
        {
            affiliation = AffliationTypes.yellow;
        }
    }
}