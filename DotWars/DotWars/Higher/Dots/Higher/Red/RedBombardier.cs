using Microsoft.Xna.Framework;

namespace DotWars
{
    public class RedBombardier : Bombardier
    {
        public RedBombardier(Vector2 p)
            : base("Dots/Red/bombardier_red", p)
        {
            affiliation = AffliationTypes.red;
            radioWave = new Sprite("Effects/bombardier_redVFX", this.position);
        }

        //This method will be used to dictate the AI's behavior in this public class
        public override void Update(ManagerHelper mH)
        {
            base.Update(mH);
        }
    }
}