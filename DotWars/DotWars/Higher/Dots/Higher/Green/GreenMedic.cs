#region

using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    public class GreenMedic : Medic
    {
        public GreenMedic(Vector2 p)
            : base("Dots/Green/medic_green", p)
        {
            affiliation = AffliationTypes.green;
        }

        //This method will be used to dictate the AI's behavior in this public class
        public override void Update(ManagerHelper mH)
        {
            base.Update(mH);
        }
    }
}