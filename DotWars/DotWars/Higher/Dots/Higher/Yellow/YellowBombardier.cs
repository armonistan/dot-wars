using Microsoft.Xna.Framework;

namespace DotWars
{
    public class YellowBombardier : Bombardier
    {
        public YellowBombardier(Vector2 p)
            : base("Dots/Yellow/bombardier_yellow", p)
        {
            affiliation = AffliationTypes.yellow;
            radioWave = new Sprite("Effects/bombardier_yellowVFX", this.position);
        }

        //This method will be used to dictate the AI's behavior in this public class
        public override void Update(ManagerHelper mH)
        {
            base.Update(mH);
        }
    }
}