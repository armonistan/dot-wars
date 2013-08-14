using Microsoft.Xna.Framework;

namespace DotWars
{
    public class RedMedic : Medic
    {
        public RedMedic(Vector2 p)
            : base("Dots/Red/medic_red", p)
        {
            affiliation = AffliationTypes.red;
        }

        //This method will be used to dictate the AI's behavior in this public class
        public override void Update(ManagerHelper mH)
        {
            base.Update(mH);
        }
    }
}