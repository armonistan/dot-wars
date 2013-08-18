using System;
using System.Collections.Generic;

namespace DotWars
{
    public class Assasssins : Gametype
    {
        #region Declarations

        #endregion

        public Assasssins(List<NPC.AffliationTypes> tL, Dictionary<Type, NPC.AffliationTypes> pL, float sT)
            : base(tL, pL, 10, 0, sT)
        {
            this.typeOfGame = GT.ASSASSINS;
        }

        public override bool Update(ManagerHelper mH)
        {
            return base.Update(mH);
        }

        public override string GetGametypeStatistics()
        {
            return "Daniel Sucks";
        }

        public override string GetName()
        {
            return "Assassins";
        }

        public override string GetSummary()
        {
            return "First commander to 10\nkills is the winner!";
        }
    }
}