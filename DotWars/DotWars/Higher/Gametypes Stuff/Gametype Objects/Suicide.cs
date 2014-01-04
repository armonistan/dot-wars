using Microsoft.Xna.Framework;

namespace DotWars
{
    public class Suicide : NPC
    {
        private double animateCounter;

        public Suicide(Vector2 p, ManagerHelper mH)
            : base("Dots/Grey/grey_suicide", p)
        {
            var temp = (Survival) mH.GetGametype();

            maxHealth = (int) (50*temp.suicideSpawnModifier);
            health = maxHealth;
            movementSpeed = (int) (50 + temp.suicideSpawnModifier * 50);
            affiliation = AffliationTypes.black;
            animateCounter = 0;
        }

        public override void Update(ManagerHelper mH)
        {
            Animate(mH);
            base.Update(mH);
        }

        protected override NPC TargetDecider(ManagerHelper mH)
        {
            NPC bestEnemy = null;
            float bestDistance = float.PositiveInfinity;

            foreach (var agent in mH.GetNPCManager().GetNPCs())
            {
                if (agent.GetAffiliation() != affiliation &&
                    NPCManager.IsNPCInRadius(agent, GetOriginPosition(), sight) &&
                    NPCManager.IsNPCInDirection(agent, GetOriginPosition(), rotation, vision, mH))
                {
                    float distanceToEnemy = PathHelper.Distance(GetOriginPosition(), agent.GetOriginPosition());

                    if (distanceToEnemy < bestDistance)
                    {
                        bestDistance = distanceToEnemy;
                        bestEnemy = agent;
                    }
                }
            }


            return bestEnemy;
        }

        protected override void Behavior(ManagerHelper mH)
        {
            target = TargetDecider(mH);

            //TODO: Make this compatable with NPC death code
            if (target != null && CollisionHelper.IntersectPixelsRadius(this, target, 24, 24) != new Vector2(-1))
            {
                Explode(mH);
            }
            else if (PathHelper.Distance(this.GetOriginPosition(), FindClosestRock(mH)) < 48)
            {
                Explode(mH);
            }
        }

        protected override void Explode(ManagerHelper mH)
        {
            mH.GetParticleManager().AddExplosion(GetOriginPosition(), ((lastDamager == null) ? this : lastDamager), 300);
            mH.GetNPCManager().Remove(this);

            if (lastDamager != null)
            {
                mH.GetGametype().ChangeScore(lastDamager, 1);
            }
        }

        private void Explode(ManagerHelper mH, NPC a)
        {
            mH.GetParticleManager().AddExplosion(GetOriginPosition(), a, 125);
            mH.GetNPCManager().Remove(this);
        }

        private void Animate(ManagerHelper mH)
        {
            if (animateCounter > .05)
            {
                if (frameIndex < 2)
                    frameIndex++;
                else
                    frameIndex = 0;

                animateCounter = 0;
            }

            else
                animateCounter += mH.GetGameTime().ElapsedGameTime.TotalSeconds;
        }

        protected override void NewPath(ManagerHelper mH)
        {
            NPC tempEnemy = null;
            float shortestDistance = float.PositiveInfinity;

            foreach (var agent in mH.GetNPCManager().GetNPCs())
            {
                if (agent.GetAffiliation() != affiliation)
                {
                    float distanceToAgent = PathHelper.Distance(GetOriginPosition(), agent.GetOriginPosition());

                    if (distanceToAgent < shortestDistance)
                    {
                        shortestDistance = distanceToAgent;
                        tempEnemy = agent;
                    }
                }
            }

            Vector2 closetRockPosition = FindClosestRock(mH);

            if (closetRockPosition != new Vector2(-1, -1))
                mH.GetPathHelper().FindClearPath(GetOriginPosition(), closetRockPosition, mH, path);
            else if (tempEnemy != null)
                mH.GetPathHelper().FindClearPath(GetOriginPosition(), tempEnemy.GetOriginPosition(), mH, path);
            else
                RandomPath(mH);
        }

        private Vector2 FindClosestRock(ManagerHelper mH)
        {
            float closestDistance = 10000;
            float distance = 0;
            LargeRock closestRock = null;

            foreach (LargeRock rock in mH.GetAbilityManager().GetLargeRocks())
            {
                distance = PathHelper.Distance(rock.GetOriginPosition(), this.GetOriginPosition());
                if (distance < closestDistance && rock.IsFullyUp())
                {
                    closestDistance = distance;
                    closestRock = rock;
                }
            }

            return (closestRock != null ? closestRock.GetOriginPosition() : new Vector2(-1,-1));
        }
    }
}