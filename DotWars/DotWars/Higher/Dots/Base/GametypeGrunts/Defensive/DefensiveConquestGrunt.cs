#region

using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    public class DefensiveConquestGrunt : Grunt
    {
        const int RADIUS = 128;

        public DefensiveConquestGrunt(string a, Vector2 p)
            : base(a, p)
        {
        }

        protected override void SpecialPath(ManagerHelper mH)
        {
            var temp = mH.Conquest;
            ConquestBase forwardBase = temp.GetForwardBase(affiliation, mH);

            if (forwardBase != null)
            {
                this.HoverPath(mH, forwardBase.originPosition, RADIUS);
            }
            else
            {
                EngagePath(mH);
            }
        }
    }
}