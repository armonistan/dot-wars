#region

using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    internal class AnimatedLight : Environment
    {
        private readonly double animationEnd;
        private bool animate;
        private double animationTimer;

        public AnimatedLight(Vector2 p, float r)
            : base("Backgrounds/Plaza/light", p, Vector2.Zero)
        {
            rotation = r;
            animationTimer = 0;
            animationEnd = 0.1f;
            animate = false;
        }

        public override void Update(ManagerHelper mH)
        {
            if (animate)
            {
                if (animationTimer > animationEnd)
                {
                    frameIndex++;
                    animationTimer = 0;

                    if (frameIndex >= totalFrames)
                    {
                        frameIndex = 0;
                        animate = false;
                    }
                }
                else
                {
                    animationTimer += mH.GetGameTime().ElapsedGameTime.TotalSeconds;
                }
            }
            else
            {
                animate = mH.GetRandom().Next(300) == 0;
            }

            base.Update(mH);
        }
    }
}