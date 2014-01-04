using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace DotWars
{
    public class LargeRock : Sprite
    {
        #region Declarations

        private NPC.AffliationTypes affiliation;
        private float animateTime;

        private int health;
        private float timer;
        private const int DAMAGE = 10;

        private NPC lastDamager;

        private ManagerHelper managers;

        #endregion

        public LargeRock(ManagerHelper mH)
            : base("Abilities/ability_green", Vector2.Zero)
        {
            managers = mH;
            lastDamager = null;
        }

        public override void Update(ManagerHelper mH)
        {
            Queue<Projectile> tempProjectiles = mH.GetProjectileManager().GetProjectiles();
            foreach (Projectile p in tempProjectiles)
            {
                if (p.GetDrawTime() > 0 &&
                    CollisionHelper.IntersectPixelsPoint(p.GetOriginPosition(), this) != new Vector2(-1))
                {
                    health -= p.GetDamage();
                    lastDamager = p.GetCreator();

                    p.SetDrawTime(0);
                }
            }

            if (timer < animateTime)
            {
                frameIndex = (int) (timer/animateTime*12);

                if (mH.GetGametype() is Survival)
                {
                    foreach (NPC a in mH.GetNPCManager().GetAllies(NPC.AffliationTypes.black))
                    {
                        var test = PathHelper.Distance(GetOriginPosition(), a.GetOriginPosition());

                        if (test < ((frameIndex + 1) * 6))
                        {
                            a.AddAcceleration(PathHelper.DirectionVector(GetOriginPosition(), a.GetOriginPosition()) * 10);

                            a.ChangeHealth(-1 * DAMAGE, mH.GetNPCManager().GetCommander(NPC.AffliationTypes.green));
                        }
                    }
                }
                else
                {
                    foreach (NPC a in mH.GetNPCManager().GetNPCs())
                    {
                        if (a.GetAffiliation() != affiliation)
                        {
                            var test = PathHelper.Distance(GetOriginPosition(), a.GetOriginPosition());

                            if (test < ((frameIndex + 1)*6))
                            {
                                a.AddAcceleration(PathHelper.DirectionVector(GetOriginPosition(), a.GetOriginPosition()) * 10);

                                a.ChangeHealth(-1 * DAMAGE, mH.GetNPCManager().GetCommander(NPC.AffliationTypes.green));
                            }
                        }
                    }
                }

                timer += (float) mH.GetGameTime().ElapsedGameTime.TotalSeconds;
            }

            base.Update(mH);
        }

        public void Set(Vector2 p, NPC.AffliationTypes aT, ManagerHelper mH)
        {
            position = p;

            animateTime = 0.3f;
            timer = 0;

            health = 150;

            affiliation = aT;

            LoadContent(mH.GetTextureManager());

            mH.GetAudioManager().Play(AudioManager.LARGE_ROCK, (float)mH.GetRandom().NextDouble() / 4 + 0.75f, AudioManager.RandomPitch(mH), 0, false);

            for (int i = 0; i < 10; i++)
            {
                mH.GetParticleManager().AddStandardSmoke(origin + position, 64);
            }
        }

        public int GetHealth()
        {
            return health;
        }

        public void SetHealth(int newHealth)
        {
            health = newHealth;
        }

        public void ChangeHealth(int amount)
        {
            SetHealth(GetHealth() + amount);
        }

        public NPC GetLastDamager()
        {
            return lastDamager;
        }

        public bool IsFullyUp()
        {
            return timer > animateTime;
        }
    }
}