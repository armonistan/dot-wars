#region

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    public class Medic : NPC
    {
        protected const int HEAL_NUM = 20;
        protected int healRadius;
        protected NPC healTarget;

        public Medic(String aN, Vector2 p)
            : base(aN, p)
        {
            health = 100;
            maxHealth = health; //The units starting health will always be his max health
            movementSpeed = 110;
            shootingSpeed = 0.5f; //the medic is unique in that it doesn't have a shooting rate it has a healing rate

            awareness = 300; //So he may see everyone
            vision = MathHelper.Pi;
            turningSpeed = MathHelper.Pi/10;
            healRadius = 70;

            affiliation = AffliationTypes.red;
        }

        //Returns the "threat" score of a dot. Lower is more dangerous
        public override int GetThreatLevel()
        {
            return 8;
        }

        protected override void Behavior(ManagerHelper mH)
        {
            if (TargetDecider(mH) != null &&
                PathHelper.DistanceSquared(GetOriginPosition(), TargetDecider(mH).GetOriginPosition()) < 96*96)
                target = TargetDecider(mH);
            else if (TargetDecider(mH) == null)
                target = null;

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

        protected override void Shoot(ManagerHelper mH)
        {
            foreach (NPC agent in mH.GetNPCManager().GetAllies(affiliation))
            {
                if (agent != this && NPCManager.IsNPCInRadius(agent, GetOriginPosition(), healRadius))
                {
                    if (agent.GetHealth() < agent.GetMaxHealth())
                    {
                        mH.GetParticleManager().AddHeal(agent);
                    }

                    agent.ChangeHealth(HEAL_NUM, this);
                }
            }
        }

        protected override NPC TargetDecider(ManagerHelper mH)
        {
            List<NPC> allies = mH.GetNPCManager().GetAllies(affiliation);
            NPC chosenOne = null;

            foreach (NPC a in allies)
            {
                if (a != this)
                {
                    if (chosenOne == null)
                        chosenOne = a;
                    else if (a.GetPercentHealth() < chosenOne.GetPercentHealth() && !(a is Medic))
                        chosenOne = a;
                }
            }

            return chosenOne;
        }

        protected override void SpecialPath(ManagerHelper mH)
        {
            if (target != null)
            {
                mH.GetPathHelper().FindClearPath(GetOriginPosition(), TargetDecider(mH).GetOriginPosition(), mH, path);
            }
            else
            {
                RandomPath(mH);
            }
        }
    }
}