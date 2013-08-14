using System;
using Microsoft.Xna.Framework;

namespace DotWars
{
    public class Impassable : Environment
    {
        public Impassable(String a, Vector2 p)
            : base(a, p, Vector2.Zero)
        {
            drag = 0;
        }
    }
}