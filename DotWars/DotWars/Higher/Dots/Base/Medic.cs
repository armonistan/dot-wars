using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace DotWars
{
    public class Medic : NPC
    {
        protected const int HEAL_NUM = 75;
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
            vision = (float) Math.PI;
            turningSpeed = (float) Math.PI/10;
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
                PathHelper.Distance(GetOriginPosition(), TargetDecider(mH).GetOriginPosition()) < 96)
                target = TargetDecider(mH);
            else if (TargetDecider(mH) == null)
                target = null;

            if (shootingCounter > shootingSpeed && target != null &&
                PathHelper.Distance(GetOriginPosition(), target.GetOriginPosition()) < 48)
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
            target.ChangeHealth(HEAL_NUM, this);
            mH.GetParticleManager().AddHeal(target);
        }

        protected override NPC TargetDecider(ManagerHelper mH)
        {
            List<NPC> allies = mH.GetNPCManager().GetAllies(affiliation, this);
            NPC chosenOne = null;

            foreach (NPC a in allies)
            {
                if (chosenOne == null)
                    chosenOne = a;
                else if (a.GetPercentHealth() < chosenOne.GetPercentHealth() && !(a is Medic))
                    chosenOne = a;
            }

            return chosenOne;
        }

        protected override Path SpecialPath(ManagerHelper mH)
        {
            if (target != null)
            {
                return mH.GetPathHelper().FindClearPath(GetOriginPosition(), TargetDecider(mH).GetOriginPosition(), mH);
            }
            else
            {
                return RandomPath(mH);
            }
        }
    }
}