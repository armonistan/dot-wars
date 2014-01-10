using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DotWars
{
    internal class Fireball : Sprite
    {
        #region Declarations

        private readonly List<NPC> doomedDots;
        private readonly List<NPC> dotsSetOnFire;
        private NPC.AffliationTypes affiliation;
        private ManagerHelper managers;

        private int frameCounter;
        private const int drawFrames = 50;
        private const int existFrames = 60;
        private const int burnFrames = 3;

        private const float modifer = 0.99f;

        #endregion

        public Fireball(ManagerHelper mH)
            : base("Abilities/ability_red", Vector2.Zero, Vector2.Zero)
        {
            doomedDots = new List<NPC>();
            dotsSetOnFire = new List<NPC>();
            managers = mH;
        }

        public void Set(Vector2 p, Vector2 direction, NPC.AffliationTypes aT, ManagerHelper mH)
        {
            frameCounter = 0;
            scale = 1;
            doomedDots.Clear();
            dotsSetOnFire.Clear();
            affiliation = aT;
            frameIndex = 0;
            originPosition = p;
            position = p - origin;
            velocity = new Vector2(300 * direction.X, 300 * direction.Y);

            mH.GetAudioManager().Play(AudioManager.FIREBALL, (float)mH.GetRandom().NextDouble()/4 + 0.75f, AudioManager.RandomPitch(mH), 0, false);
        }

        public override void Update(ManagerHelper mH)
        {
            if (frameIndex < totalFrames)
            {
                frameIndex = (int)((float)frameCounter/drawFrames*totalFrames);

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
                        if (NPCManager.IsNPCInRadius(a, GetOriginPosition(), 64) && !doomedDots.Contains(a))
                        {
                            doomedDots.Add(a);

                            if (!a.GetFireStatus())
                            {
                                a.ChangeFireStatus();
                                dotsSetOnFire.Add(a);
                            }
                            a.ChangeHealth(-30, mH.GetNPCManager().GetCommander(NPC.AffliationTypes.red));
                        }
                    }
                }
                else
                {
                    foreach (NPC a in mH.GetNPCManager().GetNPCs())
                    {
                        if (a.GetAffiliation() != affiliation && NPCManager.IsNPCInRadius(a, GetOriginPosition(), 64) && !doomedDots.Contains(a))
                        {
                            doomedDots.Add(a);

                            if (!a.GetFireStatus())
                            {
                                a.ChangeFireStatus();
                                dotsSetOnFire.Add(a);
                            }
                            a.ChangeHealth(-70, mH.GetNPCManager().GetCommander(NPC.AffliationTypes.red));
                        }
                    }
                }
            }

            //Deal Damage
            for (int i = 0; i < doomedDots.Count; i++)
            {
                NPC a = doomedDots[i];
                if (!mH.GetNPCManager().GetNPCs().Contains(a))
                {
                    doomedDots.Remove(a);
                    i--;
                    continue;
                }

                //Spawn fire
                if (mH.GetRandom().NextDouble() < 0.1f)
                {
                    mH.GetParticleManager()
                      .AddFire(a.GetOriginPosition(),
                               PathHelper.Direction((float) (mH.GetRandom().NextDouble()*Math.PI*2))*30, 1, 0.05f, 1,
                               0.1f);
                }

                if (frameCounter%burnFrames == 0)
                {
                    a.ChangeHealth(-2, mH.GetNPCManager().GetCommander(NPC.AffliationTypes.red));
                }
            }

            frameCounter++;
            this.scale = scale * modifer;

            base.Update(mH);
        }

        public override void Draw(SpriteBatch sB, Vector2 displacement, ManagerHelper mH)
        {
            if (frameCounter <= drawFrames)
            {
                base.Draw(sB, displacement, mH);
            }
        }

        public bool IsDone()
        {
            return frameCounter > existFrames;
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