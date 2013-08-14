using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DotWars
{
    public class Commander : NPC
    {
        #region Declarations

        //Variables unique to the commander

        //Constants unique to the commander
        public const int MAXNUMFLARES = 1;
        public const int MAXTEAMNUM = 4;
        public const float SHOTGUNRANGE = 128;
        protected double abilityCounter;
        protected double abilityMax;
        protected float abilityPercent;
        public int commanderRadius;

        //Indicator stuff
        protected float endtime;
        public int flareCount;
        public int grenadeType;
        protected Sprite indicator;
        public double shotgunShootingSpeed;
        protected float timer;
        public int weaponType; //0 is the machine gun, 1 is shot gun
        private PlayerIndex xboxIndex;

        #endregion

        public Commander(String aN, Vector2 p, double aS)
            : base(aN, p)
        {
            health = 225; // Large amount of health, a health commander can take 3 sniper hits
            maxHealth = health; //The units starting health will always be his max health
            movementSpeed = 135; //Above average movement speed. He is an elite, no?
            shootingSpeed = .3; //Above average shooting speed for machine gun in seconds
            shotgunShootingSpeed = 1.5; //Average shooting speed for shotgun
            weaponType = 0; //machine gun default
            grenadeType = 0;

            commanderRadius = 50;
                //Not bad...huh? Daniel can't sing at all. None of the woodwinds or the brass could play

            flareCount = 0; //Number of flares active

            grenadeCounter = 3; //default
            grenadeSpeed = 3; //Three second recharge

            //Ability Data
            abilityMax = abilityCounter = aS;
            abilityPercent = 0.5f;

            awareness = 500;

            affiliation = AffliationTypes.red;

            //Set up indicator stuff
            timer = 0;
            endtime = 0.07f;
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
                    indicator.SetFrameIndex(indicator.GetFrameIndex()+1);

                    if (indicator.GetFrameIndex() > 5)
                    {
                        indicator.SetFrameIndex(0);
                    }
                    timer = 0;
                }
                else
                {
                    timer += (float) mH.GetGameTime().ElapsedGameTime.TotalSeconds;
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

            base.Draw(sB, displacement, mH);
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
                if (PathHelper.Distance(GetOriginPosition(), target.GetOriginPosition()) > SHOTGUNRANGE)
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
                else
                    grenadeCounter += mH.GetGameTime().ElapsedGameTime.TotalSeconds;
            }

            #endregion

            #region Flare Code

            if (ShouldTossFlare(mH))
                TossFlare(mH); //toss flare

            #endregion

            #region Power Code

            if (ShouldUsePower(mH))
                UsePower(mH);
            ChargePower();

            #endregion
        }

        protected void ShootShotgun(ManagerHelper mH)
        {
            Vector2 tempPos = PathHelper.Direction(rotation + (float) (Math.PI/2))*new Vector2(10);

            mH.GetProjectileManager()
              .AddProjectile("Projectiles/bullet_shotgun", GetOriginPosition() + tempPos, this,
                             PathHelper.Direction(rotation + (float) mH.GetRandom().NextDouble()/4 - 0.125f)*500, 15,
                             false, 1);
            mH.GetProjectileManager()
              .AddProjectile("Projectiles/bullet_shotgun", GetOriginPosition() + tempPos, this,
                             PathHelper.Direction(rotation + (float) mH.GetRandom().NextDouble()/4 - 0.125f)*500, 15,
                             false, 1);
            mH.GetProjectileManager()
              .AddProjectile("Projectiles/bullet_shotgun", GetOriginPosition() + tempPos, this,
                             PathHelper.Direction(rotation + (float) mH.GetRandom().NextDouble()/4 - 0.125f)*500, 15,
                             false, 1);
            mH.GetProjectileManager()
              .AddProjectile("Projectiles/bullet_shotgun", GetOriginPosition() + tempPos, this,
                             PathHelper.Direction(rotation + (float) mH.GetRandom().NextDouble()/4 - 0.125f)*500, 15,
                             false, 1);
            mH.GetProjectileManager()
              .AddProjectile("Projectiles/bullet_shotgun", GetOriginPosition() + tempPos, this,
                             PathHelper.Direction(rotation + (float) mH.GetRandom().NextDouble()/4 - 0.125f)*500, 15,
                             false, 1);
            mH.GetProjectileManager()
              .AddProjectile("Projectiles/bullet_shotgun", GetOriginPosition() + tempPos, this,
                             PathHelper.Direction(rotation + (float) mH.GetRandom().NextDouble()/4 - 0.125f)*500, 15,
                             false, 1);

            ShotgunSound(mH);
        }

        protected virtual void ShotgunSound(ManagerHelper mH)
        {
            mH.GetAudioManager().Play("shotgunShoot", AudioManager.RandomVolume(mH),
                (float)(mH.GetRandom().NextDouble() * -0.25), 0, false);
        }

        protected override void Shoot(ManagerHelper mH)
        {
            Vector2 tempPos = PathHelper.Direction(rotation + (float)(Math.PI / 2)) * new Vector2(10);

            mH.GetProjectileManager()
              .AddProjectile("Projectiles/bullet_standard", GetOriginPosition() + tempPos, this,
                             PathHelper.Direction(rotation + (float)mH.GetRandom().NextDouble() / 8 - 0.0625f) * 300, 25,
                             false, 5);

            ShootSound(mH);
        }

        protected virtual void ShootSound(ManagerHelper mH)
        {
            mH.GetAudioManager().Play("standardShoot", AudioManager.RandomVolume(mH),
                AudioManager.RandomPitch(mH), 0, false);
        }

        protected void TossFlare(ManagerHelper mH)
        {
            mH.GetProjectileManager().AddFlare(this, PathHelper.Direction(rotation)*300);

            FlareSound(mH);
        }

        protected virtual void FlareSound(ManagerHelper mH)
        {
            mH.GetAudioManager().Play("flareShoot", AudioManager.RandomVolume(mH),
                   AudioManager.RandomPitch(mH), 0, false);
        }

        private bool ShouldTossFlare(ManagerHelper mH)
        {
            Gametype tempGameType = mH.GetGametype();

            if (mH.GetProjectileManager().GetFlare(affiliation) == null)
            {
                #region General Combat Emergency

                bool hasAllies = (mH.GetNPCManager().GetAlliesInRadius(affiliation, GetOriginPosition(), 250).Count > 3);
                bool isThreat = ((target != null) &&
                                 mH.GetNPCManager()
                                   .GetAlliesInRadius(target.GetAffiliation(), target.GetOriginPosition(), target.GetAwareness())
                                   .Count > 2 || target is Commander);

                if (hasAllies && isThreat)
                    return true;

                #endregion

                #region Gametype Occurances    

                if (tempGameType is Assasssins)
                    return false;

                else if (tempGameType is Conquest)
                {
                    var temp = (Conquest) tempGameType;
                    hasAllies = (mH.GetNPCManager().GetAlliesInRadius(affiliation, GetOriginPosition(), 250).Count > 2);
                    bool isNearBase =
                        (PathHelper.Distance(GetOriginPosition(),
                                             temp.GetClosestInList(temp.GetEnemyBases(affiliation), GetOriginPosition())
                                                 .GetOriginPosition()) < 128);

                    return (hasAllies && isNearBase);
                }

                else if (tempGameType is CaptureTheFlag)
                {
                    var temp = (CaptureTheFlag) tempGameType;

                    if (tempGameType is CaptureTheFlag)
                    {
                        hasAllies = (mH.GetNPCManager().GetAlliesInRadius(affiliation, GetOriginPosition(), 250).Count >
                                     2);
                        bool isSucidal =
                            (mH.GetNPCManager()
                               .GetAllButAlliesInRadius(affiliation,
                                                        temp.GetEnemyBase(affiliation).GetMyFlag().GetOriginPosition(),
                                                        128)
                               .Count > 2);

                        return (hasAllies && isSucidal);
                    }
                }

                else if (tempGameType is Assault)
                {
                    var temp = (Assault) tempGameType;

                    if (temp.GetAttacker() == affiliation)
                    {
                        hasAllies = (mH.GetNPCManager().GetAlliesInRadius(affiliation, GetOriginPosition(), 250).Count >
                                     2);
                        bool isSucidal =
                            (mH.GetNPCManager()
                               .GetAllButAlliesInRadius(affiliation,
                                                        temp.GetEnemyBase(affiliation).GetMyFlag().GetOriginPosition(),
                                                        128)
                               .Count > 2);

                        return (hasAllies && isSucidal);
                    }

                    else
                    {
                        hasAllies = (mH.GetNPCManager().GetAlliesInRadius(affiliation, GetOriginPosition(), 250).Count >
                                     2);
                        isThreat = ((target != null) &&
                                    mH.GetNPCManager()
                                      .GetAlliesInRadius(target.GetAffiliation(), target.GetOriginPosition(),
                                                         target.GetAwareness())
                                      .Count > 2 || target is Commander);

                        return (hasAllies && isThreat);
                    }
                }

                #endregion
            }

            return false;
        }

        protected virtual void UsePower(ManagerHelper mH)
        {
            ChangePower(-1*MaxPower()*abilityPercent);
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
            ChangePower(0.01);
        }

        public virtual bool ShouldUsePower(ManagerHelper mH)
        {
            //does nothing here
            return false;
        }

        public virtual void UsePower()
        {
            //does nothing here
        }

        #region Paths

        protected override Path NewPath(ManagerHelper mH)
        {
            if (mH.GetGametype() is Conquest)
                return ConquestPath(mH);
            else if (mH.GetGametype() is CaptureTheFlag)
                return CTFPath(mH);
            else if (mH.GetGametype() is Assault)
                return AssaultPath(mH);
            else if (mH.GetGametype() is Deathmatch)
                return DeathmatchPath(mH);
            else if (mH.GetGametype() is Survival)
            {
                pathTimerEnd = 0.1f;
                return SurvivalPath(mH);
            }
            else
                return EngagePath(mH);
        }

        protected override Path ConquestPath(ManagerHelper mH)
        {
            var temp = (Conquest) mH.GetGametype();
            ConquestBase targetBase = temp.GetClosestInList(temp.GetEnemyBases(affiliation), GetOriginPosition());

            if (targetBase != null)
            {
                if (PathHelper.Distance(GetOriginPosition(), targetBase.GetOriginPosition()) > 32)
                    return mH.GetPathHelper().FindClearPath(GetOriginPosition(), targetBase.GetOriginPosition(), mH);

                else
                    return HoverPath(mH, targetBase.GetOriginPosition(), 16);
            }

            else
            {
                return base.NewPath(mH);
            }
        }

        protected override Path CTFPath(ManagerHelper mH)
        {
            var temp = (CaptureTheFlag) mH.GetGametype();

            if (temp.GetEnemyBase(affiliation).GetMyFlag().status != Flag.FlagStatus.taken)
                return mH.GetPathHelper()
                         .FindClearPath(GetOriginPosition(),
                                        temp.GetEnemyBase(affiliation).GetMyFlag().GetOriginPosition(), mH);
            else
            {
                NPC captor = temp.GetEnemyBase(affiliation).GetMyFlag().GetCaptor();

                if (captor == this)
                    return mH.GetPathHelper()
                             .FindClearPath(GetOriginPosition(), temp.GetAllyBase(affiliation).GetOriginPosition(), mH);
                else
                    return mH.GetPathHelper().FindClearPath(GetOriginPosition(), captor.GetOriginPosition(), mH);
            }
        }

        protected override Path AssaultPath(ManagerHelper mH)
        {
            var temp = (Assault) mH.GetGametype();

            if (temp.GetAttacker() == affiliation)
            {
                if (temp.GetEnemyBase(affiliation).GetMyFlag().status != Flag.FlagStatus.taken)
                    return mH.GetPathHelper()
                             .FindClearPath(GetOriginPosition(), temp.GetEnemyBase(affiliation).GetOriginPosition(), mH);
                else
                {
                    NPC captor = temp.GetEnemyBase(affiliation).GetMyFlag().GetCaptor();

                    if (captor == this)
                        return mH.GetPathHelper()
                                 .FindClearPath(GetOriginPosition(), temp.GetAllyBase(affiliation).GetOriginPosition(),
                                                mH);
                    else if (
                        mH.GetNPCManager().GetClosestInList(mH.GetNPCManager().GetAllButAllies(affiliation), captor) !=
                        null)
                        return mH.GetPathHelper()
                                 .FindClearPath(GetOriginPosition(),
                                                mH.GetNPCManager()
                                                  .GetClosestInList(
                                                      mH.GetNPCManager().GetAllButAllies(affiliation), captor)
                                                  .GetOriginPosition(), mH);
                    else
                        return mH.GetPathHelper().FindClearPath(GetOriginPosition(), captor.GetOriginPosition(), mH);
                }
            }

            else
            {
                if (temp.GetAllyBase(affiliation).GetMyFlag().status == Flag.FlagStatus.home)
                    return HoverPath(mH, temp.GetAllyBase(affiliation).GetOriginPosition(), 48);
                else
                {
                    NPC captor = temp.GetAllyBase(affiliation).GetMyFlag().GetCaptor();

                    if (captor != null)
                        return mH.GetPathHelper().FindClearPath(GetOriginPosition(), captor.GetOriginPosition(), mH);
                    else
                        return mH.GetPathHelper()
                                 .FindClearPath(GetOriginPosition(),
                                                temp.GetAllyBase(affiliation).GetMyFlag().GetOriginPosition(), mH);
                }
            }
        }

        protected override Path DeathmatchPath(ManagerHelper mH)
        {
            var temp = (Deathmatch) mH.GetGametype();
            Claimable c = temp.GetClosestClaimable(GetOriginPosition(), mH);

            if (c != null && temp.GetPopCap() > mH.GetNPCManager().GetAllies(affiliation).Count)
                return mH.GetPathHelper().FindClearPath(GetOriginPosition(), c.GetOriginPosition(), mH);
            else
                return EngagePath(mH);
        }

        protected override Path SurvivalPath(ManagerHelper mH)
        {
            pathTimerEnd = .5f;

            var temp = (Survival) mH.GetGametype();
            Claimable c = temp.GetClosestClaimable(GetOriginPosition());
            NPC fucker =
                mH.GetNPCManager()
                  .GetClosestInList(mH.GetNPCManager().GetAlliesInSight(AffliationTypes.black, this),
                                    GetOriginPosition());

            if (fucker != null && PathHelper.Distance(GetOriginPosition(), fucker.GetOriginPosition()) < 200)
            {
                return mH.GetPathHelper().FindEscapePath(GetOriginPosition(), fucker.GetOriginPosition(), 400, mH, 200);
            }
            else if (c != null && temp.GetPopCap() > mH.GetNPCManager().GetAllies(affiliation).Count)
            {
                return mH.GetPathHelper().FindClearPath(GetOriginPosition(), c.GetOriginPosition(), mH);
            }
            else
            {
                return RandomPath(mH);
            }
        }

        #endregion
    }
}