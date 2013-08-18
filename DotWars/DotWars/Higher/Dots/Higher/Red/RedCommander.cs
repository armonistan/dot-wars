using Microsoft.Xna.Framework;

namespace DotWars
{
    public class RedCommander : Commander
    {
        public RedCommander(Vector2 p)
            : this(p, AffliationTypes.red)
        {
            //Nothing else
        }

        public RedCommander(Vector2 p, AffliationTypes aT)
            : base("Dots/Red/commander_red", p)
        {
            affiliation = aT;
            personalAffiliation = AffliationTypes.red;
            //Set up indicator
            indicator = new Sprite("Effects/PI_redCommander", GetOriginPosition(), Vector2.Zero);

            abilityUse = 75;
        }

        protected override void UsePower(ManagerHelper mH)
        {
            if (CurrentPower() > abilityUse)
            {
                Vector2 tempPos = new Vector2(64)*PathHelper.Direction(rotation) + GetOriginPosition();
                mH.GetAbilityManager().AddFireball(tempPos, affiliation);

                base.UsePower(mH);
            }
        }

        public override bool ShouldUsePower(ManagerHelper mH)
        {
            if (target != null && PathHelper.Distance(target.GetOriginPosition(), GetOriginPosition()) < 50)
                return true;
            return false;
        }
    }
}