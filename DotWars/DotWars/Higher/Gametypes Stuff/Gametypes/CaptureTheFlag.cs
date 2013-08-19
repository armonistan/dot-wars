using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DotWars
{
    public class CaptureTheFlag : Gametype
    {
        #region Declarations

        private List<CTFBase> bases;

        #endregion

        public CaptureTheFlag(List<NPC.AffliationTypes> tL, Dictionary<Type, NPC.AffliationTypes> pL, int pC, float sT)
            : base(tL, pL, 3, pC, sT)
        {
            this.typeOfGame = GT.CTF;
        }

        public void Initialize(ManagerHelper mH, List<CTFBase> bL)
        {
            bases = bL;

            foreach (CTFBase b in bases)
            {
                b.LoadContent(mH.GetTextureManager());
            }
        }

        public override bool Update(ManagerHelper mH)
        {
            foreach (CTFBase b in bases)
            {
                b.Update(mH);
            }

            foreach (NPC.AffliationTypes a in teams)
            {
                if (GetAllyBase(a).GetMyFlag() != null && GetAllyBase(a).GetMyFlag().status != Flag.FlagStatus.home)
                    UpdateTimeFlagAway(a);
                else
                {
                    UpdateMostTimeFlagAway(a);
                    timeFlagAway[a] = 0;
                }
            }

            return base.Update(mH);
        }

        public override void DrawBottom(SpriteBatch sB, Vector2 d)
        {
            foreach (CTFBase b in bases)
            {
                b.Draw(sB, d, managers);
            }

            base.DrawBottom(sB, d);
        }

        public override void DrawTop(SpriteBatch sB, Vector2 d)
        {
            foreach (CTFBase b in bases)
            {
                b.DrawFlag(sB, d, managers);
            }

            base.DrawTop(sB, d);
        }

        public List<CTFBase> GetBases()
        {
            return bases;
        }

        public CTFBase GetAllyBase(NPC.AffliationTypes a)
        {
            foreach (CTFBase fB in bases)
            {
                if (fB.affiliation == a)
                {
                    return fB;
                }
            }

            //did not find desired base
            return null;
        }

        public CTFBase GetEnemyBase(NPC.AffliationTypes a)
        {
            foreach (CTFBase fB in bases)
            {
                if (fB.affiliation != a)
                {
                    return fB;
                }
            }

            //did not find desired base
            return null;
        }

        public override string GetGametypeStatistics()
        {
            return "Daniel is Silly XD";
        }

        public override string GetName()
        {
            return "Capture the Flag";
        }

        public override string GetSummary()
        {
            return "The first team to capture\n3 flags is the winner!";
        }
        
        public override void ChangeScore(NPC agent, int s)
        {
            if (agent is Commander)
                flagsCaptured[agent.GetPersonalAffilation()]++;

            UpdateQuickestSuccessfulCapture(agent.GetAffiliation());

            base.ChangeScore(agent, s);
        }
    }
}