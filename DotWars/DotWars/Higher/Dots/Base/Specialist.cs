#region

using System;
using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    public class Specialist : NPC
    {
        public Specialist(String aN, Vector2 p)
            : base(aN, p)
        {
            health = 125; //Above average health (he is a heavy trooper)
            maxHealth = health; //set up max health
            movementSpeed = 80; //He's a bit weighted down by all that gear. Below average speed
            shootingCounter = shootingSpeed = 1.8; //Powerful weapon but harder to reload. Medium Slow shooting speed 

            affiliation = AffliationTypes.red;
        }

        //Returns the "threat" score of a dot. Lower is more dangerous
        public override int GetThreatLevel()
        {
            return 10;
        }

        protected override void Shoot(ManagerHelper mH)
        {
            Vector2 tempPos = PathHelper.Direction(rotation + MathHelper.PiOver2)*10f;

            mH.GetProjectileManager()
              .AddProjectile(ProjectileManager.ROCKET, GetOriginPosition() + tempPos, this,
                             PathHelper.Direction(rotation)*300, 100, true, true, 5f);

            mH.GetAudioManager().Play(AudioManager.SPECIALIST_SHOOT, AudioManager.RandomVolume(mH),
                                      AudioManager.RandomPitch(mH), 0f, false);
        }
    }
}