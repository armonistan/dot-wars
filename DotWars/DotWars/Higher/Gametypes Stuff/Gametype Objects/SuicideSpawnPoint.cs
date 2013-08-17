﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DotWars
{
    public class SuicideSpawnPoint : SpawnPoint
    {
        private Sprite suicideSpawnSprite;
        private float particleTimer;
        private float particleEndtime;
        private float spriteTimer;
        private float spriteEndtime;

        public SuicideSpawnPoint(Vector2 sP, ManagerHelper mH)
            :base(sP, NPC.AffliationTypes.black, mH)
        {
            pathTimer = 11;
            pathTimerEnd = 5; //TODO: Find out if this is reasonable
            path = new Path();
            asset = "Dots/Grey/grey_claimable";
            movementSpeed = 50;

            spriteEndtime = .05f;
            spriteTimer = 0;
            suicideSpawnSprite = new Sprite("Effects/suicide_spawn_sprite", sP, Vector2.Zero);

            particleTimer = 0;
            particleEndtime = .07f;
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
                spriteTimer += (float)mH.GetGameTime().ElapsedGameTime.TotalSeconds;
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
            ;    
        }

        protected override void PosUpdate(ManagerHelper mH)
        {
            //Update position
            Vector2 tempPos = position + velocity * (float)mH.GetGameTime().ElapsedGameTime.TotalSeconds;
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
            tempPos = position + velocity * (float)mH.GetGameTime().ElapsedGameTime.TotalSeconds;

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
            frame.X = frameIndex * frame.Width;
            frame.Y = modeIndex * frame.Height;
        }

        protected override bool ProjectileCheck(ManagerHelper mH)
        {
            return false;
            //return base.ProjectileCheck(mH);
        }

        protected override Path SpecialPath(ManagerHelper mH)
        {
            return RandomPath(mH);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sB, Vector2 displacement, ManagerHelper mH)
        {
            //suicideSpawnParticle.Draw(sB, displacement, mH);
            suicideSpawnSprite.Draw(sB, displacement, mH);
            //base.Draw(sB, displacement, mH);
        }
    }
}
