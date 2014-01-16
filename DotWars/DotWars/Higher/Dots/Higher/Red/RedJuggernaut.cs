#region

using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    public class RedJuggernaut : Juggernaut
    {
        public RedJuggernaut(Vector2 p)
            : base("Dots/Red/juggernaut_red", p)
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