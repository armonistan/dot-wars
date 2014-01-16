#region

using System;
using Microsoft.Xna.Framework;

#endregion

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

            affiliation = AffliationTypes.red;
        }

        //Returns the "threat" score of a dot. Lower is more dangerous
        public override int GetThreatLevel()
        {
            return 14;
        }

        protected override void Shoot(ManagerHelper mH)
        {
            Vector2 tempPos = PathHelper.Direction(rotation + MathHelper.PiOver2)*new Vector2(10);

            mH.GetProjectileManager()
              .AddProjectile(ProjectileManager.STANDARD, GetOriginPosition() + tempPos, this,
                             PathHelper.Direction(rotation)*350, 15, false, true, 1.1f);

            mH.GetAudioManager().Play(AudioManager.STANDARD_SHOOT, AudioManager.RandomVolume(mH),
                                      AudioManager.RandomPitch(mH), 0, false);
        }
    }
}