﻿using Microsoft.Xna.Framework;

namespace DotWars
{
    public class BlueDefensiveCTFGrunt : DefensiveCTFGrunt
    {
        public BlueDefensiveCTFGrunt(Vector2 p)
            : base("Dots/Blue/grunt_blue", p)
        {
            affiliation = AffliationTypes.blue;
        }
    }
}