using Microsoft.Xna.Framework;

namespace DotWars
{
    public class GreenBombardier : Bombardier
    {
        public GreenBombardier(Vector2 p)
            : base("Dots/Green/bombardier_green", p)
        {
            affiliation = AffliationTypes.green;
            radioWave = new Sprite("Effects/bombardier_greenVFX", this.position);
        }

        //This method will be used to dictate the AI's behavior in this public class
        public override void Update(ManagerHelper mH)
        {
            base.Update(mH);
        }
    }
}