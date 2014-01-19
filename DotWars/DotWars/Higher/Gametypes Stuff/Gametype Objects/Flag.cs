#region

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace DotWars
{
    public class Flag : Sprite
    {
        public enum FlagStatus
        {
            home,
            taken,
            away
        }

        #region Declarations

        private readonly double endTime;
        private readonly Vector2 homePosition;
        protected NPC.AffliationTypes affiliation;

        private NPC captor;
        public FlagStatus status;

        //Animation Stuff
        private double timer;

        #endregion

        public Flag(string a, Vector2 p, NPC.AffliationTypes aT)
            : base(a, p)
        {
            affiliation = aT;
            status = FlagStatus.home;
            homePosition = p;

            //Animations
            timer = 0;
            endTime = 0.1f;
        }

        public override void Update(ManagerHelper mH)
        {
            foreach (NPC a in mH.GetNPCManager().GetNPCs())
            {
                if (CollisionHelper.IntersectPixelsSimple(a, this) != CollisionHelper.NO_COLLIDE)
                {
                    if (status == FlagStatus.home)
                    {
                        if (a.GetAffiliation() != affiliation &&
                            (a is Commander || a is OffensiveAssaultGrunt || a is OffensiveCTFGrunt))
                        {
                            captor = a;
                            status = FlagStatus.taken;
                        }
                    }
                    else if (status == FlagStatus.away)
                    {
                        if (a.GetAffiliation() == affiliation)
                        {
                            if (a is Commander)
                                mH.GetGametype().UpdateFlagsReturned(a.GetPersonalAffilation());

                            position = homePosition - origin;
                            status = FlagStatus.home;
                        }
                        else if (a.GetAffiliation() != affiliation &&
                                 (a is Commander || a is OffensiveAssaultGrunt || a is OffensiveCTFGrunt))
                        {
                            status = FlagStatus.taken;
                            captor = a;
                        }
                    }
                }
            }

            if (status == FlagStatus.taken)
            {
                if (mH.GetNPCManager().DoesNPCExist(captor))
                {
                    position = captor.position;
                }
                else
                {
                    captor = null;
                    status = FlagStatus.away;
                }
            }

            if (mH.GetGametype() is CaptureTheFlag)
            {
                var temp = (CaptureTheFlag) mH.GetGametype();

                foreach (CTFBase b in temp.GetBases())
                {
                    if (b.affiliation != affiliation &&
                        CollisionHelper.IntersectPixelsRadius(this, b, 32, 32) != CollisionHelper.NO_COLLIDE)
                    {
                        status = FlagStatus.home;
                        position = homePosition - origin;
                        temp.ChangeScore(this.GetCaptor(), 1);
                        captor = null;
                    }
                }
            }

            else if (mH.GetGametype() is Assault)
            {
                var temp = (Assault) mH.GetGametype();

                foreach (AssaultBase b in temp.GetBases())
                {
                    if (b.affiliation != affiliation &&
                        CollisionHelper.IntersectPixelsRadius(this, b, 32, 32) != CollisionHelper.NO_COLLIDE)
                    {
                        status = FlagStatus.home;
                        position = homePosition - origin;
                        temp.ChangeScore(this.GetCaptor(), 1);
                        captor = null;
                    }
                }
            }

            //Animations
            if (status == FlagStatus.taken)
            {
                if (timer > endTime)
                {
                    if (frameIndex == totalFrames - 1)
                    {
                        frameIndex = 0;
                    }
                    else
                    {
                        frameIndex++;
                    }
                    timer = 0;
                }
                else
                {
                    timer += mH.GetGameTime().ElapsedGameTime.TotalSeconds;
                }
            }

            base.Update(mH);
        }

        public NPC GetCaptor()
        {
            return captor;
        }

        public override void Draw(SpriteBatch sB, Vector2 d, ManagerHelper mH)
        {
            base.Draw(sB, d, mH);
        }
    }
}