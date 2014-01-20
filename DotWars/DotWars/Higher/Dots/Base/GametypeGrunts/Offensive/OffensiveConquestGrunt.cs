#region

using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    public class OffensiveConquestGrunt : Grunt
    {
        public OffensiveConquestGrunt(string a, Vector2 p)
            : base(a, p)
        {
        }

        protected override void SpecialPath(ManagerHelper mH)
        {
            var temp = mH.Conquest;

            ConquestBase targetBase = null;
            float distanceToClosest = float.PositiveInfinity;

            foreach (ConquestBase conquestBase in temp.GetBases())
            {
                if (conquestBase.affiliation != affiliation)
                {
                    float distanceToBase = PathHelper.DistanceSquared(GetOriginPosition(),
                                                                      conquestBase.GetOriginPosition());

                    if (distanceToBase < distanceToClosest)
                    {
                        distanceToClosest = distanceToBase;
                        targetBase = conquestBase;
                    }
                }
            }

            if (targetBase != null)
            {
                if (PathHelper.DistanceSquared(GetOriginPosition(), targetBase.GetOriginPosition()) > 32*32)
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