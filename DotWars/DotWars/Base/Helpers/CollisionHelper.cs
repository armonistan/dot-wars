﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace DotWars
{
    public static class CollisionHelper
    {
        #region NPC Vectors

        private const int NUM_DIRECTIONS = 64;
        private static readonly Vector2[] DIRECTIONS = new Vector2[NUM_DIRECTIONS];
        private static readonly float[] ROTATIONS = new float[NUM_DIRECTIONS];

        #endregion

        public static void Initialize()
        {
            for (int i = 0; i < NUM_DIRECTIONS; i++)
            {
                ROTATIONS[i] = (float) Math.PI*2*i/NUM_DIRECTIONS;
                DIRECTIONS[i] = new Vector2((float) Math.Cos(ROTATIONS[i]), (float) Math.Sin(ROTATIONS[i]));
            }
        }

        public static Vector2 CollideRandom(Vector2 v1, Vector2 v2)
        {
            var rand = new Random();

            //Skew the angle slightly to nudge the collider out of the way gradulaly
            var angle = (float) (PathHelper.Direction(v1, v2) + (((rand.Next(2) == 0) ? -1 : 1)*Math.PI*4/5));

            //Exctract the x and y values from the skewed angle
            return new Vector2((float) Math.Cos(angle), (float) Math.Sin(angle));
        }

        public static Vector2 CollideDirectional(Vector2 v, int i)
        {
            Vector2 normal = DIRECTIONS[i];

            return v - (2*PathHelper.DotProduct(v, normal)*normal);
        }

        public static Vector2 CollideSimple(int i, Vector2 v)
        {
            return DIRECTIONS[(i + NUM_DIRECTIONS/2)%NUM_DIRECTIONS]*v.Length();
        }

        public static Vector2 IntersectPixelsSimple(Sprite sA, Sprite sB)
        {
            return IntersectPixelsRadius(sA, sB, sA.origin.Y, sB.origin.Y);
        }

        public static Vector2 IntersectPixelsRadius(Sprite sA, Sprite sB, float rA, float rB)
        {
            float dist = PathHelper.Distance(sA.GetOriginPosition(), sB.GetOriginPosition());

            if (dist < (rA + rB))
            {
                return sB.GetOriginPosition();
            }
            else
            {
                return new Vector2(-1);
            }
        }

        public static int IntersectPixelsDirectional(Sprite sA, Sprite sB)
        {
            Vector2 tempVector,
                    tempRadius = new Vector2(sA.origin.X);

            for (int i = 0; i < NUM_DIRECTIONS; i++)
            {
                tempVector = IntersectPixelsPoint(sA.GetOriginPosition() + (DIRECTIONS[i]*tempRadius), sB);

                if (tempVector != -Vector2.One)
                {
                    return 1;
                }
            }

            return -1;
           
        }

        public static int IntersectPixelsDirectionalRaw(Sprite sA, Vector2 fA, Sprite sB)
        {
            Vector2 tempVector = new Vector2(-1),
                    tempRadius = new Vector2(sA.origin.X);
            
            for (int i = 0; i < NUM_DIRECTIONS; i++)
            {
                tempVector = IntersectPixelsPoint(fA + (DIRECTIONS[i]*tempRadius), sB);

                if (tempVector != -Vector2.One)
                {
                    return i;
                }
            }
            return -1;
        }

        public static Vector2 IntersectPixelsPoint(Vector2 p, Sprite sB)
        {
            int frameWidth = sB.GetFrame().Width,
                frameHeight = sB.GetFrame().Height,
                frameStartX = sB.GetFrameIndex() * frameWidth,
                frameStartY = sB.GetModeIndex() * frameHeight,
                frameEndX = frameStartX + frameWidth,
                frameEndY = frameStartY + frameHeight;

            int x = (int) (p.X - sB.position.X) + frameStartX,
                y = (int) (p.Y - sB.position.Y) + frameStartY;

            if (x < frameStartX || x >= frameEndX || y < frameStartY || y >= frameEndY)
            {
                return -Vector2.One;
            }

            if (sB.GetTextureData()[x, y].A != 0)
            {
                return p;
            }
            else
            {
                return -Vector2.One;
            }
        }
    }
}