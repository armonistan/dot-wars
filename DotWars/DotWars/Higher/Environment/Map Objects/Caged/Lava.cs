#region

using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    internal class Lava : Environment
    {
        private float startX;
        private float endx;

        public Lava(string asset,Vector2 p, Vector2 v, float sX, float eX) :
            base(asset, p, v)
        {
            startX = sX;
            endx = eX;
        }

        public override void Update(ManagerHelper mH)
        {
            if (position.X > endx)
                position.X = startX;

            position += velocity;
        }
    }
}