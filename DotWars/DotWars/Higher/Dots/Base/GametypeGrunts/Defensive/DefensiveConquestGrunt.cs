﻿#region

using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    public class DefensiveConquestGrunt : Grunt
    {
        public DefensiveConquestGrunt(string a, Vector2 p)
            : base(a, p)
        {
        }

        protected override void SpecialPath(ManagerHelper mH)
        {
            var temp = (Conquest) mH.GetGametype();
            ConquestBase forwardBase = temp.GetForwardBase(affiliation, mH);

            if (forwardBase != null)
            {
                mH.GetPathHelper().FindClearPath(GetOriginPosition(), forwardBase.GetOriginPosition(), mH, path);
            }
            else
            {
                EngagePath(mH);
            }
        }
    }
}