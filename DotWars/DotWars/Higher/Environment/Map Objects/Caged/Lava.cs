using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DotWars
{
    class Lava : Environment
    {
        public Lava(Vector2 p, Vector2 v) :
            base("Backgrounds/Caged/lava", p, v)
        {
        }

        public override void Update(ManagerHelper mH)
        {
            if (position.X > 500)
                position.X = 101;

            position += velocity;
        }

    }
}
