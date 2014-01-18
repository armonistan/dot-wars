#region

using System;
using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    public class Gut : Particle
    {
        #region Declartions

        private int gutFrame;
        private bool turnRight;

        #endregion

        public override void Update(ManagerHelper mH)
        {
            //Slow it to a stop
            if (velocity.Length() > 5)
            {
                Turn(((turnRight) ? -1f : 1f)*MathHelper.Pi/5*(velocity.Length()/64f));

                //Spawn blood
                if (mH.GetRandom().Next(100) < 0)
                {
                    mH.GetParticleManager()
                      .AddFire(GetOriginPosition(),
                               PathHelper.Direction((float) (mH.GetRandom().NextDouble()*Math.PI*2))*100, 2, 0.03f, 1, 1);
                }
            }
            else if (velocity.Length() != 0)
            {
                velocity *= 0;
            }

            //Prevent body parts from intersecting surroundings
            foreach (Environment e in mH.GetEnvironmentManager().GetStaticBlockers())
            {
                int tempCollide = CollisionHelper.IntersectPixelsDirectional(this, e);
                if (tempCollide != -1)
                {
                    velocity = CollisionHelper.CollideDirectional(GetOriginPosition(), tempCollide)*velocity.Length()*
                               0.8f;
                }
            }

            base.Update(mH);
        }

        public void Set(NPC n, int f, ManagerHelper mH)
        {
            #region Setup

            gutFrame = f;

            string tempAsset = "";

            if (n is Grunt)
            {
                switch (n.GetAffiliation())
                {
                    case NPC.AffliationTypes.red:
                        tempAsset = "Effects/Guts/Red/deadGruntRed";
                        break;
                    case NPC.AffliationTypes.blue:
                        tempAsset = "Effects/Guts/Blue/deadGruntBlue";
                        break;
                    case NPC.AffliationTypes.green:
                        tempAsset = "Effects/Guts/Green/deadGruntGreen";
                        break;
                    case NPC.AffliationTypes.yellow:
                        tempAsset = "Effects/Guts/Yellow/deadGruntYellow";
                        break;
                }
            }
            else if (n is Gunner)
            {
                switch (n.GetAffiliation())
                {
                    case NPC.AffliationTypes.red:
                        tempAsset = "Effects/Guts/Red/deadGunnerRed";
                        break;
                    case NPC.AffliationTypes.blue:
                        tempAsset = "Effects/Guts/Blue/deadGunnerBlue";
                        break;
                    case NPC.AffliationTypes.green:
                        tempAsset = "Effects/Guts/Green/deadGunnerGreen";
                        break;
                    case NPC.AffliationTypes.yellow:
                        tempAsset = "Effects/Guts/Yellow/deadGunnerYellow";
                        break;
                }
            }
            else if (n is Juggernaut)
            {
                switch (n.GetAffiliation())
                {
                    case NPC.AffliationTypes.red:
                        tempAsset = "Effects/Guts/Red/deadJuggRed";
                        break;
                    case NPC.AffliationTypes.blue:
                        tempAsset = "Effects/Guts/Blue/deadJuggBlue";
                        break;
                    case NPC.AffliationTypes.green:
                        tempAsset = "Effects/Guts/Green/deadJuggGreen";
                        break;
                    case NPC.AffliationTypes.yellow:
                        tempAsset = "Effects/Guts/Yellow/deadJuggYellow";
                        break;
                }
            }
            else if (n is Bombardier)
            {
                switch (n.GetAffiliation())
                {
                    case NPC.AffliationTypes.red:
                        tempAsset = "Effects/Guts/Red/deadBombRed";
                        break;
                    case NPC.AffliationTypes.blue:
                        tempAsset = "Effects/Guts/Blue/deadBombBlue";
                        break;
                    case NPC.AffliationTypes.green:
                        tempAsset = "Effects/Guts/Green/deadBombGreen";
                        break;
                    case NPC.AffliationTypes.yellow:
                        tempAsset = "Effects/Guts/Yellow/deadBombYellow";
                        break;
                }
            }
            else if (n is Sniper)
            {
                switch (n.GetAffiliation())
                {
                    case NPC.AffliationTypes.red:
                        tempAsset = "Effects/Guts/Red/deadSniperRed";
                        break;
                    case NPC.AffliationTypes.blue:
                        tempAsset = "Effects/Guts/Blue/deadSniperBlue";
                        break;
                    case NPC.AffliationTypes.green:
                        tempAsset = "Effects/Guts/Green/deadSniperGreen";
                        break;
                    case NPC.AffliationTypes.yellow:
                        tempAsset = "Effects/Guts/Yellow/deadSniperYellow";
                        break;
                }
            }
            else if (n is Medic)
            {
                switch (n.GetAffiliation())
                {
                    case NPC.AffliationTypes.red:
                        tempAsset = "Effects/Guts/Red/deadMedicRed";
                        break;
                    case NPC.AffliationTypes.blue:
                        tempAsset = "Effects/Guts/Blue/deadMedicBlue";
                        break;
                    case NPC.AffliationTypes.green:
                        tempAsset = "Effects/Guts/Green/deadMedicGreen";
                        break;
                    case NPC.AffliationTypes.yellow:
                        tempAsset = "Effects/Guts/Yellow/deadMedicYellow";
                        break;
                }
            }
            else if (n is Specialist)
            {
                switch (n.GetAffiliation())
                {
                    case NPC.AffliationTypes.red:
                        tempAsset = "Effects/Guts/Red/deadSpecialistRed";
                        break;
                    case NPC.AffliationTypes.blue:
                        tempAsset = "Effects/Guts/Blue/deadSpecialistBlue";
                        break;
                    case NPC.AffliationTypes.green:
                        tempAsset = "Effects/Guts/Green/deadSpecialistGreen";
                        break;
                    case NPC.AffliationTypes.yellow:
                        tempAsset = "Effects/Guts/Yellow/deadSpecialistYellow";
                        break;
                }
            }
            else if (n is Commander)
            {
                if (n is RedCommander || n is RedPlayerCommander)
                {
                    tempAsset = "Effects/Guts/Red/deadCommanderRed";
                }
                else if (n is BlueCommander || n is BluePlayerCommander)
                {
                    tempAsset = "Effects/Guts/Blue/deadCommanderBlue";
                }
                else if (n is GreenCommander || n is GreenPlayerCommander)
                {
                    tempAsset = "Effects/Guts/Green/deadCommanderGreen";
                }
                else if (n is YellowCommander || n is YellowPlayerCommander)
                {
                    tempAsset = "Effects/Guts/Yellow/deadCommanderYellow";
                }
            }

            Turn(n.GetRotation());

            //Randomly turn right or left
            turnRight = (mH.GetRandom().Next(2) == 0);

            //Make the gun frame move slower
            if (f == 3)
            {
                velocity *= 0.5f;
            }

            #endregion

            float moveSpeed = MathHelper.Clamp(n.velocity.Length(), 0f, 50f);

            base.Set(tempAsset, n.GetOriginPosition(),
                     PathHelper.Direction(n.GetRotation() + (float) (mH.GetRandom().NextDouble() - 0.5))*
                     moveSpeed,
                     3, 0.05f, 1, 0, mH);

            frameIndex = gutFrame;
        }
    }
}