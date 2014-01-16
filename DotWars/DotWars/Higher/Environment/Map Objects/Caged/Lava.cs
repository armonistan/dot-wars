#region

using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    internal class Lava : Environment
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