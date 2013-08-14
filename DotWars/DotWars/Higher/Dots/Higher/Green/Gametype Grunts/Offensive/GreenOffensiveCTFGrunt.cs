﻿using Microsoft.Xna.Framework;

namespace DotWars
{
    public class GreenOffensiveCTFGrunt : OffensiveCTFGrunt
    {
        public GreenOffensiveCTFGrunt(Vector2 p)
            : base("Dots/Green/grunt_green", p)
        {
            affiliation = AffliationTypes.green;
        }
    }
}