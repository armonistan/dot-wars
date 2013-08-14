using Microsoft.Xna.Framework;

namespace DotWars
{
    public class GreenGunner : Gunner
    {
        public GreenGunner(Vector2 p)
            : base("Dots/Green/gunner_green", p)
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