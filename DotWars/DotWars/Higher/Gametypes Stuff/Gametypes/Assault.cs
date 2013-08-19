using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DotWars
{
    public class Assault : Gametype
    {
        #region Declarations

        private readonly NPC.AffliationTypes attacker;
        private readonly NPC.AffliationTypes defender;
        private List<AssaultBase> bases;

        #endregion

        public Assault(List<NPC.AffliationTypes> tL, Dictionary<Type, NPC.AffliationTypes> pL, int pC,
                       NPC.AffliationTypes att, NPC.AffliationTypes def, float sT)
            : base(tL, pL, 3, pC, sT)
        {
            this.typeOfGame = GT.ASSAULT;
            //set up defender and attacker
            attacker = att;
            defender = def;

            //set up the bases
        }

        public void Initialize(ManagerHelper mH, List<AssaultBase> bL)
        {
            bases = bL;

            foreach (AssaultBase b in bases)
            {
                b.LoadContent(mH.GetTextureManager());
            }
        }

        public override bool Update(ManagerHelper mH)
        {
            foreach (AssaultBase b in bases)
            {
                b.Update(mH);
            }

            return base.Update(mH);
        }

        public override void DrawBottom(SpriteBatch sB, Vector2 d)
        {
            foreach (AssaultBase b in bases)
            {
                b.Draw(sB, d, managers);
            }

            base.DrawBottom(sB, d);
        }

        public override void DrawTop(SpriteBatch sB, Vector2 d)
        {
            base.DrawTop(sB, d);

            foreach (AssaultBase b in bases)
            {
                b.DrawFlag(sB, d, managers);
            }
        }

        public override void ChangeScore(NPC agent, int s)
        {
            if (agent is Commander)
                flagsCaptured[agent.GetPersonalAffilation()]++;

            base.ChangeScore(agent, s);
        }

        public List<AssaultBase> GetBases()
        {
            return bases;
        }

        public AssaultBase GetAllyBase(NPC.AffliationTypes a)
        {
            foreach (AssaultBase fB in bases)
            {
                if (fB.affiliation == a)
                {
                    return fB;
                }
            }

            //did not find desired base
            return null;
        }

        public AssaultBase GetEnemyBase(NPC.AffliationTypes a)
        {
            foreach (AssaultBase fB in bases)
            {
                if (fB.affiliation != a)
                {
                    return fB;
                }
            }

            //did not find desired base
            return null;
        }

        public NPC.AffliationTypes GetAttacker()
        {
            return attacker;
        }

        public NPC.AffliationTypes GetDefender()
        {
            return defender;
        }

        public override string GetGametypeStatistics()
        {
            return "Daniel is Silly XD";
        }

        public override string GetName()
        {
            return "Assault";
        }

        public override string GetSummary()
        {
            return "One team defends their flag\nwhile the other team tries to take it!";
        }
    }
}