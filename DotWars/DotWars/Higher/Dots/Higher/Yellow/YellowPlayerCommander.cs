#region

using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    internal class YellowPlayerCommander : PlayerCommander
    {
        private readonly float abilityOffSpeed;
        private readonly float abilitySpeed;

        private readonly double abilityTime;
        private double abilityTimer;

        private bool shouldUsePower;

        private bool ranOut;
        private const float bottom = 5;
        private const float waitCharge = 20;

        public YellowPlayerCommander(Vector2 p, ManagerHelper mH)
            : this(p, AffliationTypes.yellow, mH)
        {
        }

        public YellowPlayerCommander(Vector2 p, AffliationTypes aT, ManagerHelper mH)
            : base("Dots/Yellow/commander_yellow", aT, p, mH, 3)
        {
            personalAffiliation = AffliationTypes.yellow;
            //Set up indicator
            indicator = new Sprite("Effects/PI_yellowCommander", GetOriginPosition(), Vector2.Zero);

            abilityTime = abilityTimer = 0.05f;
            abilityUse = 5;
            shouldUsePower = false;
            ranOut = false;

            abilitySpeed = 250;
            abilityOffSpeed = movementSpeed;
        }

        public override void Update(ManagerHelper mH)
        {
            base.Update(mH);

            if (shouldUsePower && CurrentPower() > abilityUse)
            {
                if (abilityTimer > abilityTime)
                {
                    movementSpeed = abilitySpeed;
                    mH.GetAbilityManager().AddLightning(position, PathHelper.Direction(velocity), affiliation);

                    base.UsePower(mH);
                    abilityTimer = 0;
                }
            }
            else
            {
                shouldUsePower = false;
                movementSpeed = abilityOffSpeed;
                if (CurrentPower() > MaxPower()*.3)
                {
                    abilityTimer = abilityTime;
                }
                else
                {
                    abilityTimer = 0;
                }
            }

            if (CurrentPower() < bottom)
            {
                ranOut = true;
            }

            if (ranOut && CurrentPower() > waitCharge)
            {
                ranOut = false;
            }

            abilityTimer += mH.GetGameTime().ElapsedGameTime.TotalSeconds;
        }

        protected override void UsePower(ManagerHelper mH)
        {
            if (CurrentPower() > abilityUse && !shouldUsePower && !ranOut)
            {
                shouldUsePower = true;
            }
            else
            {
                shouldUsePower = false;
            }
        }
    }
}