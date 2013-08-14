﻿using Microsoft.Xna.Framework;

namespace DotWars
{
    public class YellowDefensiveCTFGrunt : DefensiveCTFGrunt
    {
        public YellowDefensiveCTFGrunt(Vector2 p)
            : base("Dots/Yellow/grunt_yellow", p)
        {
            affiliation = AffliationTypes.yellow;
        }
    }
}