using Microsoft.Xna.Framework;

namespace DotWars
{
    internal class Fountain : InDestructable
    {
        #region Declarations

        private readonly double endTime;
        private double timer;

        #endregion

        public Fountain(Vector2 p) :
            base("Backgrounds/Plaza/192and608_animated", p)
        {
            timer = 0;
            endTime = 0.5;
        }

        public override void Update(ManagerHelper mH)
        {
            //Animate
            if (timer > endTime)
            {
                if (frameIndex == totalFrames - 1)
                {
                    frameIndex = 0;
                }
                else
                {
                    frameIndex++;
                }

                timer = 0;
            }
            else
            {
                timer += mH.GetGameTime().ElapsedGameTime.TotalSeconds;
            }

            base.Update(mH);
        }
    }
}