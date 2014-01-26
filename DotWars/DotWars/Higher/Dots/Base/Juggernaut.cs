#region

using System;
using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    public class Juggernaut : NPC
    {
        public Juggernaut(String aN, Vector2 p)
            : base(aN, p)
        {
            health = maxHealth = 200; //A completely defensive unit. Like a tank
            movementSpeed = 120; //He's worthless if he can't keep up with the npc's he's trying to defend
            pathTimerEnd = 0.1;

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

        protected override void SpecialPath(ManagerHelper mH)
        {
            //do i have friends
            NPC friend = mH.GetNPCManager().GetClosestInList(mH.GetNPCManager().GetAllies(affiliation), this);

            if (friend != null)
            {
                float closestDistancetoTarget = float.PositiveInfinity;
                target = null;

                foreach (var team in mH.GetGametype().GetTeams())
                {
                    if (team != affiliation)
                    {
                        NPC closestTargetForTeam = mH.GetNPCManager()
                                                     .GetClosestInList(mH.GetNPCManager().GetAllies(team),
                                                                       friend.GetOriginPosition());

                        if (closestTargetForTeam != null)
                        {
                            float closestDiestanceToTargetForTeam = PathHelper.DistanceSquared(
                                closestTargetForTeam.GetOriginPosition(), friend.GetOriginPosition());

                            if (closestDiestanceToTargetForTeam < closestDistancetoTarget)
                            {
                                target = closestTargetForTeam;
                                closestDistancetoTarget = closestDiestanceToTargetForTeam;
                            }
                        }
                    }
                }
            }
            Vector2 destination;

            //if so
            if (friend != null && target != null)
            {
                //we can now get a midpoint
                destination = PathHelper.MidPoint(friend.GetOriginPosition(), target.GetOriginPosition());
                //and wedge ourselves in
                mH.GetPathHelper().FindClearPath(GetOriginPosition(), destination, mH, path);
            }
                //else, we shall wander the planes between heaven and hell
            else
                RandomPath(mH);
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
                        CollisionHelper.IntersectPixelsSimple(this, p) != CollisionHelper.NO_COLLIDE)
                    {
                        lastDamagerDirection = PathHelper.DirectionVector(GetOriginPosition(), p.GetOriginPosition());
                        counter = 0;

                        p.SetDrawTime(0);

                        float test = p.GetRotation() + MathHelper.Pi;
                        if (test > MathHelper.TwoPi)
                        {
                            test -= MathHelper.TwoPi;
                        }

                        if (MathHelper.Distance(test, rotation) > (MathHelper.Pi*5/6))
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
                            Vector2 knockback = new Vector2((float) (p.velocity.X*.05), (float) (p.velocity.Y*.05));
                            AddAcceleration(knockback);
                            mH.GetAudioManager().Play(AudioManager.JUGGERNAUT_RICOHET, (float) .05, 0, 0, false);
                        }
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