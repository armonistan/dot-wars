using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DotWars
{
    class Meteor : InDestructable
    {
        //declarations
        double nextPulseCounter;
        double nextPulseSpeed;
        double pulseLength;
        double pulseCounter;
        double animateCounter;
        bool upDown; //true means up, false means down
        List<NPC> affectedNPCList;

        protected Sprite pulse;

        public Meteor(Vector2 p)
            : base("MapObjects/meteorAnimated", p, new Rectangle(0, 0, 136, 129))
        {
            nextPulseCounter = 0; //counter for next pulse
            nextPulseSpeed = 8; //how long till next pulse
            pulseLength = 1; //how long pulse lasts
            pulseCounter = 0; //counter for pulse length
            animateCounter = 0; //counter for next animatation frame
            affectedNPCList = new List<NPC>();
            upDown = true;
            //eight seconds for full animation, animation has 16 frames

            pulse = new Sprite("MapObjects/pulse", GetOriginPosition(), new Rectangle(0, 0, 470, 441));
        }

        public override void LoadContent(TextureManager tM)
        {
            base.LoadContent(tM);
            pulse.LoadContent(tM);
        }

        public override void Update(ManagerHelper mH)
        {
            if (nextPulseCounter >= nextPulseSpeed)
            {
                if (pulseCounter < pulseLength)
                {
                    Pulse(mH);
                    AnimatePulse(mH);
                    pulseCounter += mH.GetGameTime().ElapsedGameTime.TotalSeconds;
            
                    for (int i = 0; i < affectedNPCList.Count; i++)
                    {
                        if (PathHelper.Distance(this.GetOriginPosition(), affectedNPCList[i].GetOriginPosition()) > 200)
                        {
                            affectedNPCList[i].speed = new Vector2(affectedNPCList[i].movementSpeed);
                            affectedNPCList[i].path = affectedNPCList[i].NewPath(mH);
            
                            affectedNPCList.Remove(affectedNPCList[i]);
                            i--;
                        }
                    }
                }
            
                else
                {
                    foreach(NPC a in affectedNPCList)
                    {
                        a.speed = new Vector2(a.movementSpeed);
                        a.path = a.NewPath(mH);
                    }
            
                    affectedNPCList.Clear();
            
                    pulseCounter = 0;
                    nextPulseCounter = 0;
                }
            }
            
            else
                nextPulseCounter += mH.GetGameTime().ElapsedGameTime.TotalSeconds;

            Animate(mH);

            base.Update(mH);
            pulse.Update(mH);
        }

        public override void Draw(SpriteBatch sB, Vector2 displacement, ManagerHelper mH)
        {
            base.Draw(sB, displacement, mH);
            if (nextPulseCounter >= nextPulseSpeed && pulseCounter < pulseLength)
            {
                pulse.Draw(sB, displacement, mH);
            }
        }

        private void Animate(ManagerHelper mH)
        {
            if (animateCounter < .25)
            {
                animateCounter += mH.GetGameTime().ElapsedGameTime.TotalSeconds;
            }

            else if (upDown && frameIndex < 7)
            {
                animateCounter = 0;
                frameIndex++;
            }
            else if (!upDown && frameIndex > 0)
            {
                animateCounter = 0;
                frameIndex--;
            }

            else if (frameIndex == 7)
                upDown = false;
            else if (frameIndex == 0)
                upDown = true;
        }

        private void AnimatePulse(ManagerHelper mH)
        {
            int index = (int)(9 * pulseCounter / pulseLength);

            pulse.frameIndex = index % 3;
            pulse.modeIndex = index / 3;
        }

        private void Pulse(ManagerHelper mH)
        {
            List<NPC> sillyDots = mH.GetNPCManager().GetAllButAlliesInRadius(NPC.AffliationTypes.grey, GetOriginPosition(), 200);

            foreach(NPC a in sillyDots)
            {
                a.path = null;
                float dir = PathHelper.Direction(a.GetOriginPosition(), GetOriginPosition()) + (float)Math.PI;

                a.direction = new Vector2((float)Math.Cos(dir), (float)Math.Sin(dir));
                a.speed *= 1.001f;

                if (!affectedNPCList.Contains(a))
                {
                    affectedNPCList.Add(a);
                }
            }
        }
    }
}
