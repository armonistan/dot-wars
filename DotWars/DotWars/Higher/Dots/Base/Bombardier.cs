using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace DotWars
{
    public class Bombardier : NPC
    {
        protected double campingCounter; //search counter
        protected double campingEnd;
        protected bool threatened;
        private const float TURN_AMOUNT = 0.05f;

        public Bombardier(String aN, Vector2 p)
            : base(aN, p)
        {
            health = 100; //About average armor
            maxHealth = health; //The units starting health will always be his max health
            movementSpeed = 100; //The bombardier isn't the most athletic, average speed (still under decision
            shootingSpeed = 6; //Bombardiers call in plane. Slow "reload" time

            awareness = 75;
            sight = 550;

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
                        campingCounter += (float)mH.GetGameTime().ElapsedGameTime.TotalSeconds;
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

        private Vector2 BomberOrigin(ManagerHelper mH)
        {
            float x, y;

            //check change in x
            if ((GetOriginPosition().X - target.GetOriginPosition().X) <= 0)
                x = mH.GetLevelSize().X + 150; //REPLACE NUMBER???
            else
                x = -150; //REPLACE NUMBER???
            //check change in y
            if ((GetOriginPosition().Y - target.GetOriginPosition().Y) <= 0)
                y = mH.GetLevelSize().Y + 150;
            else
                y = -150;

            return new Vector2(x, y);
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

        protected override NPC TargetDecider(ManagerHelper mH)
        {
            return mH.GetNPCManager().GetClosestInList(mH.GetNPCManager().GetAllButAllies(affiliation), this);
        }

        protected override void Shoot(ManagerHelper mH)
        {
            mH.GetNPCManager().Add(new Bomber(BomberOrigin(mH), affiliation, target, mH));

            mH.GetAudioManager().Play("staticCall", AudioManager.RandomVolume(mH),
                AudioManager.RandomPitch(mH), 0, false);
        }
    }
}