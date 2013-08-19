using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DotWars
{
    public class Projectile : Sprite
    {
        #region Declarations

        protected NPC.AffliationTypes affiliation;
        protected int damage;
        protected float drawTime;
        protected float existenceTime;
        protected bool isExplosive;
        protected NPC creator;
        #endregion

        public Projectile(NPC c) :
            base("", Vector2.Zero, Vector2.Zero)
        {
            creator = c;
        }

        public override void Update(ManagerHelper mH)
        {
            if (isExplosive && drawTime <= 0)
            {
                mH.GetParticleManager().AddExplosion(GetOriginPosition(), this.creator, damage);
                isExplosive = false;
            }

            if (position.X < 0 || position.X > mH.GetLevelSize().X ||
                position.Y < 0 || position.Y > mH.GetLevelSize().Y)
            {
                SetDrawTime(0);
            }

            if (drawTime > 0)
            {
                drawTime -= (float) mH.GetGameTime().ElapsedGameTime.TotalSeconds;

                SpriteUpdate(mH);

                //Spawn cool things to make it look better
                if (mH.GetRandom().NextDouble() < 0.5f)
                    EffectSpawnCode(mH);
            }

            existenceTime -= (float) mH.GetGameTime().ElapsedGameTime.TotalSeconds;
        }

        public override void Draw(SpriteBatch sB, Vector2 displacement, ManagerHelper mH)
        {
            if (drawTime > 0)
            {
                base.Draw(sB, displacement, mH);
            }
        }

        protected virtual void EffectSpawnCode(ManagerHelper mH)
        {
            //Spawn bullet particles 15% of the time
            if (mH.GetRandom().Next(100) < 15)
            {
                mH.GetParticleManager()
                  .AddParticle("Effects/particle_smoke", GetOriginPosition(),
                               PathHelper.Direction((float) (Math.PI*mH.GetRandom().NextDouble())*2)*20, 4, 0.005f, 1,
                               0.1f);
            }
        }

        private void SpriteUpdate(ManagerHelper mH)
        {
            base.Update(mH);
        }

        #region Sets and Gets

        public virtual void Set(String a, Vector2 p, NPC n, Vector2 v, int d, bool iE, float dT, ManagerHelper mH)
        {
            asset = a;
            position = p;
            velocity = v;
            damage = d; // set damage
            existenceTime = 4; // Default 10 seconds
            drawTime = dT; //set up draw time
            affiliation = n.GetAffiliation(); //Sets up hurting
            creator = n;

            isExplosive = iE;

            //Get x and y values from angle and set up direction
            rotation = (float) Math.Atan2(velocity.Y, velocity.X);

            //No friction
            drag = 0;

            LoadContent(mH.GetTextureManager());
        }

        public int GetDamage()
        {
            return damage;
        }

        public void SetDrawTime(float dT)
        {
            drawTime = dT;
        }

        public float GetDrawTime()
        {
            return drawTime;
        }

        public NPC.AffliationTypes GetAffiliation()
        {
            return affiliation;
        }

        public float GetExistenceTime()
        {
            return existenceTime;
        }

        public NPC GetCreator() 
        {
            return creator;
        }

        #endregion
    }
}