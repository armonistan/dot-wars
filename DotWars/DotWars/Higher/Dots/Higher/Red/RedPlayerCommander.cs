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
        }

        protected override void UsePower(ManagerHelper mH)
        {
            if (CurrentPower() > MaxPower()*0.5)
            {
                Vector2 tempPos = new Vector2(64)*PathHelper.Direction(rotation) + GetOriginPosition();
                mH.GetAbilityManager().AddFireball(tempPos, affiliation);

                base.UsePower(mH);
            }
        }
    }
}