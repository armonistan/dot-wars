using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DotWars
{
    internal class Meteor : InDestructable
    {
        //declarations
        private readonly double nextPulseSpeed;
        private readonly double pulseLength;

        private double animateCounter,
                       animateTime;

        private double nextPulseCounter;
        protected Sprite pulse;
        private double pulseCounter;

        private bool upDown; //true means up, false means down
        private bool warningStarted; //Tells if the warning has occured or not

        public Meteor(Vector2 p)
            : base("Backgrounds/Relic/meteorAnimated2", p)
        {
            nextPulseCounter = 0; //counter for next pulse
            nextPulseSpeed = 8; //how long till next pulse
            pulseLength = 0.3f; //how long pulse lasts
            pulseCounter = 0; //counter for pulse length
            animateCounter = 0; //counter for next animatation frame
            animateTime = 0.25f;
            upDown = true;
            warningStarted = false;
            //eight seconds for full animation, animation has 16 frames

            pulse = new Sprite("Backgrounds/Relic/pulse", p);
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
                }

                else
                {
                    pulseCounter = 0;
                    nextPulseCounter = 0;

                    //Reset pulse
                    pulse.SetFrameIndex(0);
                    pulse.SetModeIndex(0);
                }
            }
            else
            {
                nextPulseCounter += mH.GetGameTime().ElapsedGameTime.TotalSeconds;
            }

            Animate(mH);

            base.Update(mH);
            pulse.Update(mH);
        }

        public override void Draw(SpriteBatch sB, Vector2 displacement, ManagerHelper mH)
        {
            if (nextPulseCounter >= nextPulseSpeed && pulseCounter < pulseLength)
            {
                pulse.Draw(sB, displacement, mH);
            }
            base.Draw(sB, displacement, mH);
        }

        private void Animate(ManagerHelper mH)
        {
            if (animateCounter < animateTime)
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

            //Change mode
            if (!warningStarted && nextPulseCounter >= nextPulseSpeed*0.95f)
            {
                modeIndex = 1;
                warningStarted = true;
                frameIndex = 0;
                animateTime = 0.1f;
            }
            else if (frameIndex == 7)
            {
                modeIndex = 0;
                warningStarted = false;
                animateTime = 0.25f;
            }
        }

        private void AnimatePulse(ManagerHelper mH)
        {
            var index = (int) (12*pulseCounter/pulseLength);

            pulse.SetFrameIndex(index%4);
            pulse.SetModeIndex(index/4);

            pulse.Turn((float) Math.PI/30);
        }

        private void Pulse(ManagerHelper mH)
        {
            List<NPC> sillyDots = mH.GetNPCManager()
                                    .GetAllButAlliesInRadius(NPC.AffliationTypes.grey, GetOriginPosition(), 200);

            foreach (NPC a in sillyDots)
            {
                float dir = PathHelper.Direction(a.GetOriginPosition(), GetOriginPosition()) + (float) Math.PI;

                a.AddAcceleration(PathHelper.Direction(dir) * 15);

                if (a is Commander)
                {
                    var tempCom = (Commander) a;
                    mH.GetCameraManager().SetRumble(mH.GetCameraManager().GetPlayerIndex(tempCom), 512);
                }
            }

            //foreach (Particle p in mH.GetEnvironmentManager().GetTopParticles())
            //{
            //    if (PathHelper.Distance(GetOriginPosition(), p.GetOriginPosition()) < 200)
            //    {
            //        p.accelerations.Add(PathHelper.Direction(PathHelper.Direction(GetOriginPosition(), p.GetOriginPosition())) * 1000);
            //    }
            //}
        }
    }
}