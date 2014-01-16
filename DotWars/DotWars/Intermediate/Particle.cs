#region

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace DotWars
{
    public class Particle : Sprite
    {
        public static float MAX_EXIST_TIME = 1;

        #region Declarations

        protected double drawTime;

        protected double existanceTime;

        private float rotationAmount;

        protected NPC creator;

        #endregion

        public Particle() : base("", Vector2.Zero)
        {
            creator = null;
        }

        protected void Set(string a, Vector2 p, Vector2 v, float dT, float d, float t, float rA, ManagerHelper mH)
        {
            Set(a, 0, 0, p, v, dT, d, t, rA, mH);
        }

        public void Set(string a, int fI, int mI, Vector2 p, Vector2 v, float dT, float d, float t, float rA,
                        ManagerHelper mH)
        {
            asset = a;
            position = p;
            velocity = v;

            existanceTime = MAX_EXIST_TIME;
            drawTime = dT;

            drag = d;
            thrust = t;
            rotationAmount = rA;

            LoadContent(mH.GetTextureManager());
            frameIndex = fI;
            modeIndex = mI;
        }

        public override void Update(ManagerHelper mH)
        {
            if (drawTime > 0)
            {
                Turn(rotationAmount);
                drawTime -= mH.GetGameTime().ElapsedGameTime.TotalSeconds;
                base.Update(mH);
            }

            existanceTime -= mH.GetGameTime().ElapsedGameTime.TotalSeconds;
        }

        public override void Draw(SpriteBatch sB, Vector2 displacement, ManagerHelper mH)
        {
            if (drawTime > 0)
            {
                base.Draw(sB, displacement, mH);
            }
        }

        public double GetExistanceTime()
        {
            return existanceTime;
        }

        public void SetDrawTime(float dT)
        {
            drawTime = dT;
        }
    }
}