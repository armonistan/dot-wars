using Microsoft.Xna.Framework;

namespace DotWars
{
    public class GreenJuggernaut : Juggernaut
    {
        public GreenJuggernaut(Vector2 p)
            : base("Dots/Green/juggernaut_green", p)
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