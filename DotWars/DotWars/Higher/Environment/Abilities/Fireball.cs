#region

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace DotWars
{
    internal class Fireball : Sprite
    {
        #region Declarations

        private readonly List<NPC> doomedDots;
        private readonly List<NPC> dotsSetOnFire;
        private NPC.AffliationTypes affiliation;

        private double timer;
        private const double drawTime = 0.5;
        private const double existTime = 2f;

        private const int DAMAGE = -75;

        private const float modifer = 0.95f;

        private readonly Sprite indicator;

        #endregion

        public Fireball()
            : base("Abilities/ability_red", Vector2.Zero, Vector2.Zero)
        {
            doomedDots = new List<NPC>();
            dotsSetOnFire = new List<NPC>();

            indicator = new Sprite("Abilities/ability_red2", Vector2.Zero);
        }

        public void Set(Vector2 p, Vector2 direction, NPC.AffliationTypes aT, ManagerHelper mH)
        {
            timer = 0.0;
            scale = 1;
            doomedDots.Clear();
            dotsSetOnFire.Clear();
            affiliation = aT;
            frameIndex = 0;
            originPosition = p;
            position = p - origin;
            velocity = new Vector2(300*direction.X, 300*direction.Y);

            mH.GetAudioManager()
              .Play(AudioManager.FIREBALL, (float) mH.GetRandom().NextDouble()/4 + 0.75f, AudioManager.RandomPitch(mH),
                    0, false);
        }

        public override void LoadContent(TextureManager tM)
        {
            base.LoadContent(tM);
            indicator.LoadContent(tM);
        }

        public override void Update(ManagerHelper mH)
        {
            if (frameIndex < totalFrames)
            {
                frameIndex = (int) (timer/drawTime*totalFrames);

                //Spawn fire
                if (mH.GetRandom().NextDouble() < 0.3)
                {
                    mH.GetParticleManager()
                      .AddFire(GetOriginPosition(),
                               PathHelper.Direction((float) (mH.GetRandom().NextDouble()*Math.PI*2))*100, 1, 0.05f, 1,
                               0.1f);
                }

                if (mH.GetGametype() is Survival)
                {
                    foreach (NPC a in mH.GetNPCManager().GetAllies(NPC.AffliationTypes.black))
                    {
                        if (NPCManager.IsNPCInRadius(a, GetOriginPosition(), 64*scale) && !doomedDots.Contains(a))
                        {
                            doomedDots.Add(a);

                            if (!a.GetFireStatus())
                            {
                                a.ChangeFireStatus();
                                dotsSetOnFire.Add(a);
                            }
                            a.ChangeHealth(DAMAGE, mH.GetNPCManager().GetCommander(NPC.AffliationTypes.red));
                        }
                    }
                }
                else
                {
                    foreach (NPC a in mH.GetNPCManager().GetNPCs())
                    {
                        if (a.GetAffiliation() != affiliation && NPCManager.IsNPCInRadius(a, GetOriginPosition(), 64 * this.scale) &&
                            !doomedDots.Contains(a))
                        {
                            doomedDots.Add(a);

                            if (!a.GetFireStatus())
                            {
                                a.ChangeFireStatus();
                                dotsSetOnFire.Add(a);
                            }
                            a.ChangeHealth(-50, mH.GetNPCManager().GetCommander(NPC.AffliationTypes.red));
                        }
                    }
                }
            }

            //Deal Damage
            for (int i = 0; i < doomedDots.Count; i++)
            {
                NPC a = doomedDots[i];
                if (a.GetHealth() <= 0)
                {
                    doomedDots.Remove(a);
                    i--;
                    continue;
                }

                //Spawn fire
                if (mH.GetRandom().NextDouble() < 0.1)
                {
                    mH.GetParticleManager()
                      .AddFire(a.GetOriginPosition(),
                               PathHelper.Direction((float) (mH.GetRandom().NextDouble()*Math.PI*2))*30, 1, 0.05f, 1,
                               0.1f);
                }
            }

            timer += mH.GetGameTime().ElapsedGameTime.TotalSeconds;
            scale = scale*modifer;

            base.Update(mH);
        }

        public override void Draw(SpriteBatch sB, Vector2 displacement, ManagerHelper mH)
        {
            if (timer <= drawTime)
            {
                base.Draw(sB, displacement, mH);
            }

            foreach (NPC dot in doomedDots)
            {
                indicator.position = dot.GetOriginPosition() - indicator.origin;
                indicator.SetFrameIndex(mH.GetRandom().Next(indicator.totalFrames));
                indicator.UpdateFrame();
                indicator.Draw(sB, displacement, mH);
            }
        }

        public bool IsDone()
        {
            return timer > existTime;
        }

        public void ResetFireStatus()
        {
            foreach (NPC a in dotsSetOnFire)
                a.ChangeFireStatus();
        }

        public List<NPC> GetDotsSetOnFire()
        {
            return dotsSetOnFire;
        }
    }
}