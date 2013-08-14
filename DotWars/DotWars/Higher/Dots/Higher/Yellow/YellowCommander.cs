using Microsoft.Xna.Framework;

namespace DotWars
{
    public class YellowCommander : Commander
    {
        private readonly int abilityOffSpeed;
        private readonly int abilitySpeed;
        private readonly float abilityTime;
        private float abilityTimer;

        private bool shouldUsePower;

        public YellowCommander(Vector2 p)
            : this(p, AffliationTypes.yellow)
        {
            //Nothing else
        }

        public YellowCommander(Vector2 p, AffliationTypes aT)
            : base("Dots/Yellow/commander_yellow", p, 3)
        {
            affiliation = aT;
            personalAffiliation = AffliationTypes.yellow;
            //Set up indicator
            indicator = new Sprite("Effects/PI_yellowCommander", GetOriginPosition());

            abilityTime = abilityTimer = 0.05f;
            abilityPercent = 0.05f;
            shouldUsePower = false;
            abilitySpeed = 250;
            abilityOffSpeed = movementSpeed;
        }

        public override void Update(ManagerHelper mH)
        {
            base.Update(mH);


            if (shouldUsePower && CurrentPower() > MaxPower()*abilityPercent)
            {
                if (abilityTimer < abilityTime)
                {
                    abilityTimer += (float) mH.GetGameTime().ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    abilityTimer = 0;

                    mH.GetAbilityManager().AddLightning(position, PathHelper.Direction(velocity), affiliation);
                    base.UsePower(mH);
                }
            }
            else
            {
                abilityTimer = abilityTime;
                shouldUsePower = false;
                movementSpeed = abilityOffSpeed;
            }
        }

        protected override void UsePower(ManagerHelper mH)
        {
            if (CurrentPower() > MaxPower()*abilityPercent && !shouldUsePower)
            {
                shouldUsePower = true;
                movementSpeed = abilitySpeed;
            }
            else
            {
                shouldUsePower = false;
                movementSpeed = abilityOffSpeed;
            }
        }

        public override bool ShouldUsePower(ManagerHelper mH)
        {
            if (CurrentPower() < .5*MaxPower())
                return false;

            if (mH.GetGametype() is Assault)
            {
                var temp = (Assault) mH.GetGametype();
                Flag f = temp.GetAllyBase(temp.GetDefender()).GetMyFlag();

                if (temp.GetAttacker() == affiliation)
                {
                    if (f.status != Flag.FlagStatus.taken)
                        return (PathHelper.Distance(GetOriginPosition(), f.GetOriginPosition()) < 300);
                    else
                        return (f.GetCaptor() is YellowCommander);
                }

                else
                {
                    return (f.status != Flag.FlagStatus.home);
                }
            }

            else if (mH.GetGametype() is CaptureTheFlag)
            {
                var temp = (CaptureTheFlag) mH.GetGametype();
                Flag eF = temp.GetEnemyBase(affiliation).GetMyFlag();

                if (eF.status != Flag.FlagStatus.taken)
                {
                    return (PathHelper.Distance(GetOriginPosition(), eF.GetOriginPosition()) < 240);
                }

                else if (eF.GetCaptor() != null)
                {
                    return (eF.GetCaptor() is YellowCommander);
                }
            }

            else if (mH.GetGametype() is Survival)
                return (target != null) && (PathHelper.Distance(GetOriginPosition(), target.GetOriginPosition()) < 200);

            else
                return mH.GetNPCManager().GetAllButAlliesInRadius(affiliation, GetOriginPosition(), 120).Count > 1;

            return false;
        }
    }
}