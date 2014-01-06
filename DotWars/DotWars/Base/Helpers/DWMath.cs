using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DotWars
{
    public class DWMath
    {
        static float B = 4 / MathHelper.Pi;
        static float C = -4 / (MathHelper.Pi * MathHelper.Pi);

        public static float Sin(float x)
        {
            float angle = MathHelper.WrapAngle(x);
            float val = (B * angle + C * angle * ((angle < 0) ? -angle : angle));

            return val;
        }

        public static float Cos(float x)
        {
            return Sin(x + MathHelper.PiOver2);
        }

        public static float Atan(float x)
        {
            return (float)Math.Atan(x);
        }

        public static float Atan2(float y, float x)
        {
            if (x > 0f)
            {
                return Atan(y / x);
            }
            else if (y >= 0f && x < 0f)
            {
                return Atan(y / x) - MathHelper.Pi;
            }
            else if (y < 0f && x < 0f)
            {
                return Atan(y / x) + MathHelper.Pi;
            }
            else if (y > 0f && x == 0f)
            {
                return MathHelper.PiOver2 * -1f;
            }
            else if (y < 0f && x == 0f)
            {
                return MathHelper.PiOver2;
            }
            else if (y == 0f && x == 0f)
            {
                return float.NaN;
            }

            return float.NaN;
        }

        public static float abs(float x)
        {
            if (x >= 0.0f)
            {
                return x;
            }
            else
            {
                return x*-1.0f;
            }
        }
    }
}
