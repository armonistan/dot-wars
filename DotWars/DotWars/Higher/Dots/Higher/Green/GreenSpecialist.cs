using Microsoft.Xna.Framework;

namespace DotWars
{
    public class GreenSpecialist : Specialist
    {
        public GreenSpecialist(Vector2 p)
            : base("Dots/Green/specialist_green", p)
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