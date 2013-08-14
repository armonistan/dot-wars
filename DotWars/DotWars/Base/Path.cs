using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace DotWars
{
    public class Path : LinkedList<Vector2>
    {
        #region Declarations

        private float distance; //Holds total distance of path

        #endregion

        public Path()
        {
            distance = 0;
        }

        public void Add(Vector2 p, ManagerHelper mH)
        {
            //base.Add(p);
            AddFirst(p);

            if (Count > 1)
            {
                //distance += PathHelper.Distance(base[Count - 1], base[Count - 2]);
                distance += PathHelper.Distance(base.First.Value, base.First.Next.Value);
            }
        }

        public float GetDistance()
        {
            return distance;
        }
    }
}