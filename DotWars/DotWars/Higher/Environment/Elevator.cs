using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DotWars
{
    public class Elevator : Environment
    {
        private int elevationChange;

        public Elevator(string a, Vector2 p, int l)
            : base(a, p, Vector2.Zero)
        {
            elevationChange = l;
        }

        public int GetLevel()
        {
            return elevationChange;
        }
    }
}
