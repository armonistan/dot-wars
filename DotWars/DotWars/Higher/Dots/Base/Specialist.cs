using System;
using Microsoft.Xna.Framework;

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
            shootingCounter = shootingSpeed = 1.5; //Powerful weapon but harder to reload. Medium Slow shooting speed 

            affiliation = AffliationTypes.red;
        }

        //Returns the "threat" score of a dot. Lower is more dangerous
        public override int GetThreatLevel()
        {
            return 10;
        }

        protected override void Shoot(ManagerHelper mH)
        {
            Vector2 tempPos = PathHelper.Direction(rotation + (float) (Math.PI/2))*new Vector2(10);

            mH.GetProjectileManager()
              .AddProjectile("Projectiles/bullet_rocket", GetOriginPosition() + tempPos, this,
                             PathHelper.Direction(rotation)*300, 100, true, 5);

            mH.GetAudioManager().Play("rocketShoot", AudioManager.RandomVolume(mH),
                AudioManager.RandomPitch(mH), 0, false);
        }
    }
}