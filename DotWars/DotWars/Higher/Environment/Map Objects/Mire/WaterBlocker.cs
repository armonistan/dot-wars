using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DotWars
{
    public class WaterBlocker : Impassable
    {
        private float changeEnd;

        private float changeTimer;

        public WaterBlocker(Vector2 p)
            : base("Backgrounds/Mire/waterBlocker", p)
        {
            changeEnd = 3;
            changeTimer = 0;
        }

        public override void Update(ManagerHelper mH)
        {
            //if (changeTimer > changeEnd)
            //{
            //
            //
            //    changeTimer = 0;
            //}
            //else
            //{
            //    changeTimer += (float)mH.GetGameTime().ElapsedGameTime.TotalSeconds;
            //}

            //Spawn random bubbles
            if (mH.GetRandom().Next(5) == 0)
            {
                var tempDir = (float) (Math.PI*2*mH.GetRandom().NextDouble());
                var tempVect = new Vector2((float) Math.Cos(tempDir)*1.45f, (float) Math.Sin(tempDir));
            }

            base.Update(mH);
        }

        public override void Draw(SpriteBatch sB, Vector2 displacement, ManagerHelper mH)
        {
        }

        /*public void SetClosed(bool c, ManagerHelper mH)
        {
            frameIndex = (c) ? 1 : 0;

            foreach (LillyPad l in thePads)
            {
                l.DestroySelf(mH);
            }

            thePads.Clear();

            LillyPad tempPad;

            if (frameIndex == 1)
            {
                //North
                for (int i = 0; i < 3; i++)
                {
                    tempPad = new LillyPad(new Vector2(mH.GetRandom().Next(437, 509), mH.GetRandom().Next(167, 210)), 4);
                    thePads.Add(tempPad);
                    mH.GetEnvironmentManager().AddBotParticle(tempPad);
                }
            }
            else
            {
                //North
                for (int i = 0; i < 3; i++)
                {
                    tempPad = new LillyPad(new Vector2(mH.GetRandom().Next(437, 509), mH.GetRandom().Next(167, 210)), 4);
                    thePads.Add(tempPad);
                    mH.GetEnvironmentManager().AddBotParticle(tempPad);
                }

                //West
                for (int i = 0; i < 3; i++)
                {
                    tempPad = new LillyPad(new Vector2(mH.GetRandom().Next(175, 255), mH.GetRandom().Next(392, 440)), 4);
                    thePads.Add(tempPad);
                    mH.GetEnvironmentManager().AddBotParticle(tempPad);
                }

                tempPad = new LillyPad(new Vector2(736, 364), mH.GetRandom().Next(5));
                thePads.Add(tempPad);
                mH.GetEnvironmentManager().AddBotParticle(tempPad);

                tempPad = new LillyPad(new Vector2(448, 608), mH.GetRandom().Next(5));
                thePads.Add(tempPad);
                mH.GetEnvironmentManager().AddBotParticle(tempPad);
            }
        }*/
    }
}