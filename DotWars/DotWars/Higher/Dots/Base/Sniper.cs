using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace DotWars
{
    public class Sniper : NPC
    {
        private bool threatened; //variable used to see if sniper feels unsecure
        private float campingCounter;
        private float campingEnd;
        private const float TURN_AMOUNT = 0.1f;

        protected Sniper(String aN, Vector2 p)
            : base(aN, p)
        {
            health = 90;//Lightly armored, the sniper can take a little bit more than a medic but thats it. Slightly below average health
            maxHealth = health; //The units starting health will always be his max health
            movementSpeed = 130; //The sniper must rely on speed more than armor. Above average speed
            shootingSpeed = 2; //Snipers are one shot one kills. Slow reload time

            awareness = 150;
            vision = (float) Math.PI/2;
            sight = 750;
            turningSpeed = (float) Math.PI/20;

            pathTimerEnd = 100;
            path = null;

            threatened = true;
            campingCounter = 11;
            campingEnd = 5;
        }

        protected override void Behavior(ManagerHelper mH)
        {
            //am i threatened?
            List<NPC> annoying = mH.GetNPCManager().GetAllButAlliesInRadius(affiliation, GetOriginPosition(), awareness);
            //also temp
            threatened = (annoying.Count > 0) || campingCounter >= campingEnd;

            target = TargetDecider(mH);

            if (path == null)
            {
                if (threatened)
                {
                    path = NewPath(mH);
                }
                else
                {
                    //If nothing to shoot at, turn around
                    if (target == null)
                    {
                        Turn(TURN_AMOUNT);
                        campingCounter += (float) mH.GetGameTime().ElapsedGameTime.TotalSeconds;
                    }
                    else
                    {
                        if (shootingCounter > shootingSpeed)
                        {
                            shootingCounter = 0;
                            Shoot(mH);
                        }
                        else
                        {
                            shootingCounter += mH.GetGameTime().ElapsedGameTime.TotalSeconds;
                        }
                    }
                }
            }
            else if (path.Count == 0)
            {
                path = null;
                campingCounter = 0;
            }
        }

        protected override void Shoot(ManagerHelper mH)
        {
            Vector2 tempPos = PathHelper.Direction(rotation + (float)(Math.PI / 2)) * new Vector2(10);

            mH.GetProjectileManager()
              .AddProjectile("Projectiles/bullet_standard", GetOriginPosition() + tempPos, this,
                             PathHelper.Direction(rotation + (float)mH.GetRandom().NextDouble() / 8 - 0.0625f) * 1200, 90,
                             false, true, 5);

            mH.GetAudioManager().Play("sniperShoot", AudioManager.RandomVolume(mH),
                AudioManager.RandomPitch(mH), 0, false);
        }

        protected override Path NewPath(ManagerHelper mH)
        {
            List<Vector2> sniperSpots = mH.GetLevel().GetSniperSpots();
            Vector2 endPoint = GetOriginPosition();

            foreach (Vector2 v in sniperSpots)
            {
                if (mH.GetNPCManager().GetAllButAlliesInRadius(affiliation, v, 200).Count == 0)
                {
                    endPoint = v;
                    break;
                }
            }

            return mH.GetPathHelper().FindClearPath(GetOriginPosition(), endPoint, mH);
        }
    }
}