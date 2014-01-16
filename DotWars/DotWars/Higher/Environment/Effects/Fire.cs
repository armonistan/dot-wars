#region

using System;
using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    public class Fire : Particle
    {
        #region Declarations

        private readonly double spawnTime;
        private double timer;

        #endregion

        public Fire()
        {
            timer = 0.1f; //Immediately spawn smoke
            spawnTime = 0.15f; //Spawn smoke and animate every thenth of a second
        }

        public void Set(Vector2 p, Vector2 v, float dT, float d, float t, float rA, ManagerHelper mH)
        {
            base.Set("Effects/particle_fire", p, v, dT, d, t, rA, mH);

            frameIndex = mH.GetRandom().Next(4);
            rotation = (float) (mH.GetRandom().NextDouble()*Math.PI)*2;
        }

        public override void Update(ManagerHelper mH)
        {
            if (drawTime > 0)
            {
                if (timer > spawnTime)
                {
                    //Animate the fire
                    frameIndex++;
                    if (frameIndex > 3)
                    {
                        frameIndex = 0;
                    }
                    timer = 0;

                    mH.GetParticleManager()
                      .AddStandardSmoke(GetOriginPosition(), 1);
                }
                else
                {
                    timer += mH.GetGameTime().ElapsedGameTime.TotalSeconds;
                }
            }

            base.Update(mH);
        }
    }
}