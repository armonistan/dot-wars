#region

using System;
using System.Collections.Generic;

#endregion

namespace DotWars
{
    public class Assassins : Gametype
    {
        #region Declarations

        #endregion

        public Assassins(List<NPC.AffliationTypes> tL, Dictionary<Type, NPC.AffliationTypes> pL, float sT)
            : base(tL, pL, 10, 0, sT)
        {
            this.typeOfGame = GT.ASSASSINS;

            gameEndTimer = 180;
        }

        public override bool Update(ManagerHelper mH)
        {
            return base.Update(mH);
        }

        public override string GetGametypeStatistics()
        {
            return "Daniel is Silly XD";
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