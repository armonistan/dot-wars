using Microsoft.Xna.Framework;

namespace DotWars
{
    public class BlueBombardier : Bombardier
    {
        public BlueBombardier(Vector2 p)
            : base("Dots/Blue/bombardier_blue", p)
        {
            affiliation = AffliationTypes.blue;
        }
    }
}