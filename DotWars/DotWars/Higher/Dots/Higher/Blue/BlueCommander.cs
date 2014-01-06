using Microsoft.Xna.Framework;

namespace DotWars
{
    public class BlueCommander : Commander
    {
        public BlueCommander(Vector2 p)
            : this(p, AffliationTypes.blue)
        {
            //Nothing else
        }

        public BlueCommander(Vector2 p, AffliationTypes aT)
            : base("Dots/Blue/commander_blue", p)
        {
            affiliation = aT;
            personalAffiliation = AffliationTypes.blue;
            //Set up indicator
            indicator = new Sprite("Effects/PI_blueCommander", GetOriginPosition());

            abilityUse = 70;
        }

        protected override void UsePower(ManagerHelper mH)
        {
            if (CurrentPower() > abilityUse)
            {
                for (int i = 0; i < 4; i++)
                {
                    mH.GetAbilityManager().AddWaterpool(new Vector2(32) * PathHelper.Direction(rotation + (i * (MathHelper.Pi / 2))) + GetOriginPosition(), affiliation);
                }

                base.UsePower(mH);
            }
        }

        public override bool ShouldUsePower(ManagerHelper mH)
        {
            if (mH.GetGametype() is Survival)
            {
                if (target != null && PathHelper.DistanceSquared(target.GetOriginPosition(), GetOriginPosition()) < 200 * 200)
                    return true;
            }
            else
            {
                if (GetPercentHealth() < .5)
                    return true;

                foreach (NPC ally in mH.GetNPCManager().GetAllies(affiliation))
                {
                    if (NPCManager.IsNPCInRadius(ally, GetOriginPosition(), 200) && ally.GetHealth() < (ally.GetMaxHealth() / 2))
                    {
                        return true;
                    }
                }

                //Check for number of enemies
                int enemyCount = 0;

                foreach (var agent in mH.GetNPCManager().GetNPCs())
                {
                    if (agent.GetAffiliation() != affiliation && NPCManager.IsNPCInRadius(agent, GetOriginPosition(), 200))
                    {
                        enemyCount++;
                    }
                }

                if (enemyCount > 3)
                    return true;
            }

            return base.ShouldUsePower(mH);
        }
    }
}