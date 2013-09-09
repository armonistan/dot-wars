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
    }
}