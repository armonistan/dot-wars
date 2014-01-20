#region

using System;
using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    public class Crane : Environment
    {
        #region Declarations

        private readonly double endTime;

        private float endRotation;
        public MovementPhaseType movementPhase;

        public SwitchBox myBox;
        private double timer;

        #endregion

        public enum MovementPhaseType
        {
            picking,
            dropping,
            turningRight,
            turningLeft
        }

        public Crane(Vector2 p)
            : base("Backgrounds/Switch/switchCrane", p, Vector2.Zero)
        {
            movementPhase = MovementPhaseType.picking;

            timer = 0.0;
            endTime = 10.0;
            endRotation = 0f;
        }

        public override void Update(ManagerHelper mH)
        {
            if (movementPhase == MovementPhaseType.picking || movementPhase == MovementPhaseType.dropping)
            {
                if (timer > endTime)
                {
                    timer = 0.0;

                    if (mH.GetRandom().NextDouble() > 0.5)
                    {
                        movementPhase = MovementPhaseType.turningLeft;
                    }
                    else
                    {
                        movementPhase = MovementPhaseType.turningRight;
                    }

                    do
                    {
                        endRotation = MathHelper.Pi*mH.GetRandom().Next(4)/2f;
                    } while (endRotation == rotation);
                }
                else
                {
                    timer += mH.GetGameTime().ElapsedGameTime.TotalSeconds;
                }
            }
            else
            {
                if (MathHelper.Distance(rotation, endRotation) > 0.01f)
                {
                    Turn(((movementPhase == MovementPhaseType.turningLeft) ? -1.0f : 1.0f)*MathHelper.Pi/240.0f);
                }
                else
                {
                    rotation = endRotation;
                    if (myBox == null)
                    {
                        movementPhase = MovementPhaseType.picking;
                    }
                    else
                    {
                        movementPhase = MovementPhaseType.dropping;
                    }
                }
            }

            base.Update(mH);
        }

        public Vector2 GetCranePoint()
        {
            return GetOriginPosition() + PathHelper.Direction(rotation)*(origin.X - 28f);
        }
    }
}