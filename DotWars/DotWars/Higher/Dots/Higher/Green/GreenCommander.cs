#region

using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    public class GreenCommander : Commander
    {
        private int rockCounter;

        public GreenCommander(Vector2 p)
            : this(p, AffliationTypes.green)
        {
            rockCounter = 0;
        }

        public GreenCommander(Vector2 p, AffliationTypes aT)
            : base("Dots/Green/commander_green", p)
        {
            rockCounter = 0;
            affiliation = aT;
            personalAffiliation = AffliationTypes.green;
            //Set up indicator
            indicator = new Sprite("Effects/PI_greenCommander", GetOriginPosition());
            abilityCharge = .3;
            abilityUse = 55;
        }

        protected override void UsePower(ManagerHelper mH)
        {
            if (CurrentPower() > abilityUse && !mH.GetAbilityManager().HasReachedLargeRockCap())
            {
                Vector2 tempPos;

                if (GetPercentHealth() < .5)
                    tempPos = new Vector2(64)*lastDamagerDirection + GetOriginPosition();
                else
                    tempPos = new Vector2(64)*PathHelper.Direction(rotation) + GetOriginPosition();
                tempPos.X = tempPos.X - (tempPos.X%32) + 16;
                tempPos.Y = tempPos.Y - (tempPos.Y%32) + 16;

                if (tempPos.X > 0 && tempPos.X < mH.GetLevelSize().X &&
                    tempPos.Y > 0 && tempPos.Y < mH.GetLevelSize().Y &&
                    !PathHelper.IsNodeBlocked(tempPos))
                {
                    mH.GetAbilityManager().AddLargeRock(tempPos, affiliation);

                    base.UsePower(mH);
                    UpdatePowerStatistic();
                }
            }
        }

        public override bool ShouldUsePower(ManagerHelper mH)
        {
            if (!mH.GetAbilityManager().HasReachedLargeRockCap())
            {
                if (mH.GetGametype() is Survival)
                {
                    if (target != null &&
                        PathHelper.DistanceSquared(target.GetOriginPosition(), GetOriginPosition()) < 200*200)
                        return true;
                }
                else
                {
                    if (GetPercentHealth() < .5 && lastDamagerDirection != Vector2.Zero)
                        return true;

                    int enemyCount = 0;

                    foreach (var agent in mH.GetNPCManager().GetNPCs())
                    {
                        if (agent.GetAffiliation() != affiliation &&
                            NPCManager.IsNPCInRadius(agent, GetOriginPosition(), 200))
                        {
                            enemyCount++;
                        }
                    }

                    if (enemyCount > 3)
                        return true;
                }
            }
            return base.ShouldUsePower(mH);
        }

        public override void UpdatePowerStatistic()
        {
            rockCounter++;
        }

        public override int GetPowerStatistic()
        {
            int tempBeforeCounterIsReset = rockCounter;
            rockCounter = 0;

            return tempBeforeCounterIsReset;
        }
    }
}