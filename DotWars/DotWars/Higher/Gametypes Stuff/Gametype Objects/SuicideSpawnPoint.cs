#region

using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    public class SuicideSpawnPoint : SpawnPoint
    {
        private readonly Sprite suicideSpawnSprite;
        private double spriteTimer;
        private readonly double spriteEndtime;

        public SuicideSpawnPoint(Vector2 sP, ManagerHelper mH)
            : base(sP, NPC.AffliationTypes.black, mH)
        {
            pathTimer = 11;
            pathTimerEnd = 5; //TODO: Find out if this is reasonable
            path = new Path();
            asset = "Dots/Grey/grey_claimable";
            movementSpeed = 50;

            spriteEndtime = .05f;
            spriteTimer = 0;
            suicideSpawnSprite = new Sprite("Effects/suicide_spawn_sprite", sP, Vector2.Zero);
        }

        public override void LoadContent(TextureManager tM)
        {
            suicideSpawnSprite.LoadContent(tM);
            base.LoadContent(tM);
        }

        private void animateEffects(ManagerHelper mH)
        {
            //animation of spawn sprite
            if (spriteTimer > spriteEndtime)
            {
                suicideSpawnSprite.SetFrameIndex(suicideSpawnSprite.GetFrameIndex() + 1);

                if (suicideSpawnSprite.GetFrameIndex() > 10)
                {
                    suicideSpawnSprite.SetFrameIndex(0);
                }
                spriteTimer = 0;
            }
            else
            {
                spriteTimer += mH.GetGameTime().ElapsedGameTime.TotalSeconds;
            }

            suicideSpawnSprite.position = new Vector2(position.X - origin.X, position.Y - origin.Y);
            suicideSpawnSprite.Update(mH);
        }

        public override void Update(ManagerHelper mH)
        {
            NPCUpdate(mH);
            spawnPoint = GetOriginPosition();

            animateEffects(mH);
            base.Update(mH);
        }

        protected override void Behavior(ManagerHelper mH)
        {
            if (mH.GetRandom().Next(100) == 0)
            {
                //Spawn lightning thing
                mH.GetParticleManager().AddParticle("Effects/spr_bolt_strip3", GetOriginPosition() +
                                                                               new Vector2(
                                                                                   mH.GetRandom().Next(-16, 16),
                                                                                   mH.GetRandom().Next(-16, 16)),
                                                    Vector2.Zero, 0.05f, 0, 0, MathHelper.Pi/10);
            }
        }

        protected override void PosUpdate(ManagerHelper mH)
        {
            //Update position
            Vector2 tempPos = position + velocity*mH.GetDeltaSeconds();
            Vector2 tempOPos = tempPos + origin;
            float distSquared = 1000000;

            //Collisions
            foreach (Impassable e in mH.GetEnvironmentManager().GetImpassables())
            {
                if (distSquared > PathHelper.DistanceSquared(e.GetOriginPosition(), tempOPos))
                {
                    int tempVect = CollisionHelper.IntersectPixelsDirectionalRaw(this, tempOPos, e);
                    if (tempVect != -1)
                    {
                        velocity = CollisionHelper.CollideDirectional(velocity, tempVect);
                    }
                }
            }

            foreach (Environment e in mH.GetEnvironmentManager().GetStaticBlockers())
            {
                if (distSquared > PathHelper.DistanceSquared(e.GetOriginPosition(), tempOPos))
                {
                    int tempVect = CollisionHelper.IntersectPixelsDirectionalRaw(this, tempOPos, e);
                    if (tempVect != -1)
                    {
                        velocity = CollisionHelper.CollideSimple(tempVect, velocity);
                    }
                }
            }
            tempPos = position + velocity*mH.GetDeltaSeconds();

            if (
                !(tempPos.X < buffer || tempPos.X > mH.GetLevelSize().X - frame.Width - buffer || tempPos.Y < buffer ||
                  tempPos.Y > mH.GetLevelSize().Y - frame.Height - buffer))
            {
            }
            else
            {
                if (tempPos.X <= buffer)
                {
                    tempPos.X = buffer;
                }
                else if (tempPos.X > mH.GetLevelSize().X - frame.Width - buffer)
                {
                    tempPos.X = mH.GetLevelSize().X - frame.Width - buffer;
                }

                if (tempPos.Y <= buffer)
                {
                    tempPos.Y = buffer;
                }
                else if (tempPos.Y > mH.GetLevelSize().Y - frame.Height - buffer)
                {
                    tempPos.Y = mH.GetLevelSize().Y - frame.Height - buffer;
                }
            }

            position = tempPos;

            //Update frames
            frame.X = frameIndex*frame.Width;
            frame.Y = modeIndex*frame.Height;
        }

        protected override bool ProjectileCheck(ManagerHelper mH)
        {
            return false;
            //return base.ProjectileCheck(mH);
        }

        protected override void SpecialPath(ManagerHelper mH)
        {
            RandomPath(mH);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sB, Vector2 displacement,
                                  ManagerHelper mH)
        {
            suicideSpawnSprite.Draw(sB, displacement, mH);
        }
    }
}