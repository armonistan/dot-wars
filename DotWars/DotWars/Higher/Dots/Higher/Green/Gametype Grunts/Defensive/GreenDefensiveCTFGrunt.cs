﻿using Microsoft.Xna.Framework;

namespace DotWars
{
    public class GreenDefensiveCTFGrunt : DefensiveCTFGrunt
    {
        public GreenDefensiveCTFGrunt(Vector2 p)
            : base("Dots/Green/grunt_green", p)
        {
            affiliation = AffliationTypes.green;
        }
    }
}