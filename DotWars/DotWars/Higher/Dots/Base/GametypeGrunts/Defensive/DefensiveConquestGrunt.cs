using Microsoft.Xna.Framework;

namespace DotWars
{
    public class DefensiveConquestGrunt : Grunt
    {
        public DefensiveConquestGrunt(string a, Vector2 p)
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
            ConquestBase forwardBase = temp.GetForwardBase(affiliation, mH);
            if (forwardBase != null)
            {
                return mH.GetPathHelper().FindClearPath(GetOriginPosition(), forwardBase.GetOriginPosition(), mH);
            }
            else
            {
                return EngagePath(mH);
            }
        }
    }
}