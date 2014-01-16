using System;
using Microsoft.Xna.Framework;

namespace DotWars
{
    public class Bomber : NPC
    {
        private readonly Vector2 targetPosition; //the passed in position of the bomber's bombing target
        private Sprite targetSprite;
        private Boolean hasBombed; //has the bomber dropped its load?
        private Boolean drawTarget;
        private const int NUM_BOMBS = 3;

        public Bomber(Vector2 p, AffliationTypes a, NPC t, ManagerHelper mH)
            : base("", p)
        {
            hasBombed = false;
            drawTarget = true;
            targetPosition = t.GetOriginPosition();
            health = 100;
            movementSpeed = 1000;
            affiliation = a;

            //Set up rotation
            rotation = PathHelper.Direction(GetOriginPosition(), targetPosition);

            //Set up direction
            float dir = PathHelper.Direction(GetOriginPosition(), t.GetOriginPosition());
            velocity = new Vector2(DWMath.Cos(dir), DWMath.Sin(dir))*movementSpeed;

            //Set up path
            path = new Path();
            path.AddPoint(targetPosition);

            string targeAsset = "";

            //Set up affiliation
            switch (a)
            {
                case AffliationTypes.red:
                    targeAsset = "Dots/Red/targetRed";
                    asset = "Dots/Red/bomber_red";
                    break;
                case AffliationTypes.blue:
                    targeAsset = "Dots/Blue/targetBlue";
                    asset = "Dots/Blue/bomber_blue";
                    break;
                case AffliationTypes.green:
                    targeAsset = "Dots/Green/targetGreen";
                    asset = "Dots/Green/bomber_green";
                    break;
                case AffliationTypes.yellow:
                    targeAsset = "Dots/Yellow/targetYellow";
                    asset = "Dots/Yellow/bomber_yellow";
                    break;
            }

            targetSprite = new Sprite(targeAsset, targetPosition);

            mH.GetAudioManager().Play(AudioManager.PLANE, AudioManager.RandomVolume(mH),
                AudioManager.RandomPitch(mH), 0, false);
        }

        public override void LoadContent(TextureManager tM)
        {
            base.LoadContent(tM);
            targetSprite.LoadContent(tM);
        }

        //This method will be used to dictate the AI's behavior in this public class
        public override void Update(ManagerHelper mH)
        {
            double distanceToTarget = PathHelper.DistanceSquared(GetOriginPosition(), targetPosition);

            if (distanceToTarget < 300 * 300 && !hasBombed)
            {
                Bomb(mH);
                hasBombed = true;
            }

            if (distanceToTarget < 50 * 50)
            {
                drawTarget = false;
            }

            //remove thyself
            if (position.X < -200 || position.X > mH.GetLevelSize().X + 200 || position.Y < -200 ||
                position.Y > mH.GetLevelSize().Y + 200)
            {
                mH.GetNPCManager().Remove(this);
            }

            int newTargetSpriteIndex =
                (int)
                ((targetSprite.GetTotalFrames() - 1)*
                 Math.Max(0, Math.Min(1000 * 1000, 1000 * 1000 - PathHelper.DistanceSquared(GetOriginPosition(), targetPosition))) / 1000 * 1000);
            targetSprite.SetFrameIndex(newTargetSpriteIndex);

            SpriteUpdate(mH);
            targetSprite.Update(mH);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sB, Vector2 displacement, ManagerHelper mH)
        {
            if (drawTarget)
            {
                targetSprite.Draw(sB, displacement, mH);
            }
            base.Draw(sB, displacement, mH);
        }

        private void Bomb(ManagerHelper mH)
        {
            for (int i = 0; i < NUM_BOMBS; i++)
            {
                mH.GetProjectileManager()
                  .AddProjectile(ProjectileManager.BOMB, GetOriginPosition(), this,
                                 PathHelper.Direction(rotation) * 500, 50, true, false, 0.5f);
            }
        }

        protected override void NewPath(ManagerHelper mH)
        {
            path.AddPoint(targetPosition);
        }
    }
}