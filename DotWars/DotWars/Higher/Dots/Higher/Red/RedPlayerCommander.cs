using Microsoft.Xna.Framework;

namespace DotWars
{
    internal class RedPlayerCommander : PlayerCommander
    {
        public RedPlayerCommander(Vector2 p, ManagerHelper mH)
            : this(p, AffliationTypes.red, mH)
        {
            //Nothing else
        }

        public RedPlayerCommander(Vector2 p, AffliationTypes aT, ManagerHelper mH)
            : base("Dots/Red/commander_red", aT, p, mH, 3)
        {
            personalAffiliation = AffliationTypes.red;
            //Set up indicator
            indicator = new Sprite("Effects/PI_redCommander", GetOriginPosition(), Vector2.Zero);

            abilityUse = 75;
        }

        protected override void UsePower(ManagerHelper mH)
        {
            if (CurrentPower() > abilityUse)
            {
                Vector2 tempPos = new Vector2(32)*PathHelper.Direction(rotation) + GetOriginPosition();
                mH.GetAbilityManager().AddFireball(tempPos, PathHelper.Direction(this.rotation), affiliation);

                base.UsePower(mH);
            }
        }
    }
}