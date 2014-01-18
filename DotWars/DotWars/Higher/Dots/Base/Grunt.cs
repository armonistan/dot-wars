#region

using System;
using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    public class Grunt : NPC
    {
        public Grunt(String aN, Vector2 p)
            : base(aN, p)
        {
            health = 100; //Standard health
            maxHealth = health; //The units starting health will always be his max health
            movementSpeed = 125; //Standard movement speed
            shootingSpeed = .5; //Standard shooting speed in seconds

            grenadeCounter = 0; //default
            grenadeSpeed = 3; //Three second recharge


            affiliation = AffliationTypes.red;
        }

        //Returns the "threat" score of a dot. Lower is more dangerous
        public override int GetThreatLevel()
        {
            return 20;
        }

        protected override void Behavior(ManagerHelper mH)
        {
            target = TargetDecider(mH);

            if (target != null)
            {
                if (!(GrenadeDecider(mH) && grenadeCounter > grenadeSpeed) && shootingCounter > shootingSpeed)
                {
                    shootingCounter = 0;
                    Shoot(mH);
                }
                else
                {
                    shootingCounter += mH.GetGameTime().ElapsedGameTime.TotalSeconds;
                }

                if (GrenadeDecider(mH) && grenadeCounter > grenadeSpeed)
                {
                    grenadeCounter = 0;
                    TossGrenade(mH);
                }
                else
                    grenadeCounter += mH.GetGameTime().ElapsedGameTime.TotalSeconds;
            }
        }
    }
}