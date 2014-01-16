#region

using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    internal class GreenPlayerCommander : PlayerCommander
    {
        private int rockCounter;

        public GreenPlayerCommander(Vector2 p, ManagerHelper mH)
            : this(p, AffliationTypes.green, mH)
        {
            rockCounter = 0;
        }

        public GreenPlayerCommander(Vector2 p, AffliationTypes aT, ManagerHelper mH)
            : base("Dots/Green/commander_green", aT, p, mH, 3)
        {
            rockCounter = 0;
            personalAffiliation = AffliationTypes.green;
            //Set up indicator
            indicator = new Sprite("Effects/PI_greenCommander", GetOriginPosition(), Vector2.Zero);
            abilityUse = 55;
        }

        protected override void UsePower(ManagerHelper mH)
        {
            if (CurrentPower() > abilityUse)
            {
                Vector2 tempPos = new Vector2(64)*PathHelper.Direction(rotation) + GetOriginPosition();
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