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

            maxHealth = (int) (75*temp.suicideSpawnModifier);
            health = maxHealth;
            movementSpeed = (int) (100*temp.suicideSpawnModifier);
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
            return
                mH.GetNPCManager()
                  .GetClosestInList(
                      mH.GetNPCManager()
                        .GetAllButAlliesInDirection(affiliation, GetOriginPosition(), sight, rotation, vision), this);
        }

        protected override void Behavior(ManagerHelper mH)
        {
            target = TargetDecider(mH);

            //TODO: Make this compatable with NPC death code
            if (target != null && CollisionHelper.IntersectPixelsRadius(this, target, 24, 24) != new Vector2(-1))
                Explode(mH);
            else if (PathHelper.Distance(this.GetOriginPosition(), FindClosestRock(mH)) < 48)
                Explode(mH);
        }

        protected override bool ProjectileCheck(ManagerHelper mH)
        {
            //TODO: Make this compatable with rest of code
            if (health <= 0)
            {
                Explode(mH, lastDamager);
                return true;
            }
            else
            {
                foreach (Projectile p in mH.GetProjectileManager().GetProjectiles())
                {
                    if (p.GetAffiliation() != affiliation &&
                        CollisionHelper.IntersectPixelsSimple(this, p) != new Vector2(-1))
                    {
                        lastDamagerDirection = PathHelper.DirectionVector(GetOriginPosition(), p.GetOriginPosition());
                        ChangeHealth(-1*p.GetDamage(), p.GetCreator());
                        counter = 0;

                        if (health <= 0)
                        {
                            if (p.GetAffiliation() != AffliationTypes.grey)
                                mH.GetGametype().ChangeScore(p.GetCreator(), 1);

                            return true;
                        }

                        p.SetDrawTime(0);
                    }

                    else
                        counter += mH.GetGameTime().ElapsedGameTime.TotalSeconds;

                    if (counter > 2)
                    {
                        counter = 0;
                        lastDamagerDirection = Vector2.Zero;
                    }
                }
            }

            return false;
        }

        protected override void Explode(ManagerHelper mH)
        {
            mH.GetParticleManager().AddExplosion(GetOriginPosition(), this, 300);
            mH.GetNPCManager().Remove(this);
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

        protected override Path NewPath(ManagerHelper mH)
        {
            NPC tempEnemy = mH.GetNPCManager()
                              .GetClosestInList(mH.GetNPCManager().GetAllButAllies(affiliation), GetOriginPosition());
            Vector2 closetRockPosition = FindClosestRock(mH);

            if (closetRockPosition != new Vector2(-1, -1))
                return mH.GetPathHelper().FindClearPath(GetOriginPosition(), closetRockPosition, mH);
            else if (tempEnemy != null)
                return mH.GetPathHelper().FindClearPath(GetOriginPosition(), tempEnemy.GetOriginPosition(), mH);
            else
                return RandomPath(mH);
        }

        private Vector2 FindClosestRock(ManagerHelper mH)
        {
            float closestDistance = 10000;
            float distance = 0;
            LargeRock closestRock = null;

            foreach (LargeRock rock in mH.GetAbilityManager().GetLargeRocks())
            {
                distance = PathHelper.Distance(rock.GetOriginPosition(), this.GetOriginPosition());
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestRock = rock;
                }
            }

            return (closestRock != null ? closestRock.GetOriginPosition() : new Vector2(-1,-1));
        }
    }
}