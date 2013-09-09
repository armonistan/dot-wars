using System;
using Microsoft.Xna.Framework;

namespace DotWars
{
    internal class WhirlPool : Impathable
    {
        private Sprite[] poolSections;
        private const int NUM_SECTIONS = 5;

        public WhirlPool(Vector2 p)
            : base("Backgrounds/Archipelago/whirlpool0", p, Vector2.Zero)
        {
            poolSections = new Sprite[NUM_SECTIONS];
            
            for (int i = 0; i < NUM_SECTIONS; i++)
            {
                poolSections[i] = new Sprite("Backgrounds/Archipelago/whirlpool" + (i + 1), p);
            }
        }

        public override void LoadContent(TextureManager tM)
        {
            foreach (Sprite section in poolSections)
            {
                section.LoadContent(tM);
            }

            base.LoadContent(tM);
        }

        public override void Update(ManagerHelper mH)
        {
            foreach (Sprite section in poolSections)
            {
                section.Turn(10000.0f / (section.GetFrame().Width * section.GetFrame().Width) * (float)mH.GetGameTime().ElapsedGameTime.TotalSeconds);
            }

            foreach (NPC a in mH.GetNPCManager().GetNPCs())
            {
                if (!(a is Bomber))
                {
                    float tempDistance = PathHelper.Distance(GetOriginPosition(), a.GetOriginPosition());
                    if (tempDistance < 15)
                    {
                        //TODO: Modify
                        a.position = mH.GetLevelSize() * new Vector2(mH.GetRandom().Next(2), mH.GetRandom().Next(2));
                    }
                    else if (tempDistance < (frame.Width/2))
                    {
                        float tempRot = PathHelper.Direction(a.GetOriginPosition(), GetOriginPosition()) -
                                        (float) Math.PI/9;
                        a.AddAcceleration(new Vector2((float)Math.Cos(tempRot), (float)Math.Sin(tempRot)) * 4);
                    }
                }
            }

            foreach (Particle e in mH.GetParticleManager().GetParticles())
            {
                float tempDistance = PathHelper.Distance(GetOriginPosition(), e.GetOriginPosition());

                if (tempDistance < 15)
                {
                    e.SetDrawTime(0);
                }
                else if (tempDistance < (frame.Width/2))
                {
                    float tempRot = PathHelper.Direction(e.GetOriginPosition(), GetOriginPosition()) + (float) Math.PI/9;
                    e.AddAcceleration(new Vector2((float)Math.Cos(tempRot), (float)Math.Sin(tempRot)) * 100);
                }
            }

            base.Update(mH);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sB, Vector2 displacement, ManagerHelper mH)
        {
            base.Draw(sB, displacement, mH);
            foreach (Sprite section in poolSections)
            {
                section.Draw(sB, displacement, mH);
            }
        }
    }
}