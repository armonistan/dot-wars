using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DotWars
{
    public class Environment : Sprite
    {
        private readonly Sprite square;

        protected Environment(String a, Vector2 p, Vector2 v)
            : base(a, p, v)
        {
            square = new Sprite("square", Vector2.Zero);
            drag = 2;
        }

        public override void LoadContent(TextureManager tM)
        {
            base.LoadContent(tM);
            square.LoadContent(tM);
        }

        public override void Draw(SpriteBatch sB, Vector2 displacement, ManagerHelper mH)
        {
            //foreach (Vector2 b in GetFrameBlockers())
            //{
            //    square.position = b + position;
            //    square.Draw(sB, displacement, mH);
            //}

            base.Draw(sB, displacement, mH);
        }

        public List<Vector2> GetFrameBlockersSpecial(ManagerHelper mH)
        {
            var tempList = new List<Vector2>();

            foreach (Vector2 n in GetFrameBlockers())
            {
                int x = (int) ((position.X/32) + n.X);
                int y = (int) ((position.Y/32) + n.Y);

                if (x >= 0 && x < mH.GetPathHelper().GetLength() && y >= 0 && y < mH.GetPathHelper().GetLength())
                {
                    tempList.Add(new Vector2(x, y));
                }
            }

            return tempList;
        }
    }
}