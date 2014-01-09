using Microsoft.Xna.Framework;

namespace DotWars
{
    public class OffensiveAssaultGrunt : Grunt
    {
        public OffensiveAssaultGrunt(string a, Vector2 p)
            : base(a, p)
        {
        }
        
        protected override void SpecialPath(ManagerHelper mH)
        {
            var temp = (Assault) mH.GetGametype();
            Flag f = temp.GetEnemyBase(affiliation).GetMyFlag();

            if (f.status != Flag.FlagStatus.taken)
                mH.GetPathHelper()
                         .FindClearPath(GetOriginPosition(),
                                        temp.GetEnemyBase(affiliation).GetMyFlag().GetOriginPosition(), mH, path);
            else
            {
                NPC captor = temp.GetEnemyBase(affiliation).GetMyFlag().GetCaptor();

                if (captor == this)
                    mH.GetPathHelper()
                             .FindClearPath(GetOriginPosition(), temp.GetAllyBase(affiliation).GetOriginPosition(), mH, path);
                else
                    mH.GetPathHelper().FindClearPath(GetOriginPosition(), captor.GetOriginPosition(), mH, path);
            }
        }
    }
}