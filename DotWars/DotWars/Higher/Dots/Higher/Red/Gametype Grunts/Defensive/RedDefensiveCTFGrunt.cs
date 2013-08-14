﻿using Microsoft.Xna.Framework;

namespace DotWars
{
    public class RedDefensiveCTFGrunt : DefensiveCTFGrunt
    {
        public RedDefensiveCTFGrunt(Vector2 p)
            : base("Dots/Red/grunt_red", p)
        {
            affiliation = AffliationTypes.red;
        }
    }
}