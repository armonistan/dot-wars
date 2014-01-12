using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace DotWars
{
    public class Path : List<Vector2>
    {
        #region Declarations

        private float distance; //Holds total distance of path
        private bool moving;

        #endregion

        public Path()
        {
            distance = 0;
            moving = true;
        }

        public void AddPoint(Vector2 p)
        {
            Add(p);

            if (Count > 1)
            {
                distance += PathHelper.DistanceSquared(base[Count - 1], base[Count - 2]);
            }

            SetMoving(true);
        }

        public double GetDistance()
        {
            return distance;
        }

        public bool GetMoving()
        {
            return moving;
        }

        public void SetMoving(bool moving)
        {
            this.moving = moving;
        }
    }
}