using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DotWars
{
    internal class SwitchFan : Environment
    {
        private readonly float endAnimation;
        private readonly Sprite theFan;

        private float animationTimer;
        private bool on;

        public SwitchFan(bool l, Vector2 p)
            : base((l) ? "Backgrounds/Switch/fanLeft" : "Backgrounds/Switch/fanLeft", p, Vector2.Zero)
        {
            theFan = new Sprite((l) ? "Backgrounds/Switch/gustLeft" : "Backgrounds/Switch/gustLeft",
                                p + new Vector2(36, 24));
            on = false;
            endAnimation = 0.5f;
            animationTimer = 0;
        }

        public override void LoadContent(TextureManager tM)
        {
            base.LoadContent(tM);
            theFan.LoadContent(tM);
        }

        public override void Update(ManagerHelper mH)
        {
            if (on)
            {
                foreach (NPC a in mH.GetNPCManager().GetNPCs())
                {
                    if (CollisionHelper.IntersectPixelsDirectional(a, theFan) != -1)
                    {
                        a.AddAcceleration(new Vector2(1));
                    }
                }
            }

            //Animation
            if (animationTimer > endAnimation)
            {
                frameIndex++;

                if (frameIndex >= totalFrames)
                {
                    frameIndex = 0;
                }

                if (on)
                {
                    theFan.SetFrameIndex(mH.GetRandom().Next(theFan.totalFrames));
                }

                animationTimer = 0;
            }
            else
            {
                animationTimer += ((on) ? 10 : 1)*(float) mH.GetGameTime().ElapsedGameTime.TotalSeconds;
            }

            base.Update(mH);
            theFan.Update(mH);
        }

        public override void Draw(SpriteBatch sB, Vector2 displacement, ManagerHelper mH)
        {
            base.Draw(sB, displacement, mH);

            if (on)
            {
                theFan.Draw(sB, displacement, mH);
            }
        }

        public void SetOn(bool o)
        {
            on = o;
        }
    }
}