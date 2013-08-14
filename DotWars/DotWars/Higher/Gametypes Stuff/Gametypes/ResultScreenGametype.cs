using System;
using System.Collections.Generic;

namespace DotWars
{
    public class ResultScreenGametype : Gametype
    {
        public ResultScreenGametype()
            : base(new List<NPC.AffliationTypes>(), new Dictionary<Type, NPC.AffliationTypes>(), 1, 0, 0)
        {
            teams.Add(NPC.AffliationTypes.grey);
            scores = new int[1];
        }

        public override bool Update(ManagerHelper mH)
        {
            return false;
        }
    }
}