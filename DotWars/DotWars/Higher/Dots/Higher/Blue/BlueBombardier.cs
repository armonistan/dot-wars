using Microsoft.Xna.Framework;

namespace DotWars
{
    public class BlueBombardier : Bombardier
    {
        public BlueBombardier(Vector2 p)
            : base("Dots/Blue/bombardier_blue", p)
        {
            affiliation = AffliationTypes.blue;
            radioWave = new Sprite("Effects/bombardier_blueVFX", this.position);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sB, Vector2 displacement, ManagerHelper mH)
        {
            base.Draw(sB, displacement, mH);
            
        }
    }
}