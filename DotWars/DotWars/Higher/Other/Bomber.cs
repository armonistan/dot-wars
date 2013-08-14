using System;
using Microsoft.Xna.Framework;

namespace DotWars
{
    public class Bomber : NPC
    {
        private readonly Vector2 targetPosition; //the passed in position of the bomber's bombing target
        private Boolean hasBombed; //has the bomber dropped its load?
        private const int NUM_BOMBS = 3;

        public Bomber(Vector2 p, AffliationTypes a, NPC t, ManagerHelper mH)
            : base("", p)
        {
            hasBombed = false;
            targetPosition = t.GetOriginPosition();
            health = 100;
            movementSpeed = 1000;
            affiliation = a;

            //Set up rotation
            rotation = PathHelper.Direction(GetOriginPosition(), targetPosition);

            //Set up direction
            float dir = PathHelper.Direction(GetOriginPosition(), t.GetOriginPosition());
            velocity = new Vector2((float) Math.Cos(dir), (float) Math.Sin(dir))*movementSpeed;

            //Set up path
            path = new Path();
            path.Add(targetPosition, mH);

            //Set up affiliation
            switch (a)
            {
                case AffliationTypes.red:
                    asset = "Dots/Red/bomber_red";
                    break;
                case AffliationTypes.blue:
                    asset = "Dots/Blue/bomber_blue";
                    break;
                case AffliationTypes.green:
                    asset = "Dots/Green/bomber_green";
                    break;
                case AffliationTypes.yellow:
                    asset = "Dots/Yellow/bomber_yellow";
                    break;
            }

            mH.GetAudioManager().Play("planeFly", AudioManager.RandomVolume(mH),
                AudioManager.RandomPitch(mH), 0, false);
        }

        //This method will be used to dictate the AI's behavior in this public class
        public override void Update(ManagerHelper mH)
        {
            if (PathHelper.Distance(GetOriginPosition(), targetPosition) < 300 && !hasBombed)
            {
                Bomb(mH);
                hasBombed = true;
            }

            //remove thyself
            if (position.X < -200 || position.X > mH.GetLevelSize().X + 200 || position.Y < -200 ||
                position.Y > mH.GetLevelSize().Y + 200)
            {
                mH.GetNPCManager().Remove(this);
            }

            SpriteUpdate(mH);
        }

        private void Bomb(ManagerHelper mH)
        {
            for (int i = 0; i < NUM_BOMBS; i++)
            {
                mH.GetProjectileManager()
                  .AddProjectile("Projectiles/bullet_bombs", GetOriginPosition(), this,
                                 PathHelper.Direction(rotation) * 500, 50, true, 0.5f);
            }
        }

        protected override Path NewPath(ManagerHelper mH)
        {
            var aPath = new Path();
            path.Add(targetPosition, mH);

            return aPath;
        }
    }
}