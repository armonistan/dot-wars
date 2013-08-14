using Microsoft.Xna.Framework;

namespace DotWars
{
    public class SwitchBox : Destructable
    {
        private readonly Crane theCrane;
        private readonly SwitchBeltNetwork theNet;
        private SwitchBeltNetwork.Node lastNode;
        private float lifeCounter;

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

            lifeCounter -= (float)mH.GetGameTime().ElapsedGameTime.TotalSeconds;

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
                for (int i = 0; i < theNet.boxNodes.Length; i++)
                {
                    if (lastNode != theNet.boxNodes[i] &&
                        PathHelper.Distance(GetOriginPosition(), theNet.boxNodes[i].pos) < 2)
                    {
                        lastNode = theNet.boxNodes[i];
                        velocity = theNet.boxNodes[i].GetRandomDir(mH) * 15;
                        position = theNet.boxNodes[i].pos - origin;
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