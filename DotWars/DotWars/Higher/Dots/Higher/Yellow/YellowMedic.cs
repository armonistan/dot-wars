#region

using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    public class YellowMedic : Medic
    {
        public YellowMedic(Vector2 p)
            : base("Dots/Yellow/medic_yellow", p)
        {
            affiliation = AffliationTypes.yellow;
        }

        //This method will be used to dictate the AI's behavior in this public class
        public override void Update(ManagerHelper mH)
        {
            base.Update(mH);
        }
    }
}