using Microsoft.Xna.Framework;

namespace DotWars
{
    public class DefensiveAssaultGrunt : Grunt
    {
        public DefensiveAssaultGrunt(string a, Vector2 p)
            : base(a, p)
        {
        }

        protected override Path NewPath(ManagerHelper mH)
        {
            if (mH.GetProjectileManager().GetFlare(affiliation) != null)
                return FlarePath(mH);
            else
                return SpecialPath(mH);
        }

        protected override Path SpecialPath(ManagerHelper mH)
        {
            var temp = (Assault) mH.GetGametype();

            if (temp.GetAllyBase(affiliation).GetMyFlag().status == Flag.FlagStatus.home)
                return DefensePath(mH, temp.GetAllyBase(affiliation).GetOriginPosition());
            else
            {
                NPC captor = temp.GetAllyBase(affiliation).GetMyFlag().GetCaptor();

                if (captor != null)
                    return mH.GetPathHelper().FindClearPath(GetOriginPosition(), captor.GetOriginPosition(), mH);
                else
                    return mH.GetPathHelper()
                             .FindClearPath(GetOriginPosition(),
                                            temp.GetAllyBase(affiliation).GetMyFlag().GetOriginPosition(), mH);
            }
        }
    }
}