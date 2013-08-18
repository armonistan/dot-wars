using Microsoft.Xna.Framework;

namespace DotWars
{
    internal class BluePlayerCommander : PlayerCommander
    {
        public BluePlayerCommander(Vector2 p, ManagerHelper mH)
            : this(p, AffliationTypes.blue, mH)
        {
            //Nothing else
        }

        public BluePlayerCommander(Vector2 p, AffliationTypes aT, ManagerHelper mH)
            : base("Dots/Blue/commander_blue", aT, p, mH, 3)
        {
            //Set up indicator
            indicator = new Sprite("Effects/PI_blueCommander", GetOriginPosition(), Vector2.Zero);
            personalAffiliation = AffliationTypes.blue;

            abilityUse = 60;
        }

        protected override void UsePower(ManagerHelper mH)
        {
            if (CurrentPower() > abilityUse)
            {
                Vector2 tempPos = new Vector2(64)*PathHelper.Direction(rotation) + GetOriginPosition();

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
    }
}