#region

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

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
    }
}