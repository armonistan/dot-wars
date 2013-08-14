using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DotWars
{
    public class WaterPool : Sprite
    {
        //Declarations

        private readonly float animateEnd;

        private readonly float deathTime;

        private Sprite splash;
        private readonly float splashEnd;
        private NPC.AffliationTypes affiliation;

        private float animateTimer;

        public WaterPool()
            : base("Abilities/ability_blue_spread2", Vector2.Zero, Vector2.Zero)
        {
            animateEnd = 3;
            deathTime = 7;
            splashEnd = 0.2f;
            splash = new Sprite("Abilities/ability_blue_splash", GetOriginPosition());
        }

        public void Set(Vector2 p, NPC.AffliationTypes aT, ManagerHelper mH)
        {
            position = p;
            originPosition = position + origin;
            affiliation = aT;
            animateTimer = 0;
            rotation = (float) (Math.PI*2*mH.GetRandom().NextDouble());
            splash.SetOriginPosition(originPosition);
            splash.position = position;

            mH.GetAudioManager().Play("waterSound", (float)mH.GetRandom().NextDouble() / 4 + 0.5f, AudioManager.RandomPitch(mH), 0, false);
        }

        public override void LoadContent(TextureManager tM)
        {
            base.LoadContent(tM);
            splash.LoadContent(tM);
        }

        public override void Update(ManagerHelper mH)
        {
            foreach (NPC a in mH.GetNPCManager().GetNPCs())
            {
                if (CollisionHelper.IntersectPixelsDirectional(a, this) != -1)
                {
                    if (a.GetAffiliation() == affiliation)
                    {
                        if (mH.GetRandom().Next(60) == 0)
                        {
                            a.ChangeHealth(10, NPC.AffliationTypes.same);
                            mH.GetParticleManager().AddHeal(a);
                        }
                    }
                    else
                    {
                        a.AddAcceleration(a.velocity * new Vector2(-0.003f));
                    }
                }
            }

            //Animations
            if (animateTimer < animateEnd)
            {
                frameIndex = (int) (animateTimer/animateEnd*totalFrames);
            }
            if (animateTimer < splashEnd)
            {
                splash.SetFrameIndex((int)(animateTimer/splashEnd*splash.totalFrames));
            }
            animateTimer += (float) mH.GetGameTime().ElapsedGameTime.TotalSeconds +
                            (float) (mH.GetRandom().NextDouble()/1000);

            //Base Updates
            splash.Update(mH);
            base.Update(mH);
        }

        public override void Draw(SpriteBatch sB, Vector2 displacement, ManagerHelper mH)
        {
            base.Draw(sB, displacement, mH);

            if (animateTimer < splashEnd)
            {
                splash.Draw(sB, displacement, mH);
            }
        }

        public bool IsDone()
        {
            return animateTimer > deathTime;
        }
    }
}