#region

using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    public class BlueMedic : Medic
    {
        public BlueMedic(Vector2 p)
            : base("Dots/Blue/medic_blue", p)
        {
            affiliation = AffliationTypes.blue;
        }

        //This method will be used to dictate the AI's behavior in this public class
        public override void Update(ManagerHelper mH)
        {
            base.Update(mH);
        }
    }
}