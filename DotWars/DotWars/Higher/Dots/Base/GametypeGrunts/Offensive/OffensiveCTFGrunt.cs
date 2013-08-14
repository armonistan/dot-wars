using Microsoft.Xna.Framework;

namespace DotWars
{
    public class OffensiveCTFGrunt : Grunt
    {
        public OffensiveCTFGrunt(string a, Vector2 p)
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
            var temp = (CaptureTheFlag) mH.GetGametype();

            if (temp.GetEnemyBase(affiliation).GetMyFlag().status != Flag.FlagStatus.taken)
                return mH.GetPathHelper()
                         .FindClearPath(GetOriginPosition(),
                                        temp.GetEnemyBase(affiliation).GetMyFlag().GetOriginPosition(), mH);
            else
            {
                NPC captor = temp.GetEnemyBase(affiliation).GetMyFlag().GetCaptor();

                if (captor == this)
                    return mH.GetPathHelper()
                             .FindClearPath(GetOriginPosition(), temp.GetAllyBase(affiliation).GetOriginPosition(), mH);
                else
                    return mH.GetPathHelper().FindClearPath(GetOriginPosition(), captor.GetOriginPosition(), mH);
            }
        }
    }
}