#region

using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    public class DefensiveCTFGrunt : Grunt
    {
        const int RADIUS = 128;

        public DefensiveCTFGrunt(string a, Vector2 p)
            : base(a, p)
        {
        }

        protected override void SpecialPath(ManagerHelper mH)
        {
            var temp = mH.CaptureTheFlag;

            if (temp.GetAllyBase(affiliation).GetMyFlag().status == Flag.FlagStatus.home)
            {
                this.HoverPath(mH, temp.GetAllyBase(affiliation).originPosition, RADIUS);
            }
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