#region

using System;
using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    public class LavaBurner : Impathable
    {
        public LavaBurner(Vector2 pos)
            : base("Backgrounds/Caged/cagedImpassable", pos, Vector2.Zero)
        {
        }

        public override void Update(ManagerHelper mH)
        {
            foreach (var agent in mH.GetNPCManager().GetNPCs())
            {
                if (CollisionHelper.IntersectPixelsPoint(agent.GetOriginPosition(), this) != new Vector2(-1))
                {
                    agent.ChangeHealth(-2, null);

                    mH.GetParticleManager()
                      .AddFire(agent.GetOriginPosition(),
                               PathHelper.Direction((float) (mH.GetRandom().NextDouble()*Math.PI*2))*100, 1, 0.05f, 1,
                               0.1f);
                }
            }

            base.Update(mH);
        }
    }
}