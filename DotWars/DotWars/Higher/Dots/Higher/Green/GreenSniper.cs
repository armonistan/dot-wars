using Microsoft.Xna.Framework;

namespace DotWars
{
    public class GreenSniper : Sniper
    {
        public GreenSniper(Vector2 p)
            : base("Dots/Green/sniper_green", p)
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