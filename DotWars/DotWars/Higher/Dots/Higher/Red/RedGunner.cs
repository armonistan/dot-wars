using Microsoft.Xna.Framework;

namespace DotWars
{
    public class RedGunner : Gunner
    {
        public RedGunner(Vector2 p)
            : base("Dots/Red/gunner_red", p)
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