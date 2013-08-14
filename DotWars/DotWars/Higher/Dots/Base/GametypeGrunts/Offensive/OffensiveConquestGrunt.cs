using Microsoft.Xna.Framework;

namespace DotWars
{
    public class OffensiveConquestGrunt : Grunt
    {
        public OffensiveConquestGrunt(string a, Vector2 p)
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
            var temp = (Conquest) mH.GetGametype();
            ConquestBase targetBase = temp.GetClosestInList(temp.GetEnemyBases(affiliation), GetOriginPosition());

            if (targetBase != null)
            {
                if (PathHelper.Distance(GetOriginPosition(), targetBase.GetOriginPosition()) > 32)
                    return mH.GetPathHelper().FindClearPath(GetOriginPosition(), targetBase.GetOriginPosition(), mH);

                else
                    return HoverPath(mH, targetBase.GetOriginPosition(), 32);
            }

            else
            {
                return base.NewPath(mH);
            }
        }
    }
}