﻿#region

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    public class Sniper : NPC
    {
        private bool threatened; //variable used to see if sniper feels unsecure
        private double campingCounter;
        private readonly double campingEnd;
        private const float TURN_AMOUNT = 0.1f;

        protected Sniper(String aN, Vector2 p)
            : base(aN, p)
        {
            health = 90;
            //Lightly armored, the sniper can take a little bit more than a medic but thats it. Slightly below average health
            maxHealth = health; //The units starting health will always be his max health
            movementSpeed = 130; //The sniper must rely on speed more than armor. Above average speed
            shootingSpeed = 1.5;

            awareness = 150;
            vision = MathHelper.PiOver2;
            sight = 750;
            turningSpeed = MathHelper.Pi / 20f;

            pathTimerEnd = 100;
            path.SetMoving(false);

            threatened = true;
            campingCounter = 11.0;
            campingEnd = 5.0;
        }

        protected override void Behavior(ManagerHelper mH)
        {
            //am i threatened?
            threatened = false;
            foreach (NPC agent in mH.GetNPCManager().GetNPCs())
            {
                if (agent.GetAffiliation() != affiliation &&
                    NPCManager.IsNPCInRadius(agent, GetOriginPosition(), awareness))
                {
                    threatened = true;
                    break;
                }
            }

            threatened = threatened || campingCounter >= campingEnd;

            target = TargetDecider(mH);

            if (threatened && !path.GetMoving())
            {
                NewPath(mH);
                campingCounter = 0;
            }

            if (!path.GetMoving())
            {
                //If nothing to shoot at, turn around
                if (target == null)
                {
                    Turn(TURN_AMOUNT);
                    campingCounter += mH.GetGameTime().ElapsedGameTime.TotalSeconds;
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
            else if (path.Count == 0)
            {
                path.SetMoving(false);
                campingCounter = 0;
            }
        }

        protected override void Shoot(ManagerHelper mH)
        {
            Vector2 tempPos = PathHelper.Direction(rotation + MathHelper.PiOver2) * new Vector2(10);

            mH.GetProjectileManager()
              .AddProjectile(ProjectileManager.STANDARD, GetOriginPosition() + tempPos, this,
                             PathHelper.Direction(rotation + (float)mH.GetRandom().NextDouble() / 8 - 0.0625f) * 1200, 90,
                             false, true, 5);

            mH.GetAudioManager().Play(AudioManager.SNIPER_SHOOT, AudioManager.RandomVolume(mH),
                                      AudioManager.RandomPitch(mH), 0, false);
        }

        protected override void NewPath(ManagerHelper mH)
        {
            Vector2 endPoint = pickAPoint(mH);

            mH.GetPathHelper().FindClearPath(GetOriginPosition(), endPoint, mH, path);
        }

        private Vector2 pickAPoint(ManagerHelper mH)
        {
            List<Vector2> sniperSpots = mH.GetLevel().GetSniperSpots();
            Vector2 endPoint = GetOriginPosition();
            bool validPoint = true;
            int localCounter = 0;

            while(localCounter < 100)
            {
                validPoint = true;
                Vector2 v = sniperSpots[mH.GetRandom().Next(sniperSpots.Count)];
                foreach (NPC agent in mH.GetNPCManager().GetNPCs())
                {
                    if (agent.GetAffiliation() != affiliation &&
                        NPCManager.IsNPCInRadius(agent, GetOriginPosition(), 200))
                    {
                        validPoint = false;
                        break;
                    }
                }

                if (validPoint)
                {
                    return v;
                }

                localCounter++;
            }
            
            return sniperSpots[mH.GetRandom().Next(sniperSpots.Count)];
        }
    }
}