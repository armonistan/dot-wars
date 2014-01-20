using Microsoft.Xna.Framework;

namespace DotWars
{
    public class YellowCommander : Commander
    {
        private readonly float abilityOffSpeed;
        private readonly float abilitySpeed;

        private readonly double abilityTime;
        private double abilityTimer;

        private bool shouldUsePower;

        public YellowCommander(Vector2 p)
            : this(p, AffliationTypes.yellow)
        {
            //Nothing else
        }

        public YellowCommander(Vector2 p, AffliationTypes aT)
            : base("Dots/Yellow/commander_yellow", p)
        {
            affiliation = aT;
            personalAffiliation = AffliationTypes.yellow;
            //Set up indicator
            indicator = new Sprite("Effects/PI_yellowCommander", GetOriginPosition());

            abilityTime = abilityTimer = 0.05f;
            shouldUsePower = false;
            abilitySpeed = 250;
            abilityOffSpeed = movementSpeed;
        }

        public override void Update(ManagerHelper mH)
        {
            base.Update(mH);


            if (shouldUsePower && CurrentPower() > abilityUse)
            {
                if (abilityTimer < abilityTime)
                {
                    abilityTimer += mH.GetGameTime().ElapsedGameTime.TotalSeconds;
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
            if (CurrentPower() > abilityUse && !shouldUsePower)
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
            if (CurrentPower() < .5 * MaxPower())
                return false;

            if (mH.GetGametype() is Assault)
            {
                var temp = mH.Assault;
                Flag f = temp.GetAllyBase(temp.GetDefender()).GetMyFlag();

                if (temp.GetAttacker() == affiliation)
                {
                    if (f.status != Flag.FlagStatus.taken)
                        return (PathHelper.DistanceSquared(GetOriginPosition(), f.GetOriginPosition()) < 300 * 300);
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
                var temp = mH.CaptureTheFlag;
                Flag eF = temp.GetEnemyBase(affiliation).GetMyFlag();

                if (eF.status != Flag.FlagStatus.taken)
                {
                    return (PathHelper.DistanceSquared(GetOriginPosition(), eF.GetOriginPosition()) < 240 * 240);
                }

                else if (eF.GetCaptor() != null)
                {
                    return (eF.GetCaptor() is YellowCommander);
                }
            }

            else if (mH.GetGametype() is Survival)
                return (target != null) && (PathHelper.DistanceSquared(GetOriginPosition(), target.GetOriginPosition()) < 200 * 200);

            else
            {
                int enemyCount = 0;

                foreach (var agent in mH.GetNPCManager().GetNPCs())
                {
                    if (agent.GetAffiliation() != affiliation &&
                        NPCManager.IsNPCInRadius(agent, GetOriginPosition(), 120))
                    {
                        enemyCount++;
                    }
                }

                return enemyCount > 1;
            }

            return false;
        }
    }
}