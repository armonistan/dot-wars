using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DotWars
{
    internal class PlayerCommander : Commander
    {
        private float wantedRotation;

#if WINDOWS
        private KeyboardState oldState;
        #endif

        public PlayerCommander(string a, AffliationTypes aT, Vector2 p, ManagerHelper mH, double aS)
            : base(a, p, aS)
        {
            affiliation = aT;
            wantedRotation = rotation;
#if WINDOWS
            oldState = Keyboard.GetState();
#endif
            maxHealth = 225;
            health = maxHealth;

            shootingSpeed = 0.35f;
        }

        public override void Update(ManagerHelper mH)
        {
            //Check health
            if (ProjectileCheck(mH))
            {
                Explode(mH);
                return;
            }

#if WINDOWS
            KeyboardState keyState = mH.GetCurrentState();
            MouseState mouseState = Mouse.GetState();
#elif XBOX
            GamePadState theState =
                mH.GetCameraManager().GetCameras()[mH.GetCameraManager().GetPlayerInt(this.GetType())].GetState();
            GamePadState oldState =
                mH.GetCameraManager().GetCameras()[mH.GetCameraManager().GetPlayerInt(this.GetType())].GetOldState();
            #endif

            #region Shooting

#if WINDOWS
            if (mouseState.LeftButton == ButtonState.Pressed /* true*/)
            {
                if (weaponType == 0 && shootingCounter > shootingSpeed /* true*/)
                {
                    shootingCounter = 0;
                    Shoot(mH);
                }
                else if (weaponType == 1 && shootingCounter > shotgunShootingSpeed)
                {
                    shootingCounter = 0;
                    ShootShotgun(mH);
                }
            }
            else if (mouseState.RightButton == ButtonState.Pressed && grenadeCounter > grenadeSpeed)
            {
                if (grenadeType == 0)
                {
                    grenadeCounter = 0;
                    TossGrenade(mH);
                }
                else if (grenadeType == 1)
                {
                    grenadeCounter = 0;
                    TossFlare(mH);
                }
            }

            //Switch weapons
            if (keyState.IsKeyDown(Keys.E) && !oldState.IsKeyDown(Keys.E))
            {
                weaponType = (weaponType == 0) ? 1 : 0;

                modeIndex = weaponType;
            }
            else if (keyState.IsKeyDown(Keys.R) && !oldState.IsKeyDown(Keys.R))
            {
                grenadeType = (grenadeType == 0) ? 1 : 0;
            }
#elif XBOX
            if (theState.IsButtonDown(Buttons.RightTrigger))
            {
                if (weaponType == 0 && shootingCounter > shootingSpeed)
                {
                    shootingCounter = 0;
                    Shoot(mH);
                }
                else if (weaponType == 1 && shootingCounter > shotgunShootingSpeed)
                {
                    shootingCounter = 0;
                    ShootShotgun(mH);
                }
            }
            else if (theState.IsButtonDown(Buttons.LeftTrigger) && grenadeCounter > grenadeSpeed)
            {
                if (grenadeType == 0)
                {
                    grenadeCounter = 0;
                    TossGrenade(mH);
                }
                else if (grenadeType == 1)
                {
                    grenadeCounter = 0;
                    TossFlare(mH);
                }
            }
            
            //Switch weapons
            if (theState.IsButtonDown(Buttons.RightShoulder) && !oldState.IsButtonDown(Buttons.RightShoulder))
            {
                weaponType = (weaponType == 0) ? 1 : 0;
            
                modeIndex = weaponType;
            }
            else if (theState.IsButtonDown(Buttons.LeftShoulder) && !oldState.IsButtonDown(Buttons.LeftShoulder))
            {
                grenadeType = (grenadeType == 0) ? 1 : 0;
            }
#endif

            shootingCounter += mH.GetGameTime().ElapsedGameTime.TotalSeconds;
            grenadeCounter += mH.GetGameTime().ElapsedGameTime.TotalSeconds;

            #endregion

            #region Ability

#if WINDOWS
            if (keyState.IsKeyDown(Keys.Space) && !oldState.IsKeyDown(Keys.Space))
            {
                UsePower(mH);
            }

#elif XBOX
            if (theState.IsButtonDown(Buttons.A))
            {
                UsePower(mH);
            }
            #endif

            #endregion

            #region Movement

#if WINDOWS
            Vector2 dir = Vector2.Zero;

            if (keyState.IsKeyDown(Keys.W))
            {
                dir.Y = -1;
            }
            else if (keyState.IsKeyDown(Keys.S))
            {
                dir.Y = 1;
            }

            if (keyState.IsKeyDown(Keys.A))
            {
                dir.X = -1;
            }
            else if (keyState.IsKeyDown(Keys.D))
            {
                dir.X = 1;
            }


            if (dir != Vector2.Zero)
            {
                dir.Normalize();
                accelerations.Add(dir);
            }
#elif XBOX
    //Forward
            if (theState.ThumbSticks.Left.Y != 0 || theState.ThumbSticks.Left.X != 0)
            {
                accelerations.Add(new Vector2(theState.ThumbSticks.Left.X, theState.ThumbSticks.Left.Y * -1));
            }
            #endif

            #endregion

            #region Finalize Direction

            acceleration = Vector2.Zero;
            foreach (Vector2 a in accelerations)
            {
                acceleration += a;
            }
            drag = 0.1f;
            thrust = movementSpeed*drag;
            velocity += thrust*acceleration - drag*velocity;

            accelerations.Clear();

            #endregion

            #region Rotation

#if WINDOWS
            wantedRotation =
                PathHelper.Direction(
                    CameraManager.Transform(position, mH.GetCameraManager().GetDisplacement(GetType())) + origin,
                    new Vector2(mouseState.X, mouseState.Y));
            if (wantedRotation < 0)
            {
                wantedRotation += (float) Math.PI*2;
            }

            //Calculate turningSpeed to maximize speed and minimize jittering
            if (Math.Abs(rotation - wantedRotation) < turningSpeed && turningSpeed > (float) Math.PI/160)
            {
                turningSpeed /= 2;
            }
            else if (Math.Abs(rotation - wantedRotation) > turningSpeed && turningSpeed < maxTurningSpeed)
            {
                turningSpeed *= 2;
            }

            //Apply turningSpeed to rotation in correct direction
            float otherRot = rotation + ((float) Math.PI*2)*((rotation > Math.PI) ? -1 : 1);
                //Same angle, different name to compensate for linear numbers
            float distADir = Math.Abs(wantedRotation - rotation),
                  //Archlength sorta from actual rotation
                  distBDir = Math.Abs(wantedRotation - otherRot); //Archlength sorta from same angle but 2pi over

            //If the usual angle is closer
            if (distADir < distBDir)
            {
                //Do normal rotation
                if (rotation > wantedRotation)
                {
                    Turn(-1*turningSpeed);
                }
                else if (rotation < wantedRotation)
                {
                    Turn(turningSpeed);
                }
            }
                //Otherwise
            else
            {
                //Do a rotation using the new number, which is able to give the correct turning direction
                if (otherRot > wantedRotation)
                {
                    Turn(-1*turningSpeed);
                }
                else if (otherRot < wantedRotation)
                {
                    Turn(turningSpeed);
                }
            }

#elif XBOX
            if (theState.ThumbSticks.Right.X != 0 || theState.ThumbSticks.Right.Y != 0)
            {
                wantedRotation = (float)Math.Atan2(theState.ThumbSticks.Right.Y * -1, theState.ThumbSticks.Right.X);
                if (wantedRotation < 0)
                {
                    wantedRotation += (float)Math.PI * 2;
                }
            
                //Calculate turningSpeed to maximize speed and minimize jittering
                if (Math.Abs(rotation - wantedRotation) < turningSpeed && turningSpeed > (float)Math.PI / 160)
                {
                    turningSpeed /= 2;
                }
                else if (Math.Abs(rotation - wantedRotation) > turningSpeed && turningSpeed < maxTurningSpeed)
                {
                    turningSpeed *= 2;
                }
            
                //Apply turningSpeed to rotation in correct direction
                float otherRot = rotation + ((float)Math.PI * 2) * ((rotation > Math.PI) ? -1 : 1);//Same angle, different name to compensate for linear numbers
                float distADir = (float)Math.Abs(wantedRotation - rotation),//Archlength sorta from actual rotation
                    distBDir = (float)Math.Abs(wantedRotation - otherRot);//Archlength sorta from same angle but 2pi over
            
                //If the usual angle is closer
                if (distADir < distBDir)
                {
                    //Do normal rotation
                    if (rotation > wantedRotation)
                    {
                        Turn(-1 * turningSpeed);
                    }
                    else if (rotation < wantedRotation)
                    {
                        Turn(turningSpeed);
                    }
                }
                //Otherwise
                else
                {
                    //Do a rotation using the new number, which is able to give the correct turning direction
                    if (otherRot > wantedRotation)
                    {
                        Turn(-1 * turningSpeed);
                    }
                    else if (otherRot < wantedRotation)
                    {
                        Turn(turningSpeed);
                    }
                }
            }
            else
            {
                wantedRotation = rotation;
            }
            #endif

            #endregion

            PosUpdate(mH);

            if (indicator != null)
            {
                //Animation of indicator
                if (timer > endtime)
                {
                    indicator.SetFrameIndex(indicator.GetFrameIndex()+1);

                    if (indicator.GetFrameIndex() > 5)
                    {
                        indicator.SetFrameIndex(0);
                    }
                    timer = 0;
                }
                else
                {
                    timer += (float) mH.GetGameTime().ElapsedGameTime.TotalSeconds;
                }

                indicator.position = new Vector2(position.X - origin.X, position.Y - origin.Y);
                indicator.Update(mH);
            }

            ChargePower();

#if WINDOWS
            oldState = keyState;
#elif XBOX
            oldState = theState;
            #endif

            originPosition = position + origin;
        }

        protected override Path NewPath(ManagerHelper mH)
        {
            return new Path();
        }

        protected override void ShotgunSound(ManagerHelper mH)
        {
            mH.GetAudioManager().Play("commanderShotgun", 0.90f,
                (float)(mH.GetRandom().NextDouble() * -0.25), 0, false);
        }

        protected override void ShootSound(ManagerHelper mH)
        {
            mH.GetAudioManager().Play("commanderShoot", 0.90f,
                (float)(mH.GetRandom().NextDouble() * -0.25), 0, false);
        }

        protected override void FlareSound(ManagerHelper mH)
        {
            mH.GetAudioManager().Play("commanderFlare", 0.90f,
                   AudioManager.RandomPitch(mH), 0, false);
        }

        protected override void GrenadeSound(ManagerHelper mH)
        {
            mH.GetAudioManager().Play("commanderGrenade", 0.90f,
                   AudioManager.RandomPitch(mH), 0, false);
        }
    }
}