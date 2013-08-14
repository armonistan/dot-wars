using System;
using Microsoft.Xna.Framework;

namespace DotWars
{
    public class InDestructable : Environment
    {
        #region Declarations

        //None, for now

        #endregion

        public InDestructable(String a, Vector2 p)
            : base(a, p, Vector2.Zero)
        {
            drag = 0;
        }

        public override void Update(ManagerHelper mH)
        {
            ProjectileCheck(mH);

            base.Update(mH);
        }

        public virtual bool ProjectileCheck(ManagerHelper mH)
        {
            foreach (Projectile p in mH.GetProjectileManager().GetProjectiles())
            {
                if (p.GetDrawTime() > 0 &&
                    CollisionHelper.IntersectPixelsPoint(p.GetOriginPosition(), this) != new Vector2(-1))
                {
                    p.SetDrawTime(0);
                }
            }

            return false;
        }
    }
}