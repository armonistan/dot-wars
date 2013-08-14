using Microsoft.Xna.Framework;

namespace DotWars
{
    public class YellowSniper : Sniper
    {
        public YellowSniper(Vector2 p)
            : base("Dots/Yellow/sniper_yellow", p)
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