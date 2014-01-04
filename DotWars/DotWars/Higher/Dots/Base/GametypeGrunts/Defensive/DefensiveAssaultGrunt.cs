﻿using Microsoft.Xna.Framework;

namespace DotWars
{
    public class DefensiveAssaultGrunt : Grunt
    {
        public DefensiveAssaultGrunt(string a, Vector2 p)
            : base(a, p)
        {
        }

        protected override void NewPath(ManagerHelper mH)
        {
            if (mH.GetProjectileManager().GetFlare(affiliation) != null)
                FlarePath(mH);
            else
                SpecialPath(mH);
        }

        protected override void SpecialPath(ManagerHelper mH)
        {
            var temp = (Assault) mH.GetGametype();

            if (temp.GetAllyBase(affiliation).GetMyFlag().status == Flag.FlagStatus.home)
                DefensePath(mH, temp.GetAllyBase(affiliation).GetOriginPosition());
            else
            {
                NPC captor = temp.GetAllyBase(affiliation).GetMyFlag().GetCaptor();

                if (captor != null)
                    mH.GetPathHelper().FindClearPath(GetOriginPosition(), captor.GetOriginPosition(), mH, path);
                else
                    mH.GetPathHelper()
                             .FindClearPath(GetOriginPosition(),
                                            temp.GetAllyBase(affiliation).GetMyFlag().GetOriginPosition(), mH, path);
            }
        }
    }
}