#region

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace DotWars
{
    public class Commander : NPC
    {
        #region Declarations

        //Variables unique to the commander

        //Constants unique to the commander
        public const float SHOTGUNRANGE = 128;
        protected double abilityCounter;
        protected double abilityMax;
        protected double abilityUse;
        protected double abilityCharge;

        //Indicator stuff
        protected double endtime;
        public int flareCount;
        public int grenadeType;
        protected Sprite indicator;
        public double shotgunShootingSpeed;
        protected double timer;
        public int weaponType; //0 is the machine gun, 1 is shot gun

        private const string SHOTGUN_SHOOT = "shotgunShoot";
        private const string FLARE_SHOOT = "flareShoot";
        #endregion

        public Commander(String aN, Vector2 p)
            : base(aN, p)
        {
            health = 225; // Large amount of health, a health commander can take 3 sniper hits
            maxHealth = health; //The units starting health will always be his max health
            movementSpeed = 135; //Above average movement speed. He is an elite, no?
            shootingSpeed = .2; //Above average shooting speed for machine gun in seconds
            shotgunShootingSpeed = 1.2; //Average shooting speed for shotgun
            weaponType = 0; //machine gun default
            grenadeType = 0;

            flareCount = 0; //Number of flares active

            grenadeCounter = 3; //default
            grenadeSpeed = 3; //Three second recharge

            //Ability Data
            abilityMax = abilityCounter = 100;
            abilityUse = 50;
            abilityCharge = 0.3;

            awareness = 500;
            vision = MathHelper.PiOver2;
            sight = 300;
            turningSpeed = MathHelper.Pi/20;

            affiliation = AffliationTypes.red;

            //Set up indicator stuff
            timer = 0;
            endtime = 0.07f;

            protectedTimer = 2;
        }

        public override void LoadContent(TextureManager tM)
        {
            if (indicator != null)
                indicator.LoadContent(tM);

            base.LoadContent(tM);
        }

        //This method will be used to dictate the AI's behavior in this public class
        public override void Update(ManagerHelper mH)
        {
            base.Update(mH);

            if (indicator != null)
            {
                //Animation of indicator
                if (timer > endtime)
                {
                    indicator.SetFrameIndex(indicator.GetFrameIndex() + 1);

                    if (indicator.GetFrameIndex() > 5)
                    {
                        indicator.SetFrameIndex(0);
                    }
                    timer = 0;
                }
                else
                {
                    timer += mH.GetGameTime().ElapsedGameTime.TotalSeconds;
                }

                indicator.position = new Vector2(position.X - origin.X, position.Y - origin.Y);
                indicator.Update(mH);
            }
        }

        public override void Draw(SpriteBatch sB, Vector2 displacement, ManagerHelper mH)
        {
            if (indicator != null)
            {
                indicator.Draw(sB, displacement, mH);
            }

            var test = Color.White*((IsProtected()) ? 0.5f : 1f);

            base.Draw(sB, displacement, test);
        }

        //Returns the "threat" score of a dot. Lower is more dangerous
        public override int GetThreatLevel()
        {
            return 5;
        }

        protected override void Behavior(ManagerHelper mH)
        {
            #region Shooting Code

            target = TargetDecider(mH);

            if (target != null)
            {
                //First find out what gun I want
                if (PathHelper.DistanceSquared(GetOriginPosition(), target.GetOriginPosition()) >
                    SHOTGUNRANGE*SHOTGUNRANGE)
                {
                    weaponType = 1;
                }
                else
                {
                    weaponType = 2;
                }

                //Then find out if you can actually shoot
                if (!(GrenadeDecider(mH) && grenadeCounter > grenadeSpeed) && weaponType == 1 &&
                    shootingCounter > shootingSpeed)
                {
                    shootingCounter = 0;
                    Shoot(mH);
                }
                else if (!(GrenadeDecider(mH) && grenadeCounter > grenadeSpeed) && weaponType == 2 &&
                         shootingCounter > shotgunShootingSpeed)
                {
                    shootingCounter = 0;
                    ShootShotgun(mH);
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
                else if (ShouldUseMine(mH) && grenadeCounter > grenadeSpeed)
                {
                    grenadeCounter = 0;
                    LayMine(mH);
                }
                else
                    grenadeCounter += mH.GetGameTime().ElapsedGameTime.TotalSeconds;
            }

            #endregion

            #region Power Code

            if (ShouldUsePower(mH))
                UsePower(mH);

            ChargePower();

            #endregion
        }

        protected void ShootShotgun(ManagerHelper mH)
        {
            Vector2 tempPos = PathHelper.Direction(rotation + MathHelper.PiOver2)*new Vector2(10);

            //7 Shots
            for (int s = 0; s < 7; s++)
            {
                mH.GetProjectileManager()
                  .AddProjectile(ProjectileManager.SHOTGUN, GetOriginPosition() + tempPos, this,
                                 PathHelper.Direction(rotation + (float) mH.GetRandom().NextDouble()/4 - 0.125f)*500, 18,
                                 false, true, 0.5f);
            }

            ShotgunSound(mH);
        }

        protected virtual void ShotgunSound(ManagerHelper mH)
        {
            mH.GetAudioManager().Play(SHOTGUN_SHOOT, AudioManager.RandomVolume(mH),
                                      (float) (mH.GetRandom().NextDouble()*-0.25), 0, false);
        }

        protected override void Shoot(ManagerHelper mH)
        {
            Vector2 tempPos = PathHelper.Direction(rotation + MathHelper.PiOver2)*new Vector2(10);

            mH.GetProjectileManager()
              .AddProjectile(ProjectileManager.STANDARD, GetOriginPosition() + tempPos, this,
                             PathHelper.Direction(rotation + (float) mH.GetRandom().NextDouble()/8 - 0.0625f)*400, 15,
                             false, true, 1.3f);

            ShootSound(mH);
        }

        protected virtual void ShootSound(ManagerHelper mH)
        {
            mH.GetAudioManager().Play(AudioManager.STANDARD_SHOOT, AudioManager.RandomVolume(mH),
                                      AudioManager.RandomPitch(mH), 0, false);
        }

        protected void LayMine(ManagerHelper mH)
        {
            mH.GetProjectileManager().AddMine(this);

            MineSound(mH);
        }

        protected virtual void MineSound(ManagerHelper mH)
        {
            //TODO: Does it need sound?
        }

        protected virtual void UsePower(ManagerHelper mH)
        {
            ChangePower(-1*abilityUse);
        }

        public double MaxPower()
        {
            return abilityMax;
        }

        public double CurrentPower()
        {
            return abilityCounter;
        }

        protected void ChangePower(double pC)
        {
            abilityCounter += pC;

            if (abilityCounter > abilityMax)
            {
                abilityCounter = abilityMax;
            }
            else if (abilityCounter < 0)
            {
                abilityCounter = 0;
            }
        }

        protected void ChargePower()
        {
            ChangePower(abilityCharge);
        }

        protected virtual bool ShouldUsePower(ManagerHelper mH)
        {
            //does nothing here
            return false;
        }

        protected bool ShouldUseMine(ManagerHelper mH)
        {
            if (mH.GetGametype() is CaptureTheFlag)
            {
                CaptureTheFlag tempGametype = mH.CaptureTheFlag;

                if (tempGametype.GetEnemyBase(affiliation).GetMyFlag().GetCaptor() == this)
                {
                    return true;
                }
            }
            else if (mH.GetGametype() is Assault)
            {
                Assault tempGametype = mH.Assault;

                if (tempGametype.GetAttacker() == affiliation &&
                    tempGametype.GetEnemyBase(affiliation).GetMyFlag().GetCaptor() == this)
                {
                    return true;
                }
            }
            else if (mH.GetGametype() is Conquest)
            {
                Conquest tempGametype = mH.Conquest;

                foreach (ConquestBase point in tempGametype.GetBases())
                {
                    if (point.affiliation != affiliation &&
                        PathHelper.DistanceSquared(GetOriginPosition(), point.GetOriginPosition()) < awareness*awareness)
                    {
                        return true;
                    }
                }
            }
            else if (mH.GetGametype() is Deathmatch)
            {
                Deathmatch tempGametype = mH.Deathmatch;

                foreach (Claimable claimable in tempGametype.GetClaimables())
                {
                    if (PathHelper.DistanceSquared(GetOriginPosition(), claimable.GetOriginPosition()) <
                        awareness*awareness)
                    {
                        return true;
                    }
                }
            }

            //Default
            int numEnemies = 0;

            foreach (NPC agent in mH.GetNPCManager().GetNPCs())
            {
                if (agent.GetAffiliation() != affiliation &&
                    NPCManager.IsNPCInRadius(agent, GetOriginPosition(), awareness))
                {
                    numEnemies++;
                }
            }

            return numEnemies > 2;
        }

        public virtual void UsePower()
        {
            //does nothing here
        }


        #region Paths

        protected override void NewPath(ManagerHelper mH)
        {
            if (mH.GetGametype() is Conquest)
                ConquestPath(mH);
            else if (mH.GetGametype() is CaptureTheFlag)
                CTFPath(mH);
            else if (mH.GetGametype() is Assault)
                AssaultPath(mH);
            else if (mH.GetGametype() is Deathmatch)
                DeathmatchPath(mH);
            else if (mH.GetGametype() is Survival)
            {
                SurvivalPath(mH);
            }
            else
                EngagePath(mH);
        }

        protected override void ConquestPath(ManagerHelper mH)
        {
            var temp = mH.Conquest;

            ConquestBase targetBase = null;
            float distanceToClosest = float.PositiveInfinity;

            foreach (ConquestBase conquestBase in temp.GetBases())
            {
                if (conquestBase.affiliation != affiliation)
                {
                    float distanceToBase = PathHelper.DistanceSquared(GetOriginPosition(),
                                                                      conquestBase.GetOriginPosition());

                    if (distanceToBase < distanceToClosest)
                    {
                        distanceToClosest = distanceToBase;
                        targetBase = conquestBase;
                    }
                }
            }

            if (targetBase != null)
            {
                if (PathHelper.DistanceSquared(GetOriginPosition(), targetBase.GetOriginPosition()) > 32f*32f)
                    mH.GetPathHelper().FindClearPath(GetOriginPosition(), targetBase.GetOriginPosition(), mH, path);
                else
                    HoverPath(mH, targetBase.GetOriginPosition(), 16);
            }
            else
            {
                EngagePath(mH);
            }
        }

        protected override void CTFPath(ManagerHelper mH)
        {
            var temp = mH.CaptureTheFlag;

            if (temp.GetEnemyBase(affiliation).GetMyFlag().status != Flag.FlagStatus.taken)
                mH.GetPathHelper()
                  .FindClearPath(GetOriginPosition(),
                                 temp.GetEnemyBase(affiliation).GetMyFlag().GetOriginPosition(), mH, path);
            else
            {
                NPC captor = temp.GetEnemyBase(affiliation).GetMyFlag().GetCaptor();

                if (captor == this)
                    mH.GetPathHelper()
                      .FindClearPath(GetOriginPosition(), temp.GetAllyBase(affiliation).GetOriginPosition(), mH, path);
                else
                    mH.GetPathHelper().FindClearPath(GetOriginPosition(), captor.GetOriginPosition(), mH, path);
            }
        }

        protected override void AssaultPath(ManagerHelper mH)
        {
            var temp = mH.Assault;

            if (temp.GetAttacker() == affiliation)
            {
                if (temp.GetEnemyBase(affiliation).GetMyFlag().status != Flag.FlagStatus.taken)
                    mH.GetPathHelper()
                      .FindClearPath(GetOriginPosition(), temp.GetEnemyBase(affiliation).GetOriginPosition(), mH, path);
                else
                {
                    NPC captor = temp.GetEnemyBase(affiliation).GetMyFlag().GetCaptor();

                    NPC closestEnemy = null;
                    float closestDistanceToEnemy = float.PositiveInfinity;

                    foreach (var team in mH.GetGametype().GetTeams())
                    {
                        if (team != affiliation)
                        {
                            NPC closestEnemyForTeam =
                                mH.GetNPCManager().GetClosestInList(mH.GetNPCManager().GetAllies(team), captor);

                            if (closestEnemyForTeam != null)
                            {
                                float closestDistanceToEnemyForTeam =
                                    PathHelper.DistanceSquared(closestEnemyForTeam.GetOriginPosition(),
                                                               captor.GetOriginPosition());

                                if (closestDistanceToEnemyForTeam < closestDistanceToEnemy)
                                {
                                    closestDistanceToEnemy = closestDistanceToEnemyForTeam;
                                    closestEnemy = closestEnemyForTeam;
                                }
                            }
                        }
                    }

                    if (captor == this)
                    {
                        mH.GetPathHelper()
                          .FindClearPath(GetOriginPosition(), temp.GetAllyBase(affiliation).GetOriginPosition(),
                                         mH, path);
                    }
                    else if (closestEnemy != null)
                    {
                        mH.GetPathHelper()
                          .FindClearPath(GetOriginPosition(), closestEnemy.GetOriginPosition(), mH, path);
                    }
                    else
                    {
                        mH.GetPathHelper().FindClearPath(GetOriginPosition(), captor.GetOriginPosition(), mH, path);
                    }
                }
            }

            else
            {
                if (temp.GetAllyBase(affiliation).GetMyFlag().status == Flag.FlagStatus.home)
                    HoverPath(mH, temp.GetAllyBase(affiliation).GetOriginPosition(), 48);
                else
                {
                    NPC captor = temp.GetAllyBase(affiliation).GetMyFlag().GetCaptor();

                    if (captor != null)
                        mH.GetPathHelper().FindClearPath(GetOriginPosition(), captor.GetOriginPosition(), mH, path);
                    else
                        mH.GetPathHelper()
                          .FindClearPath(GetOriginPosition(),
                                         temp.GetAllyBase(affiliation).GetMyFlag().GetOriginPosition(), mH, path);
                }
            }
        }

        protected override void DeathmatchPath(ManagerHelper mH)
        {
            var temp = mH.Deathmatch;
            Claimable c = temp.GetClosestClaimable(GetOriginPosition(), mH);

            if (c != null && temp.GetPopCap() > mH.GetNPCManager().GetAllies(affiliation).Count)
                mH.GetPathHelper().FindClearPath(GetOriginPosition(), c.GetOriginPosition(), mH, path);
            else
                EngagePath(mH);
        }

        protected override void SurvivalPath(ManagerHelper mH)
        {
            pathTimerEnd = .5;

            var temp = mH.Survival;
            Claimable c = temp.GetClosestClaimable(GetOriginPosition());
            NPC enemy =
                mH.GetNPCManager()
                  .GetClosestInList(mH.GetNPCManager().GetAllies(AffliationTypes.black),
                                    GetOriginPosition());

            if (enemy != null && PathHelper.DistanceSquared(GetOriginPosition(), enemy.GetOriginPosition()) < 200*200)
            {
                mH.GetPathHelper().FindEscapePath(GetOriginPosition(), enemy.GetOriginPosition(), 400, mH, 200, path);
            }
            else if (c != null && temp.GetPopCap() > mH.GetNPCManager().GetAllies(affiliation).Count)
            {
                mH.GetPathHelper().FindClearPath(GetOriginPosition(), c.GetOriginPosition(), mH, path);
            }
            else
            {
                RandomPath(mH);
            }
        }

        #endregion
    }
}