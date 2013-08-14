﻿using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace DotWars
{
    public class NPC : Sprite
    {
        #region Static Members

        public enum AffliationTypes
        {
            red,
            blue,
            green,
            yellow,
            grey,
            black,
            same
        }

        public static int buffer = 1;

        #endregion

        #region Declarations

        protected AffliationTypes affiliation; //the npc's affliation
        protected AffliationTypes personalAffiliation;

        protected int health; //sets up generic npc health
        protected int maxHealth; //Cap for generic NPC health
        protected NPC lastDamager;
        protected Vector2 lastDamagerDirection;

        protected float awareness; // Radius that dot is aware of
        protected float vision; //Arch-length that NPC can see
        protected float sight; // Distance to see forward

        protected double counter; //ambiguous name is necessary
        protected double grenadeCounter; //counter used to act (usually but not always shooting)
        protected double grenadeSpeed;
        protected double shootingCounter; //counter used to act (usually but not always shooting)
        protected double shootingSpeed; //a counter for the speed at which a npc shoots
        protected NPC target;

        protected readonly float maxTurningSpeed; // Prevents dots from turning too fast
        protected int movementSpeed; //sets up generic npc movement speed
        protected float turningSpeed; // Used to smoothly turn

        //Path used for going
        protected Path path;
        protected float pathTimer;
        protected float pathTimerEnd;

        private int otherUpdate;

        private bool fireStatus;
        #endregion

        protected NPC(String a, Vector2 p) :
            base(a, p)
        {
            affiliation = AffliationTypes.grey;
            health = 100;
            maxHealth = 100;

            path = new Path();
            pathTimer = 11;
            pathTimerEnd = 5; //TODO: Find out if this is reasonable
            velocity = new Vector2(1, 0)*movementSpeed;
            drag = 0;


            movementSpeed = 75;
            shootingSpeed = 0.2f;
            grenadeSpeed = 3f;
            turningSpeed = maxTurningSpeed = (float) Math.PI/40;

            awareness = 150;
            sight = 250;
            vision = (float) Math.PI/3;

            shootingCounter = 0;
            grenadeCounter = 0;

            otherUpdate = 0;

            lastDamager = null;
            fireStatus = false;
        }

        public override void Update(ManagerHelper mH)
        {
            //Check health
            if (ProjectileCheck(mH))
            {
                Explode(mH);
                return;
            }

            //Set up dir for rotations
            float dir = rotation;

            //TODO: What is going on here?
            if (otherUpdate < 3)
            {
                otherUpdate = 0;

                //Do Behaviour
                Behavior(mH);

                dir = MoveNPC(mH, dir);

                RotateNPC(dir);
            }
            else
            {
                otherUpdate++;

                #region movement

                if (path.Count > 0)
                {
                    //Get next destination
                    Vector2 next = path.First();

                    //Find angle between points
                    dir = PathHelper.Direction(base.GetOriginPosition(), next);

                    //Get x and y values from angle and set up direction
                    accelerations.Add(PathHelper.Direction(dir));

                    //If already there...
                    if (PathHelper.Distance(next, GetOriginPosition()) < 16)
                    {
                        //Go on to next destination
                        path.RemoveLast();
                    }
                }

                #endregion
            }
            /*
            #region Calculate EndPath

            if (path != null)
            {
                float d = path.GetDistance();
                const float b = (float) .005;
                pathTimerEnd = d*b;
            }

            #endregion
            */
            PosUpdate(mH);
            originPosition = position + origin; //Update originPosition
        }

        #region Update Submethods

        private float MoveNPC(ManagerHelper mH, float dir)
        {
            //If path is null, ie stay in same spot
            if (path != null)
            {
                //If there are still destinations
                if (pathTimer >= pathTimerEnd || path.Count == 0)
                {
                    path = NewPath(mH);
                    pathTimer = 0;
                }
                else
                {
                    Vector2 next = path.First(); //Get next destination
                    dir = PathHelper.Direction(base.GetOriginPosition(), next); //Find angle between points
                    accelerations.Add(PathHelper.Direction(dir));
                        //Get x and y values from angle and set up direction

                    //If already there...
                    if (PathHelper.Distance(next, GetOriginPosition()) < 15)
                    {
                        path.RemoveFirst(); //Go on to next destination
                    }

                    pathTimer += (float) mH.GetGameTime().ElapsedGameTime.TotalSeconds;
                }
            }

            //Finalize direction
            foreach (Vector2 a in accelerations)
            {
                acceleration += a;
            }
            drag = 0.05f;
            thrust = movementSpeed*drag;
            velocity += thrust*acceleration - drag*velocity;

            accelerations.Clear();
            acceleration = Vector2.Zero;

            return dir;
        }

        private void RotateNPC(float dir)
        {
            //If you have a target, then set the rotation direction to face that instead
            if (target != null)
            {
                dir = PathHelper.Direction(GetOriginPosition(), target.GetOriginPosition());
            }

            //Calculate turningSpeed to maximize speed and minimize jittering
            if (Math.Abs(rotation - dir) < turningSpeed && turningSpeed > (float) Math.PI/160)
            {
                turningSpeed /= 2;
            }
            else if (Math.Abs(rotation - dir) > turningSpeed && turningSpeed < maxTurningSpeed)
            {
                turningSpeed *= 2;
            }

            //Apply turningSpeed to rotation in correct direction
            float otherRot = rotation + ((float) Math.PI*2)*((rotation > Math.PI) ? -1 : 1);
            //Same angle, different name to compensate for linear numbers
            float distADir = Math.Abs(dir - rotation),
                  //Archlength sorta from actual rotation
                  distBDir = Math.Abs(dir - otherRot); //Archlength sorta from same angle but 2pi over

            //If the usual angle is closer
            if (distADir < distBDir)
            {
                //Do normal rotation
                if (rotation > dir)
                {
                    Turn(-1*turningSpeed);
                }
                else if (rotation < dir)
                {
                    Turn(turningSpeed);
                }
            }
                //Otherwise
            else
            {
                //Do a rotation using the new number, which is able to give the correct turning direction
                if (otherRot > dir)
                {
                    Turn(-1*turningSpeed);
                }
                else if (otherRot < dir)
                {
                    Turn(turningSpeed);
                }
            }
        }

        protected void SpriteUpdate(ManagerHelper mH)
        {
            base.Update(mH);
        }

        protected virtual void PosUpdate(ManagerHelper mH)
        {
            //Update position
            Vector2 tempPos = position + velocity*(float) mH.GetGameTime().ElapsedGameTime.TotalSeconds;
            Vector2 tempOPos = tempPos + origin;
            float distSquared = 1000000;

            //Collisions
            foreach (NPC a in mH.GetNPCManager().GetAllButAllies(affiliation)) //first go through every single npc
            {
                if (distSquared > PathHelper.DistanceSquared(a.GetOriginPosition(), tempOPos)) 
                {
                    Vector2 tempVect = CollisionHelper.IntersectPixelsSimple(this, a);
                    if (tempVect != new Vector2(-1))
                    {
                        velocity = CollisionHelper.CollideRandom(GetOriginPosition(), tempVect)*movementSpeed;
                    }
                }
            }

            foreach (LargeRock r in mH.GetAbilityManager().GetLargeRocks())
            {
                if (distSquared > PathHelper.DistanceSquared(r.GetOriginPosition(), tempOPos))
                {
                    int tempVect = CollisionHelper.IntersectPixelsDirectionalRaw(this, tempOPos, r);
                    if (tempVect != -1)
                    {
                        velocity = CollisionHelper.CollideSimple(tempVect, velocity);
                    }
                }
            }

            foreach (Environment e in mH.GetEnvironmentManager().GetStaticBlockers())
            {
                if (distSquared > PathHelper.DistanceSquared(e.GetOriginPosition(), tempOPos))
                {
                    int tempVect = CollisionHelper.IntersectPixelsDirectionalRaw(this, tempOPos, e);
                    if (tempVect != -1)
                    {
                        if (e is SwitchBox) {
                            SwitchBox box = (SwitchBox)e;
                            box.lowerHealth();
                        }
                        velocity = CollisionHelper.CollideSimple(tempVect, velocity);
                    }
                }
            }

            foreach (Impassable e in mH.GetEnvironmentManager().GetImpassables())
            {
                if (distSquared > PathHelper.DistanceSquared(e.GetOriginPosition(), tempOPos))
                {
                    int tempVect = CollisionHelper.IntersectPixelsDirectionalRaw(this, tempOPos, e);
                    if (tempVect != -1)
                    {
                        velocity = CollisionHelper.CollideDirectional(velocity, tempVect);
                    }
                }
            }

            tempPos = position + velocity*(float) mH.GetGameTime().ElapsedGameTime.TotalSeconds;

            if (
                !(tempPos.X < buffer || tempPos.X > mH.GetLevelSize().X - frame.Width - buffer || tempPos.Y < buffer ||
                  tempPos.Y > mH.GetLevelSize().Y - frame.Height - buffer))
            {
            }
            else
            {
                if (tempPos.X <= buffer)
                {
                    tempPos.X = buffer;
                }
                else if (tempPos.X > mH.GetLevelSize().X - frame.Width - buffer)
                {
                    tempPos.X = mH.GetLevelSize().X - frame.Width - buffer;
                }

                if (tempPos.Y <= buffer)
                {
                    tempPos.Y = buffer;
                }
                else if (tempPos.Y > mH.GetLevelSize().Y - frame.Height - buffer)
                {
                    tempPos.Y = mH.GetLevelSize().Y - frame.Height - buffer;
                }
            }

            position = tempPos;

            //Update frames
            frame.X = frameIndex*frame.Width;
            frame.Y = modeIndex*frame.Height;
        }

        protected virtual void Behavior(ManagerHelper mH)
        {
            target = TargetDecider(mH);

            if (shootingCounter > shootingSpeed && target != null)
            {
                shootingCounter = 0;
                Shoot(mH);
            }
            else
            {
                shootingCounter += mH.GetGameTime().ElapsedGameTime.TotalSeconds;
            }
        }

        protected virtual bool ProjectileCheck(ManagerHelper mH)
        {
            if (health <= 0)
            {
                return true;
            }

            if (mH.GetGametype() is Survival)
                return false;
            else
            {
                foreach (Projectile p in mH.GetProjectileManager().GetProjectiles())
                {
                    if (p.GetDrawTime() > 0 && p.GetAffiliation() != affiliation &&
                        CollisionHelper.IntersectPixelsSimple(this, p) != new Vector2(-1))
                    {
                        lastDamagerDirection = PathHelper.DirectionVector(GetOriginPosition(), p.GetOriginPosition());
                        ChangeHealth(-1*p.GetDamage(), p.GetCreator());
                        mH.GetParticleManager().AddBlood(this);
                        counter = 0;

                        if (health <= 0)
                        {
                            return true;
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

        protected virtual Path NewPath(ManagerHelper mH)
        {
            return SpecialPath(mH);
        }

        protected virtual void Explode(ManagerHelper mH)
        {
            for (int i = 0; i < 4; i++)
            {
                mH.GetParticleManager().AddGut(this, i);
            }

            mH.GetNPCManager().Remove(this);

            if ((mH.GetGametype() is Assasssins || mH.GetGametype() is Deathmatch) && lastDamager != null) 
            {
                mH.GetGametype().ChangeScore(lastDamager, 1);
            }
        }

        #endregion

        #region Attacking Functions

        protected bool GrenadeDecider(ManagerHelper mH)
        {
            if (mH.GetNPCManager().GetAlliesInRadius(target.affiliation, target.GetOriginPosition(), 100).Count > 2)
                return true;
            return false;
        }

        protected virtual NPC TargetDecider(ManagerHelper mH)
        {
            if (mH.GetGametype() is Survival)
                return
                    mH.GetNPCManager()
                      .GetClosestInList(
                          mH.GetNPCManager()
                            .GetAlliesInDirection(AffliationTypes.black, GetOriginPosition(), sight, rotation, vision),
                          this);

            return
                mH.GetNPCManager()
                  .GetRandInList(
                      mH.GetNPCManager()
                        .GetAllButAlliesInDirection(affiliation, GetOriginPosition(), sight, rotation, vision), this);
        }

        protected virtual void Shoot(ManagerHelper mH)
        {
            Vector2 tempPos = PathHelper.Direction(rotation + (float) (Math.PI/2))*new Vector2(10);

            mH.GetProjectileManager()
              .AddProjectile("Projectiles/bullet_standard", GetOriginPosition() + tempPos, this,
                             PathHelper.Direction(rotation + (float) mH.GetRandom().NextDouble()/8 - 0.0625f)*300, 25,
                             false, 5);

            mH.GetAudioManager().Play("standardShoot", AudioManager.RandomVolume(mH),
                AudioManager.RandomPitch(mH), 0, false);
        }

        protected void TossGrenade(ManagerHelper mH)
        {
            Vector2 tempPos = PathHelper.Direction(rotation + (float) (Math.PI/2))*new Vector2(10);

            mH.GetProjectileManager()
              .AddTossable("Projectiles/grenade", GetOriginPosition() + tempPos, this,
                           PathHelper.Direction(rotation)*500, 100, true, 1.5f);

            GrenadeSound(mH);
        }

        protected virtual void GrenadeSound(ManagerHelper mH)
        {
            mH.GetAudioManager().Play("grenadeShoot", AudioManager.RandomVolume(mH),
                   AudioManager.RandomPitch(mH), 0, false);
        }

        #endregion

        #region Gets and Sets
        public AffliationTypes GetPersonalAffilation()
        {
            return personalAffiliation;
        }

        public virtual int GetPowerStatistic()
        {
            return 0;
        }


        public int GetHealth()
        {
            return health;
        }

        public AffliationTypes GetAffiliation()
        {
            return affiliation;
        }

        public float GetAwareness()
        {
            return awareness;
        }

        public float GetSight()
        {
            return sight;
        }

        public float GetVision()
        {
            return vision;
        }

        public NPC GetLastDamager()
        {
            return lastDamager;
        }

        public float GetPercentHealth()
        {
            return (float) health/maxHealth;
        }

        public int GetMaxHealth()
        {
            return maxHealth;
        }

        public void ChangeHealth(int hM, NPC lD)
        {
            health += hM;

            if (health > maxHealth)
            {
                health = maxHealth;
            }

            if (lD != null && lD.affiliation != affiliation && lD.affiliation != AffliationTypes.same)
            {
                lastDamager = lD;
            }
        }

        public void ChangeHealth(int hM, NPC.AffliationTypes lD)
        {
            health += hM;

            if (health > maxHealth)
            {
                health = maxHealth;
            }
        }

        public int GetDistanceScore(NPC a, NPC b, ManagerHelper mH)
        {
            float distance = PathHelper.Distance(a.GetOriginPosition(), b.GetOriginPosition());
                // distance between two npcs
            var distanceScore = (int) (distance/10); //raw score

            return distanceScore;
        }

        public int GetHealthScore(NPC t)
        {
            int healthScore = t.GetHealth(); //raw score

            return healthScore;
        }

        public virtual int GetThreatLevel()
        {
            return 0;
        }

        public Path GetPath()
        {
            return path;
        }

        public virtual void UpdatePowerStatistic()
        {
        }

        public bool GetFireStatus()
        {
            return fireStatus;
        }
        #endregion

        public void ChangeFireStatus()
        {
            fireStatus = !fireStatus;
        }

        #region Paths

        protected Path RandomPath(ManagerHelper mH)
        {
            bool validPoint;
            Vector2 randPoint;
            pathTimerEnd = 10;

            do
            {
                validPoint = true;
                randPoint = new Vector2(mH.GetRandom().Next((int) mH.GetLevelSize().X),
                                        mH.GetRandom().Next((int) mH.GetLevelSize().Y)) / mH.GetPathHelper().GetNodeSize();

                foreach (Environment e in mH.GetEnvironmentManager().GetStaticBlockers())
                {
                    foreach (Vector2 n in e.GetFrameBlockers())
                    {
                        if (n.Equals(randPoint))
                        {
                            validPoint = false;
                            break;
                        }
                        else
                        {
                            validPoint = true;
                        }
                    }

                    if (!validPoint)
                    {
                        break;
                    }
                }
            } while (!validPoint);

            return mH.GetPathHelper().FindClearPath(GetOriginPosition(), randPoint*32, mH);
        }

        protected virtual Path SpecialPath(ManagerHelper mH)
        {
            if (mH.GetGametype() is Conquest)
            {
                return ConquestPath(mH);
            }
            else if (mH.GetGametype() is CaptureTheFlag)
            {
                return CTFPath(mH);
            }
            else if (mH.GetGametype() is Assault)
            {
                return AssaultPath(mH);
            }
            else if (mH.GetGametype() is Deathmatch)
            {
                return DeathmatchPath(mH);
            }
            else if (mH.GetGametype() is Survival)
            {
                pathTimerEnd = 0.1f;
                return SurvivalPath(mH);
            }
            else
            {
                return EngagePath(mH);
            }
        }

        protected Path EngagePath(ManagerHelper mH)
        {
            NPC tempEnemy;

            if (mH.GetGametype() is Survival)
            {
                tempEnemy = target ?? mH.GetNPCManager()
                                        .GetClosestInList(mH.GetNPCManager().GetAllies(AffliationTypes.black), this);
            }
            else
            {
                tempEnemy = target ?? mH.GetNPCManager()
                                        .GetClosestInList(mH.GetNPCManager().GetAllButAllies(affiliation),
                                                          GetOriginPosition());
            }

            if (tempEnemy != null)
            {
                if (PathHelper.Distance(GetOriginPosition(), tempEnemy.GetOriginPosition()) > 128)
                {
                    return mH.GetPathHelper().FindClearPath(GetOriginPosition(), tempEnemy.GetOriginPosition(), mH);
                    
                }
                else
                {
                    return HoverPath(mH, GetOriginPosition(), 64);
                }
            }
            else
            {
                return RandomPath(mH);
            }
        }

        protected Path FlarePath(ManagerHelper mH)
        {
            Flare f = mH.GetProjectileManager().GetFlare(affiliation);
            float tempDist = PathHelper.Distance(GetOriginPosition(), f.GetOriginPosition());

            if (!f.IsInList(this) && tempDist > f.GetFlareRadius())
            {
                return mH.GetPathHelper().FindClearPath(GetOriginPosition(), f.GetOriginPosition(), mH);
            }
            else
            {
                f.AddToList(this);
                return SpecialPath(mH);
            }
        }

        protected Path HoverPath(ManagerHelper mH, Vector2 p, int r)
        {
            bool validPoint;
            Vector2 randPoint;
            Vector2 originNode = GetOriginPosition()/mH.GetPathHelper().GetNodeSize();

            do
            {
                validPoint = true;
                randPoint = originNode + new Vector2(mH.GetRandom().Next(-1*r/32, r/32), mH.GetRandom().Next(-1*r/32, r/32));

                if ((randPoint.X > 0 && randPoint.X < mH.GetLevelSize().X/32) &&
                    (randPoint.Y > 0 && randPoint.Y < mH.GetLevelSize().Y/32))
                {
                    foreach (Environment e in mH.GetEnvironmentManager().GetStaticBlockers())
                    {
                        foreach (Vector2 n in e.GetFrameBlockers())
                        {
                            if ((n.X + (int) e.GetOriginPosition().X) == randPoint.X &&
                                (n.Y + (int) e.GetOriginPosition().Y) == randPoint.Y)
                            {
                                validPoint = false;
                                break;
                            }
                        }

                        if (!validPoint)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    validPoint = false;
                }
            } while (!validPoint);

            return mH.GetPathHelper().FindClearPath(GetOriginPosition(), randPoint*32, mH);
        }

        protected virtual Path DeathmatchPath(ManagerHelper mH)
        {
            return EngagePath(mH);
        }

        protected virtual Path ConquestPath(ManagerHelper mH)
        {
            return EngagePath(mH);
        }

        protected virtual Path CTFPath(ManagerHelper mH)
        {
            var ctf = (CaptureTheFlag) mH.GetGametype();
            Flag f = ctf.GetAllyBase(affiliation).GetMyFlag();

            if (f.status == Flag.FlagStatus.away)
            {
                return mH.GetPathHelper().FindClearPath(GetOriginPosition(), f.GetOriginPosition(), mH);
            }
            else
            {
                return EngagePath(mH);
            }
        }

        protected virtual Path AssaultPath(ManagerHelper mH)
        {
            var ass = (Assault) mH.GetGametype();
            Flag f = ass.GetAllyBase(affiliation).GetMyFlag();

            if (f != null && f.status == Flag.FlagStatus.away)
            {
                return mH.GetPathHelper().FindClearPath(GetOriginPosition(), f.GetOriginPosition(), mH);
            }
            else
            {
                return EngagePath(mH);
            }
        }

        protected virtual Path SurvivalPath(ManagerHelper mH)
        {
            var c = (Commander) mH.GetNPCManager().GetCommander(affiliation);

            if (c != null)
            {
                if (PathHelper.Distance(c.GetOriginPosition(), GetOriginPosition()) < 96)
                {
                    return HoverPath(mH, c.GetOriginPosition(), 96);
                }
                else
                {
                    return mH.GetPathHelper().FindClearPath(GetOriginPosition(), c.GetOriginPosition(), mH);
                }
            }
            else
            {
                return RandomPath(mH);
            }
        }

        #endregion

        public static NPC.AffliationTypes CommanderColor(Type commander)
        {
            if (commander == typeof (RedCommander) || commander == typeof (RedPlayerCommander))
            {
                return AffliationTypes.red;
            }
            else if (commander == typeof(BlueCommander) || commander == typeof(BluePlayerCommander))
            {
                return AffliationTypes.blue;
            }
            else if (commander == typeof(GreenCommander) || commander == typeof(GreenPlayerCommander))
            {
                return AffliationTypes.green;
            }
            else if (commander == typeof(YellowCommander) || commander == typeof(YellowPlayerCommander))
            {
                return AffliationTypes.yellow;
            }

            return AffliationTypes.grey;
        }

        public static int GetTeam(NPC.AffliationTypes a)
        {
            switch (a)
            {
                case NPC.AffliationTypes.red:
                    return 0;
                case NPC.AffliationTypes.blue:
                    return 1;
                case NPC.AffliationTypes.green:
                    return 2;
                case NPC.AffliationTypes.yellow:
                    return 3;
                default:
                    return 0;
            }
        }
    }
}