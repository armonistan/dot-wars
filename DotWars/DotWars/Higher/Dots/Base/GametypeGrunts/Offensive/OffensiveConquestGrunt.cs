using Microsoft.Xna.Framework;

namespace DotWars
{
    public class OffensiveConquestGrunt : Grunt
    {
        public OffensiveConquestGrunt(string a, Vector2 p)
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
            var temp = (Conquest) mH.GetGametype();
            ConquestBase targetBase = temp.GetClosestInList(temp.GetEnemyBases(affiliation), GetOriginPosition());

            if (targetBase != null)
            {
                if (PathHelper.DistanceSquared(GetOriginPosition(), targetBase.GetOriginPosition()) > 32 * 32)
                    mH.GetPathHelper().FindClearPath(GetOriginPosition(), targetBase.GetOriginPosition(), mH, path);

                else
                    HoverPath(mH, targetBase.GetOriginPosition(), 32);
            }

            else
            {
                EngagePath(mH);
            }
        }
    }
}