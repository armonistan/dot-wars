using Microsoft.Xna.Framework;
using System;

namespace DotWars
{
    public class Tossable : Projectile
    {
        private int frameDirection;
        private double pulseTime;
        private double frameTimer;
        private double pulseTimer;

        private double elapsedTime;

        public Tossable() : base(null) {
            frameDirection = 1;
            frameTimer = 0.0;
            pulseTimer = 0.0;
        }

        public override void Update(ManagerHelper mH)
        {
            elapsedTime = mH.GetGameTime().ElapsedGameTime.TotalSeconds;
            if (isExplosive && drawTime <= 0)
            {
                mH.GetParticleManager().AddExplosion(GetOriginPosition(), creator, damage);
                isExplosive = false;
            }

            if (drawTime > 0)
            {
                drawTime -= (float)elapsedTime;
                pulseTimer += elapsedTime;
                frameTimer += elapsedTime;
                #region Tossable Update

                #region Keep it in the level

                //Update position
                Vector2 tempPos = position + velocity*(float) mH.GetGameTime().ElapsedGameTime.TotalSeconds;

                //Check for collisions with environment
                foreach (Environment e in mH.GetEnvironmentManager().GetStaticBlockers())
                {
                    int tempCollide = CollisionHelper.IntersectPixelsDirectionalRaw(this, tempPos, e);

                    if (tempCollide != -1)
                    {
                        velocity = CollisionHelper.CollideDirectional(velocity, tempCollide);
                    }
                }

                tempPos = position + velocity*(float) mH.GetGameTime().ElapsedGameTime.TotalSeconds;

                if ((tempPos.X < 0 || tempPos.X > mH.GetLevelSize().X - frame.Width - 0 || tempPos.Y < 0 ||
                     tempPos.Y > mH.GetLevelSize().Y - frame.Height - 0))
                {
                    if (tempPos.X <= 0)
                    {
                        velocity = new Vector2(velocity.X*-1, velocity.Y);
                        tempPos.X = 0;
                    }
                    else if (tempPos.X > mH.GetLevelSize().X - frame.Width - 0)
                    {
                        velocity = new Vector2(velocity.X*-1, velocity.Y);
                        tempPos.X = mH.GetLevelSize().X - frame.Width - 0;
                    }

                    if (tempPos.Y <= 0)
                    {
                        velocity = new Vector2(velocity.X, velocity.Y*-1);
                        tempPos.Y = 0;
                    }
                    else if (tempPos.Y > mH.GetLevelSize().Y - frame.Height - 0)
                    {
                        velocity = new Vector2(velocity.X, velocity.Y*-1);
                        tempPos.Y = mH.GetLevelSize().Y - frame.Height - 0;
                    }
                }
                position = tempPos;

                //Update frames
                frame.X = frameIndex*frame.Width;
                frame.Y = modeIndex*frame.Height;

                #endregion

                #region Finalize Direction

                foreach (Vector2 a in accelerations)
                {
                    acceleration += (a*(float) mH.GetGameTime().ElapsedGameTime.TotalSeconds);
                }

                velocity += thrust*acceleration - drag*velocity;

                accelerations.Clear();
                acceleration = Vector2.Zero;

                #endregion

                //Update position
                originPosition = position + origin;

                //Update frame
                if (frameIndex < 1)
                {
                    frameIndex = 0;
                    frameDirection = 1;
                }
                else if (frameIndex == totalFrames - 1)
                {
                    frameIndex = totalFrames - 1;
                    frameDirection = -1;
                }

                if (pulseTimer > pulseTime)
                {
                    pulseTime /= 2;
                    pulseTimer = 0;
                }

                if (frameTimer > pulseTime / (totalFrames * 2))
                {
                    frameTimer = 0;
                    frameIndex += frameDirection;
                }

                if (modeIndex < 0)
                {
                    modeIndex = 0;
                }
                else if (modeIndex >= totalModes)
                {
                    modeIndex = totalModes;
                }

                frame.X = frameIndex*frame.Width;
                frame.Y = modeIndex*frame.Height;

                #endregion

                //Spawn cool things to make it look better
                EffectSpawnCode(mH);
            }
            existenceTime -= (float) mH.GetGameTime().ElapsedGameTime.TotalSeconds;
        }

        protected override void EffectSpawnCode(ManagerHelper mH)
        {
            if (mH.GetRandom().NextDouble() < 0.5f)
            {
                base.EffectSpawnCode(mH);
            }
        }

        public override void Set(string a, Vector2 p, NPC n, Vector2 v, int d, bool iE, bool collide, float dT, ManagerHelper mH)
        {
            base.Set(a, p, n, v, d, iE, collide, dT, mH);
            pulseTime = dT *.5;
            frameTimer = 0.0;
            pulseTimer = 0.0;
            drag = 0.03f;
        }
    }
}