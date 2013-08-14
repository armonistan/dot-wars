using Microsoft.Xna.Framework;

namespace DotWars
{
    public class RedSniper : Sniper
    {
        public RedSniper(Vector2 p)
            : base("Dots/Red/sniper_red", p)
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