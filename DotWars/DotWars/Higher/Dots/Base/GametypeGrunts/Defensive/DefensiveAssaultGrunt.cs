#region

using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    public class DefensiveAssaultGrunt : Grunt
    {
        const int RADIUS = 128;

        public DefensiveAssaultGrunt(string a, Vector2 p)
            : base(a, p)
        {
        }

        protected override void SpecialPath(ManagerHelper mH)
        {
            var temp = mH.Assault;

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