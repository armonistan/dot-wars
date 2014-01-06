using System;
using Microsoft.Xna.Framework;

namespace DotWars
{
    public class Juggernaut : NPC
    {
        public Juggernaut(String aN, Vector2 p)
            : base(aN, p)
        {
            health = maxHealth = 250; //A completely defensive unit. Like a tank
            movementSpeed = 120; //He's worthless if he can't keep up with the npc's he's trying to defend
            pathTimerEnd = 0.1f;

            awareness = 200; //Needs to see things far away
            sight = 300;

            affiliation = AffliationTypes.red;
        }

        //Returns the "threat" score of a dot. Lower is more dangerous
        public override int GetThreatLevel()
        {
            return 40;
        }

        protected override void Behavior(ManagerHelper mH)
        {
            //Do nothing
        }

        protected override Path SpecialPath(ManagerHelper mH)
        {
            //do i have friends
            NPC friend = mH.GetNPCManager().GetClosestInList(mH.GetNPCManager().GetAllies(affiliation), this);
            if (friend != null)
                target = mH.GetNPCManager()
                           .GetClosestInList(mH.GetNPCManager().GetAllButAllies(affiliation), friend.GetOriginPosition());
            Vector2 destination;

            //if so
            if (friend != null && target != null)
            {
                //we can now get a midpoint
                destination = PathHelper.MidPoint(friend.GetOriginPosition(), target.GetOriginPosition());
                //and wedge ourselves in
                return mH.GetPathHelper().FindClearPath(GetOriginPosition(), destination, mH);
            }
                //else, we shall wander the planes between heaven and hell
            else
                return RandomPath(mH);
        }

        protected override bool ProjectileCheck(ManagerHelper mH)
        {
            if (health <= 0)
            {
                return true;
            }

            else
            {
                foreach (Projectile p in mH.GetProjectileManager().GetProjectiles())
                {
                    if (p.GetDrawTime() > 0 && p.GetAffiliation() != affiliation &&
                        CollisionHelper.IntersectPixelsSimple(this, p) != new Vector2(-1))
                    {
                        lastDamagerDirection = PathHelper.DirectionVector(GetOriginPosition(), p.GetOriginPosition());
                        counter = 0;

                        var test = p.GetRotation() + Math.PI;
                        if (test > 2*Math.PI)
                        {
                            test -= 2*Math.PI;
                        }

                        if (Math.Abs(test - rotation) > (Math.PI*5/6))
                        {
                            ChangeHealth(-1*p.GetDamage(), p.GetCreator());
                            mH.GetParticleManager().AddBlood(this);

                            if (health <= 0)
                            {
                                return true;
                            }
                        }
                        else
                        {
                            Vector2 knockback = new Vector2((float)(p.velocity.X * .05), (float)(p.velocity.Y * .05));
                            accelerations.Add(knockback);
                            mH.GetAudioManager().Play("juggernautRicohet", (float).05, (float)-.5, 0, false);
                        }

                        p.SetDrawTime(0);
                    }

                    else
                        counter += mH.GetGameTime().ElapsedGameTime.TotalSeconds;

                    if (counter > 2)
                    {
                        counter = 0;
                        lastDamagerDirection = Vector2.Zero;
                    }
                }
            }

            return false;
        }
    }
}