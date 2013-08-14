using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DotWars
{
    public class Deathmatch : Gametype
    {
        #region Declarations

        public List<Claimable> claimables;
        public Dictionary<NPC.AffliationTypes, int> commanderKills;
        #endregion

        public Deathmatch(List<NPC.AffliationTypes> tL, Dictionary<Type, NPC.AffliationTypes> pL, int pC, float sT)
            : base(tL, pL, 50, pC, sT)
        {
            this.typeOfGame = GT.DEATHMATCH;
            commanderKills = new Dictionary<NPC.AffliationTypes, int>();
        }

        public void Initialize(ManagerHelper mH, List<Claimable> cL)
        {
            claimables = cL;

            foreach (Claimable c in claimables)
            {
                c.LoadContent(mH.GetTextureManager());
            }
        }

        public override bool Update(ManagerHelper mH)
        {
            foreach (Claimable c in claimables)
            {
                c.Update(mH);
            }

            return base.Update(mH);
        }

        public override void DrawBottom(SpriteBatch sB, Vector2 d)
        {
            foreach (Claimable c in claimables)
            {
                c.Draw(sB, d, managers);
            }

            base.DrawBottom(sB, d);
        }

        public List<Claimable> GetClaimables()
        {
            return claimables;
        }

        public Claimable GetClosestClaimable(Vector2 p, ManagerHelper mH)
        {
            if (claimables.Count > 0)
            {
                Claimable closest = claimables.First();
                float minDist = PathHelper.Distance(p, closest.GetOriginPosition());

                foreach (Claimable a in claimables)
                {
                    if (!a.taken)
                    {
                        float aDist = PathHelper.Distance(p, a.GetOriginPosition());
                        if (aDist < minDist)
                        {
                            closest = a;
                            minDist = aDist;
                        }
                    }
                }

                if (closest.taken)
                {
                    return null;
                }
                else
                {
                    return closest;
                }
            }

            return null;
        }

        public override string GetGametypeStatistics()
        {
            return "Daniel Sucks";
        }

        public override string GetName()
        {
            return "Deathmatch";
        }

        public override string GetSummary()
        {
            return "First team to " + winScore + " kills\nis the winner!";
        }
    }
}