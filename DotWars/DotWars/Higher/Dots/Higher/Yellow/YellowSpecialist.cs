﻿using Microsoft.Xna.Framework;

namespace DotWars
{
    public class YellowSpecialist : Specialist
    {
        public YellowSpecialist(Vector2 p)
            : base("Dots/Yellow/specialist_yellow", p)
        {
            affiliation = AffliationTypes.yellow;
        }

        //This method will be used to dictate the AI's behavior in this public class
        public override void Update(ManagerHelper mH)
        {
            base.Update(mH);
        }
    }
}