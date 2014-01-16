#region

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace DotWars
{
    public class LargeRock : Sprite
    {
        #region Declarations

        private NPC.AffliationTypes affiliation;
        private double animateTime;
        private double fadeTime;

        private int health;
        private int maxHealth;
        private double timer;
        private const int DAMAGE = 10;

        private NPC lastDamager;

        private ManagerHelper managers;

        private readonly Sprite pulse;
        private float pulseDistance;

        #endregion

        public LargeRock(ManagerHelper mH)
            : base("Abilities/ability_green", Vector2.Zero)
        {
            managers = mH;
            lastDamager = null;

            pulse = new Sprite("Abilities/ability_green2", Vector2.Zero);
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
                frameIndex = (int) (timer/animateTime*totalFrames);
                float frameModifier = ((frameIndex + 1)*6);

                foreach (NPC agent in mH.GetNPCManager().GetNPCs())
                {
                    if (agent.GetAffiliation() == NPC.AffliationTypes.black && mH.GetGametype() is Survival ||
                        agent.GetAffiliation() != affiliation)
                    {
                        float distanceToAgent = PathHelper.DistanceSquared(GetOriginPosition(),
                                                                           agent.GetOriginPosition());

                        if (distanceToAgent < frameModifier*frameModifier)
                        {
                            agent.ChangeHealth(-1*DAMAGE, mH.GetNPCManager().GetCommander(NPC.AffliationTypes.green));
                        }

                        if (distanceToAgent < pulseDistance*pulseDistance)
                        {
                            Vector2 direction = PathHelper.DirectionVector(GetOriginPosition(),
                                                                           agent.GetOriginPosition());

                            float modifier = 1f/(distanceToAgent)*(pulseDistance*pulseDistance)*2f;

                            agent.AddAcceleration(direction*modifier);
                        }
                    }
                }
            }
            else
            {
                modeIndex = 1;

                frameIndex = 4 - 4*health/maxHealth;
            }

            if (timer < fadeTime)
            {
                pulse.SetFrameIndex((int) (timer/fadeTime*pulse.totalFrames));
                pulse.Update(mH);
            }

            timer += mH.GetGameTime().ElapsedGameTime.TotalSeconds;

            base.Update(mH);
        }

        public void Set(Vector2 p, NPC.AffliationTypes aT, ManagerHelper mH)
        {
            position = p;

            animateTime = 0.3;
            fadeTime = 1.0;
            timer = 0;

            health = 150;
            maxHealth = health;

            affiliation = aT;

            LoadContent(mH.GetTextureManager());

            mH.GetAudioManager()
              .Play(AudioManager.LARGE_ROCK, (float) mH.GetRandom().NextDouble()/4 + 0.75f, AudioManager.RandomPitch(mH),
                    0, false);

            for (int i = 0; i < 10; i++)
            {
                mH.GetParticleManager().AddStandardSmoke(origin + position, 64);
            }

            pulse.LoadContent(mH.GetTextureManager());
            pulseDistance = pulse.GetFrame().Width;
            pulse.position = p - pulse.origin;
            pulse.SetRotation((float) mH.GetRandom().NextDouble()*MathHelper.TwoPi);
        }

        public void DrawPulse(SpriteBatch sB, Vector2 d, ManagerHelper mH)
        {
            if (timer < fadeTime)
            {
                pulse.Draw(sB, d, mH);
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
            return modeIndex == 1;
        }
    }
}