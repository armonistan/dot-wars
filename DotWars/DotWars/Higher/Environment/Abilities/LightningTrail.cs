using Microsoft.Xna.Framework;

namespace DotWars
{
    public class LightningTrail : Sprite
    {
        private NPC.AffliationTypes affiliation;
        private double lifeTimer;
        private ManagerHelper managers;

        public LightningTrail(ManagerHelper mH)
            : base("Abilities/yellow_test", Vector2.Zero, Vector2.Zero)
        {
            managers = mH;
        }

        public void Set(Vector2 p, float r, NPC.AffliationTypes aT, ManagerHelper mH)
        {
            affiliation = aT;
            rotation = r;
            position = p;

            lifeTimer = 2;

            mH.GetAudioManager().Play(AudioManager.SPARK, (float)mH.GetRandom().NextDouble() / 4 + 0.5f, AudioManager.RandomPitch(mH), 0, false);
        }

        public override void Update(ManagerHelper mH)
        {
            frameIndex = mH.GetRandom().Next(totalFrames);

            if (mH.GetGametype() is Survival)
            {
                foreach (NPC a in mH.GetNPCManager().GetAllies(NPC.AffliationTypes.black))
                {
                    if (CollisionHelper.IntersectPixelsDirectional(a, this) != -1)
                    {
                        a.ChangeHealth(-1, mH.GetNPCManager().GetCommander(NPC.AffliationTypes.yellow));
                    }
                }
            }

            else
            {
                foreach (NPC a in mH.GetNPCManager().GetNPCs())
                {
                    if (a.GetAffiliation() != affiliation && CollisionHelper.IntersectPixelsPoint(GetOriginPosition(), a) != new Vector2(-1))
                    {
                        a.ChangeHealth(-1, mH.GetNPCManager().GetCommander(NPC.AffliationTypes.yellow));
                    }
                }
            }

            lifeTimer -= mH.GetGameTime().ElapsedGameTime.TotalSeconds;

            base.Update(mH);
        }

        public double GetLifeTime()
        {
            return lifeTimer;
        }
    }
}