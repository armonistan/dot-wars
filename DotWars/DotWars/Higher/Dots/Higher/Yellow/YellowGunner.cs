using Microsoft.Xna.Framework;

namespace DotWars
{
    public class YellowGunner : Gunner
    {
        public YellowGunner(Vector2 p)
            : base("Dots/Yellow/gunner_yellow", p)
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