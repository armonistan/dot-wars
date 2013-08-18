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

            abilityUse = 60;
        }

        protected override void UsePower(ManagerHelper mH)
        {
            if (CurrentPower() > abilityUse)
            {
                Vector2 tempPos;

                if (GetPercentHealth() < .5)
                    tempPos = GetOriginPosition();
                else
                    tempPos = new Vector2(64)*PathHelper.Direction(rotation) + GetOriginPosition();

                mH.GetAbilityManager()
                  .AddWaterpool(tempPos + new Vector2(mH.GetRandom().Next(-64, 64), mH.GetRandom().Next(-64, 64)),
                                affiliation);
                mH.GetAbilityManager()
                  .AddWaterpool(tempPos + new Vector2(mH.GetRandom().Next(-64, 64), mH.GetRandom().Next(-64, 64)),
                                affiliation);
                mH.GetAbilityManager()
                  .AddWaterpool(tempPos + new Vector2(mH.GetRandom().Next(-64, 64), mH.GetRandom().Next(-64, 64)),
                                affiliation);
                mH.GetAbilityManager()
                  .AddWaterpool(tempPos + new Vector2(mH.GetRandom().Next(-64, 64), mH.GetRandom().Next(-64, 64)),
                                affiliation);

                base.UsePower(mH);
            }
        }

        public override bool ShouldUsePower(ManagerHelper mH)
        {
            if (mH.GetGametype() is Survival)
            {
                if (target != null && PathHelper.Distance(target.GetOriginPosition(), GetOriginPosition()) < 200)
                    return true;
            }
            else
            {
                if (GetPercentHealth() < .5)
                    return true;
                foreach (NPC ally in mH.GetNPCManager().GetAlliesInRadius(affiliation, GetOriginPosition(), 200))
                {
                    if (ally.GetHealth() < (ally.GetMaxHealth() / 2))
                    {
                        return true;
                    }
                }
                if (mH.GetNPCManager().GetAllButAlliesInRadius(affiliation, GetOriginPosition(), 200).Count > 3)
                    return true;
            }

            return base.ShouldUsePower(mH);
        }
    }
}