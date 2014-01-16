#region

using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    public class BlueGunner : Gunner
    {
        public BlueGunner(Vector2 p)
            : base("Dots/Blue/gunner_blue", p)
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