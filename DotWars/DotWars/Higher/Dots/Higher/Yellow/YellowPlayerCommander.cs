using Microsoft.Xna.Framework;

namespace DotWars
{
    internal class YellowPlayerCommander : PlayerCommander
    {
        private readonly int abilityOffSpeed;
        private readonly int abilitySpeed;
        private readonly float abilityTime;
        private float abilityTimer;
        private float abilityCutoff;
        private float lightningTimer;
        private float lightningTimerRefresh;
        private bool shouldUsePower;

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

            abilityTime = abilityTimer = 1f;
            abilityPercent = 0.01f;
            shouldUsePower = false;
            abilitySpeed = 250;
            abilityOffSpeed = movementSpeed;
            abilityCutoff = 0.05f;
            lightningTimer = 50.0f;
            lightningTimerRefresh = 50.0f;
        }

        public override void Update(ManagerHelper mH)
        {
            base.Update(mH);

            if (shouldUsePower && CurrentPower() > MaxPower() * abilityCutoff)
            {
                if (abilityTimer > abilityTime)
                {
                    movementSpeed = abilitySpeed;
                    if (lightningTimer > lightningTimerRefresh)
                    {
                        mH.GetAbilityManager().AddLightning(position, PathHelper.Direction(velocity), affiliation);
                        lightningTimer = 0.0f;
                    }
                    base.UsePower(mH);
                }
            }
            else
            {
                shouldUsePower = false;
                movementSpeed = abilityOffSpeed;
                if (CurrentPower() > MaxPower() * .3)
                {
                    abilityTimer = abilityTime;
                }
                else
                {
                    abilityTimer = 0;
                }
            }

            abilityTimer += (float)mH.GetGameTime().ElapsedGameTime.TotalSeconds;
            lightningTimer += (float)mH.GetGameTime().ElapsedGameTime.TotalMilliseconds;
        }

        protected override void UsePower(ManagerHelper mH)
        {
            if (CurrentPower() > MaxPower() * abilityCutoff && !shouldUsePower)
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