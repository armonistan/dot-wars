#region

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    public class Destructable : Environment
    {
        #region Declarations

        protected int health;

        #endregion

        public Destructable(String a, Vector2 p, int h)
            : base(a, p, Vector2.Zero)
        {
            health = h;
            drag = 0;
        }

        public override void Update(ManagerHelper mH)
        {
            //If the health is run out, kill self
            if (ProjectileCheck(mH))
            {
                DeathCode(mH);
                return;
            }

            base.Update(mH);
        }

        public virtual bool ProjectileCheck(ManagerHelper mH)
        {
            if (health <= 0)
            {
                SetShouldRemove(true);
                return true;
            }

            Queue<Projectile> tempProjectiles = mH.GetProjectileManager().GetProjectiles();
            foreach (Projectile p in tempProjectiles)
            {
                if (CollisionHelper.IntersectPixelsPoint(p.GetOriginPosition(), this) != CollisionHelper.NO_COLLIDE &&
                    p.GetDrawTime() > 0)
                {
                    health -= p.GetDamage();
                    p.SetDrawTime(0f);

                    if (health <= 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public virtual void DeathCode(ManagerHelper mH)
        {
            //nothing
        }

        public void setHealth(int newHealth)
        {
            this.health = newHealth;
        }
    }
}