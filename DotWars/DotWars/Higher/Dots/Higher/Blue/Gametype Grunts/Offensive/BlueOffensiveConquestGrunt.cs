﻿using Microsoft.Xna.Framework;

namespace DotWars
{
    public class BlueOffensiveConquestGrunt : OffensiveConquestGrunt
    {
        public BlueOffensiveConquestGrunt(Vector2 p)
            : base("Dots/Blue/grunt_blue", p)
        {
            affiliation = AffliationTypes.blue;
        }
    }
}