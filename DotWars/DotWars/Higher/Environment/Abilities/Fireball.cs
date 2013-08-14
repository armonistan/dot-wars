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
        private readonly float endAnimation;
        private readonly float endLife;
        private NPC.AffliationTypes affiliation;
        private ManagerHelper managers;
        private float timer;

        #endregion

        public Fireball(ManagerHelper mH)
            : base("Abilities/ability_red", Vector2.Zero, Vector2.Zero)
        {
            doomedDots = new List<NPC>();
            dotsSetOnFire = new List<NPC>();
            endLife = 5;
            endAnimation = 0.1f;
            managers = mH;
        }

        public void Set(Vector2 p, NPC.AffliationTypes aT, ManagerHelper mH)
        {
            timer = 0;
            doomedDots.Clear();
            affiliation = aT;
            frameIndex = 0;
            position = p;
            originPosition = position + origin;

            mH.GetAudioManager().Play("fireballSound", (float)mH.GetRandom().NextDouble()/4 + 0.75f, AudioManager.RandomPitch(mH), 0, false);
        }

        public override void Update(ManagerHelper mH)
        {
            if (frameIndex < totalFrames)
            {
                frameIndex = (int) (timer/endAnimation*4);

                //Spawn fire
                if (mH.GetRandom().NextDouble() < 0.7f)
                {
                    mH.GetParticleManager()
                      .AddFire(GetOriginPosition(),
                               PathHelper.Direction((float) (mH.GetRandom().NextDouble()*Math.PI*2))*100, 1, 0.05f, 1,
                               0.1f);
                }

                foreach (NPC a in mH.GetNPCManager().GetAllButAlliesInRadius(affiliation, GetOriginPosition(), 64))
                {
                    if (!doomedDots.Contains(a))
                    {
                        doomedDots.Add(a);

                        if (!a.GetFireStatus())
                        {
                            a.ChangeFireStatus();
                            dotsSetOnFire.Add(a);
                        }
                        a.ChangeHealth(-10, mH.GetNPCManager().GetCommander(NPC.AffliationTypes.red));
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
                    a.ChangeHealth(-4, mH.GetNPCManager().GetCommander(NPC.AffliationTypes.red));
                }
            }

            timer += (float) mH.GetGameTime().ElapsedGameTime.TotalSeconds;

            base.Update(mH);
        }

        public override void Draw(SpriteBatch sB, Vector2 displacement, ManagerHelper mH)
        {
            if (timer <= endAnimation)
            {
                base.Draw(sB, displacement, mH);
            }
        }

        public bool IsDone()
        {
            return timer > endLife;
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