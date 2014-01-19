#region

using System;
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
            foreach (var agent in mH.GetNPCManager().GetNPCs())
            {
                switch (status)
                {
                    case FlagStatus.home:
                        if (agent.GetAffiliation() != affiliation &&
                            (agent is Commander || agent is Grunt) &&
                            CollisionHelper.IntersectPixelsSimple(agent, this) != CollisionHelper.NO_COLLIDE)
                        {
                            captor = agent;
                            status = FlagStatus.taken;
                        }
                        break;
                    case FlagStatus.away:
                        if (CollisionHelper.IntersectPixelsSimple(agent, this) != CollisionHelper.NO_COLLIDE)
                        {
                            if (agent.GetAffiliation() == affiliation)
                            {
                                if (agent is Commander)
                                {
                                    mH.GetGametype().UpdateFlagsReturned(agent.GetPersonalAffilation());
                                }

                                position = homePosition - origin;
                                status = FlagStatus.home;
                            }
                            else if (agent is Commander || agent is Grunt)
                            {
                                captor = agent;
                                status = FlagStatus.taken;
                            }
                        }
                        break;
                }
            }

            if (status == FlagStatus.taken)
            {
                if (captor.GetHealth() > 0)
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
                        CollisionHelper.IntersectPixelsRadius(this, b, 32f, 32f) != CollisionHelper.NO_COLLIDE)
                    {
                        status = FlagStatus.home;
                        position = homePosition - origin;
                        temp.ChangeScore(GetCaptor(), 1);
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
                        CollisionHelper.IntersectPixelsRadius(this, b, 32f, 32f) != CollisionHelper.NO_COLLIDE)
                    {
                        status = FlagStatus.home;
                        position = homePosition - origin;
                        temp.ChangeScore(GetCaptor(), 1);
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