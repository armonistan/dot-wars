#region

using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    public class Suicide : NPC
    {
        private double animateCounter;

        public Suicide(Vector2 p, ManagerHelper mH)
            : base("Dots/Grey/grey_suicide", p)
        {
            var temp = mH.Survival;

            maxHealth = (int) (50*temp.suicideSpawnModifier);
            health = maxHealth;
            movementSpeed = 50f + temp.suicideSpawnModifier*50f;
            affiliation = AffliationTypes.black;
            animateCounter = 0.0;
            awareness = 2000f;
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
                    NPCManager.IsNPCInRadius(agent, GetOriginPosition(), awareness))
                {
                    float distanceToEnemy = PathHelper.DistanceSquared(GetOriginPosition(), agent.GetOriginPosition());

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
            if (target != null && PathHelper.DistanceSquared(this.GetOriginPosition(), target.GetOriginPosition()) < 48f * 48f)
            {
                Explode(mH);
            }
            else if (PathHelper.DistanceSquared(this.GetOriginPosition(), FindClosestRock(mH)) < 48f*48f)
            {
                Explode(mH);
            }
        }

        protected override void Explode(ManagerHelper mH)
        {
            mH.GetParticleManager().AddExplosion(GetOriginPosition(), ((lastDamager == null) ? this : lastDamager), 300);
            Kill();

            if (lastDamager != null)
            {
                mH.GetGametype().ChangeScore(lastDamager, 1);
            }
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
                    float distanceToAgent = PathHelper.DistanceSquared(GetOriginPosition(), agent.GetOriginPosition());

                    if (distanceToAgent < shortestDistance)
                    {
                        shortestDistance = distanceToAgent;
                        tempEnemy = agent;
                    }
                }
            }

            Vector2 closetRockPosition = FindClosestRock(mH);

            if (closetRockPosition != CollisionHelper.NO_COLLIDE)
                mH.GetPathHelper().FindClearPath(GetOriginPosition(), closetRockPosition, mH, path);
            else if (tempEnemy != null)
                mH.GetPathHelper().FindClearPath(GetOriginPosition(), tempEnemy.GetOriginPosition(), mH, path);
            else
                RandomPath(mH);
        }

        private Vector2 FindClosestRock(ManagerHelper mH)
        {
            float closestDistance = float.PositiveInfinity;
            float distance;
            LargeRock closestRock = null;

            foreach (LargeRock rock in mH.GetAbilityManager().GetLargeRocks())
            {
                distance = PathHelper.DistanceSquared(rock.GetOriginPosition(), GetOriginPosition());
                if (distance < closestDistance && rock.IsFullyUp())
                {
                    closestDistance = distance;
                    closestRock = rock;
                }
            }

            return (closestRock != null ? closestRock.GetOriginPosition() : CollisionHelper.NO_COLLIDE);
        }
    }
}