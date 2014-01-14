using System;
using Microsoft.Xna.Framework;

namespace DotWars
{
    public class Explosion : Particle
    {
        #region Declarations

        private readonly float radius;
        private NPC.AffliationTypes affiliation;
        private int damage;
        private bool exploaded;

        #endregion

        public Explosion()
        {
            asset = "Effects/explodeTop";
            exploaded = false;

            radius = 64;
        }

        public override void Update(ManagerHelper mH)
        {
            foreach (NPC a in mH.GetNPCManager().GetNPCs())
            {
                if (NPCManager.IsNPCInRadius(a, GetOriginPosition(), radius))
                {
                    if (!exploaded && a.GetAffiliation() != affiliation)
                    {
                        a.ChangeHealth(-1*damage, creator);
                    }

                    //Make screen rumble
                    if (a is Commander)
                    {
                        var tempCom = (Commander) a;
                        mH.GetCameraManager().SetRumble(mH.GetCameraManager().GetPlayerIndex(tempCom), 1000);
                    }
                }
            }

            foreach (LargeRock largeRock in mH.GetAbilityManager().GetLargeRocks())
            {
                var test = PathHelper.DistanceSquared(GetOriginPosition(), largeRock.GetOriginPosition());

                if (!exploaded && test < radius * radius)
                {
                    largeRock.ChangeHealth(-1 * damage);
                }
            }

            frameIndex = (int) (existanceTime*10);

            //Move particles from explosions
            foreach (Particle p in mH.GetParticleManager().GetParticles())
            {
                float dir = PathHelper.Direction(GetOriginPosition(), p.GetOriginPosition());
                p.AddAcceleration(PathHelper.Direction(dir) * 10000.0f / 
                                    (PathHelper.DistanceSquared(GetOriginPosition(), p.GetOriginPosition()) + 1));
            }
            foreach (Gut g in mH.GetParticleManager().GetGuts())
            {
                float dir = PathHelper.Direction(GetOriginPosition(), g.GetOriginPosition());
                g.AddAcceleration(PathHelper.Direction(dir) * 10000.0f /
                                    (PathHelper.DistanceSquared(GetOriginPosition(), g.GetOriginPosition()) + 1));
            }

            Turn(MathHelper.Pi/21);

            //Spawn Fires to make effect
            if (mH.GetRandom().NextDouble() < 0.25f)
            {
                mH.GetParticleManager()
                  .AddFire(GetOriginPosition(),
                           PathHelper.Direction((float) (mH.GetRandom().NextDouble()*Math.PI*2))*500*
                           (float) mH.GetRandom().NextDouble(), 1, 0.01f, 1, 0.1f);
                for (int d = damage - 75; d >= 0; d -= 20)
                {
                    mH.GetParticleManager()
                      .AddFire(GetOriginPosition(),
                               PathHelper.Direction((float)(mH.GetRandom().NextDouble()*MathHelper.TwoPi))*500*
                               (float) mH.GetRandom().NextDouble(), 1, 0.01f, 1, 0.1f);
                }
            }

            base.Update(mH);

            exploaded = true;
        }

        public void Set(Vector2 p, int d, NPC n, ManagerHelper mH)
        {
            base.Set(asset, p, Vector2.Zero, 0.1f, 0, 0, 0.5f, mH);

            existanceTime = 0.1f;
            affiliation = n.GetAffiliation();
            creator = n;
            damage = d;
            exploaded = false;

            originPosition = position + origin;

            mH.GetAudioManager().Play(AudioManager.EXPLOSION, AudioManager.RandomVolume(mH), AudioManager.RandomPitch(mH), 0, false);
        }

        public void Set(Vector2 p, int d, NPC.AffliationTypes aT, ManagerHelper mH)
        {
            base.Set(asset, p, Vector2.Zero, 0.1f, 0, 0, 0.5f, mH);

            existanceTime = 0.1f;
            affiliation = aT;
            creator = null;
            damage = d;
            exploaded = false;

            mH.GetAudioManager().Play(AudioManager.EXPLOSION, AudioManager.RandomVolume(mH), AudioManager.RandomPitch(mH), 0, false);
        }

        public float GetRadius()
        {
            return radius;
        }

        public NPC.AffliationTypes GetAffiliation()
        {
            return affiliation;
        }
    }
}