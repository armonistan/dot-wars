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
            health = 90;
            maxHealth = health; //The units starting health will always be his max health
            movementSpeed = 100; //The bombardier isn't the most athletic, average speed (still under decision
            shootingSpeed = 6; //Bombardiers call in plane. Slow "reload" time

            awareness = 100;
            vision = MathHelper.PiOver2;
            sight = 550;
            turningSpeed = MathHelper.Pi/20;

            pathTimerEnd = 100;
            path.SetMoving(true);

            threatened = true;
            campingCounter = 11;
            campingEnd = 5;
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
                }
            }

            //also temp
            threatened = threatened || campingCounter >= campingEnd;

            target = TargetDecider(mH);

            if (threatened)
            {
                NewPath(mH);
                campingCounter = 0;
            }

            if (path.GetMoving())
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

        protected override void NewPath(ManagerHelper mH)
        {
            List<Vector2> sniperSpots = mH.GetLevel().GetSniperSpots();
            Vector2 endPoint = GetOriginPosition();

            foreach (Vector2 v in sniperSpots)
            {
                bool validPoint = true;

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
                    endPoint = v;
                    break;
                }
            }

            mH.GetPathHelper().FindClearPath(GetOriginPosition(), endPoint, mH, path);
        }

        protected override NPC TargetDecider(ManagerHelper mH)
        {
            NPC closest = null;
            float closestDistance = float.PositiveInfinity;

            foreach (var affliationType in mH.GetGametype().GetTeams())
            {
                if (affliationType != affiliation)
                {
                    NPC closestForTeam =
                        mH.GetNPCManager().GetClosestInList(mH.GetNPCManager().GetAllies(affliationType), this);

                    if (closestForTeam != null)
                    {
                        float closestDistanceForTeam = PathHelper.DistanceSquared(GetOriginPosition(),
                                                                            closestForTeam.GetOriginPosition());

                        if (closestDistanceForTeam < closestDistance)
                        {
                            closest = closestForTeam;
                            closestDistance = closestDistanceForTeam;
                        }
                    }
                }
            }

            return closest;
        }

        protected override void Shoot(ManagerHelper mH)
        {
            mH.GetNPCManager().Add(new Bomber(BomberOrigin(mH), affiliation, target, mH));

            mH.GetAudioManager().Play(AudioManager.STATIC, AudioManager.RandomVolume(mH),
                AudioManager.RandomPitch(mH), 0, false);
        }
    }
}