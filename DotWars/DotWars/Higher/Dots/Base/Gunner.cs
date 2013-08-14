using System;
using Microsoft.Xna.Framework;

namespace DotWars
{
    public class Gunner : NPC
    {
        public Gunner(String aN, Vector2 p)
            : base(aN, p)
        {
            health = 125; //Above average health
            maxHealth = health; //The units starting health will always be his max health
            movementSpeed = 80; //Slightly below average movement speed
            shootingSpeed = .15; //Fast fire rate

            vision = 120;

            affiliation = AffliationTypes.red;
        }

        //Returns the "threat" score of a dot. Lower is more dangerous
        public override int GetThreatLevel()
        {
            return 14;
        }

        protected override void Shoot(ManagerHelper mH)
        {
            Vector2 tempPos = PathHelper.Direction(rotation + (float) (Math.PI/2))*new Vector2(10);

            mH.GetProjectileManager()
              .AddProjectile("Projectiles/bullet_standard", GetOriginPosition() + tempPos, this,
                             PathHelper.Direction(rotation)*300, 15, false, 4);

            mH.GetAudioManager().Play("standardShoot", AudioManager.RandomVolume(mH),
                AudioManager.RandomPitch(mH), 0, false);
        }
    }
}