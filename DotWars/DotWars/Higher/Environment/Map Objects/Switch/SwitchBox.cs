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
            thrust = 500;
            drag = 0;
            lifeCounter = 30;
            theCrane = c;
            position -= origin;
            this.setHealth(75);
        }

        public override void Update(ManagerHelper mH)
        {
            if (lifeCounter < 0)
            {
                mH.GetEnvironmentManager().RemoveStaticBlocker(this);
            }

            lifeCounter -= mH.GetGameTime().ElapsedGameTime.TotalSeconds;

            //Code for getting picked up
            if (theCrane.movementPhase == Crane.MovementPhaseType.picking)
            {
                //Get picked up
                if (theCrane.myBox == null &&
                    CollisionHelper.IntersectPixelsPoint(theCrane.GetCranePoint(), this) != new Vector2(-1))
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
                        PathHelper.DistanceSquared(GetOriginPosition(), t.pos) < 4*4)
                    {
                        lastNode = t;
                        velocity = t.GetRandomDir(mH)*15;
                        position = t.pos - origin;
                        break;
                    }
                }

                if (velocity == Vector2.Zero)
                {
                    mH.GetEnvironmentManager().RemoveStaticBlocker(this);
                }

                rotation = PathHelper.Direction(velocity);
            }

            if (this.health < 0)
            {
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
            mH.GetEnvironmentManager().RemoveStaticBlocker(this);
        }

        public void lowerHealth()
        {
            lifeCounter -= 5;
        }
    }
}