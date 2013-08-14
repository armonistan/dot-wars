using System;
using Microsoft.Xna.Framework;

namespace DotWars
{
    public class Crane : Environment
    {
        #region Declarations

        private readonly float endTime;

        private float endRotation;
        public MovementPhaseType movementPhase;

        public SwitchBox myBox;
        private float timer;

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

            timer = 0;
            endTime = 10;
            endRotation = 0;
        }

        public override void Update(ManagerHelper mH)
        {
            if (movementPhase == MovementPhaseType.picking || movementPhase == MovementPhaseType.dropping)
            {
                if (timer > endTime)
                {
                    timer = 0;

                    if (mH.GetRandom().NextDouble() > 0.5f)
                    {
                        movementPhase = MovementPhaseType.turningLeft;
                    }
                    else
                    {
                        movementPhase = MovementPhaseType.turningRight;
                    }

                    do
                    {
                        endRotation = (float) (Math.PI*mH.GetRandom().Next(4)/2);
                    } while (endRotation == rotation);
                }
                else
                {
                    timer += (float) mH.GetGameTime().ElapsedGameTime.TotalSeconds;
                }
            }
            else
            {
                if (Math.Abs(rotation - endRotation) > 0.01)
                {
                    Turn((float) (((movementPhase == MovementPhaseType.turningLeft) ? -1 : 1)*Math.PI/240));
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
            return GetOriginPosition() + PathHelper.Direction(rotation)*(origin.X - 28);
        }
    }
}