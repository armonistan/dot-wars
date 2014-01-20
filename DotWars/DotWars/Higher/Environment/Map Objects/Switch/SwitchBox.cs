#region

using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    public class SwitchBox : Destructable
    {
        private readonly Crane theCrane;
        private readonly SwitchBeltNetwork theNet;
        private SwitchBeltNetwork.Node lastNode;
        private double lifeCounter;

        public SwitchBox(Vector2 p, SwitchBeltNetwork n, Crane c)
            : base("Backgrounds/Switch/switchBox", p, 50)
        {
            theNet = n;
            thrust = 500f;
            drag = 0f;
            lifeCounter = 30.0;
            theCrane = c;
            position -= origin;
            setHealth(75);
        }

        public override void Update(ManagerHelper mH)
        {
            if (lifeCounter < 0)
            {
                SetShouldRemove(true);
            }

            lifeCounter -= mH.GetGameTime().ElapsedGameTime.TotalSeconds;

            //Code for getting picked up
            if (theCrane.movementPhase == Crane.MovementPhaseType.picking)
            {
                //Get picked up
                if (theCrane.myBox == null &&
                    CollisionHelper.IntersectPixelsPoint(theCrane.GetCranePoint(), this) != CollisionHelper.NO_COLLIDE)
                {
                    theCrane.myBox = this;
                }
            }
            else if (theCrane.movementPhase == Crane.MovementPhaseType.dropping)
            {
                theCrane.myBox = null;
            }

            //Movement
            if (theCrane.myBox == this)
            {
                position = theCrane.GetCranePoint() - origin;
                rotation = theCrane.GetRotation();
            }
            else
            {
                foreach (SwitchBeltNetwork.Node t in theNet.boxNodes)
                {
                    if (lastNode != t &&
                        PathHelper.DistanceSquared(GetOriginPosition(), t.pos) < 4f*4f)
                    {
                        lastNode = t;
                        velocity = t.GetRandomDir(mH)*15f;
                        position = t.pos - origin;
                        break;
                    }
                }

                if (velocity == Vector2.Zero)
                {
                    SetShouldRemove(true);
                }

                rotation = PathHelper.Direction(velocity);
            }

            base.Update(mH);
        }

        public override bool ProjectileCheck(ManagerHelper mH)
        {
            if (theCrane.myBox == this)
            {
                return false;
            }
            else
            {
                return base.ProjectileCheck(mH);
            }
        }

        public override void DeathCode(ManagerHelper mH)
        {
            SetShouldRemove(true);
        }

        public void lowerHealth()
        {
            lifeCounter -= 5;
        }
    }
}