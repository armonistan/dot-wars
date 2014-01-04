using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DotWars
{
    public class Claimable : Sprite
    {
        #region Declarations

        private List<NPC> allies;

        private double counter;
        public bool taken;

        #endregion

        public Claimable(Vector2 p)
            : base("Dots/Grey/grey_claimable", p)
        {
            counter = 0;
            taken = false;
        }

        public override void Update(ManagerHelper mH)
        {
            if (!taken)
            {
                foreach (NPC a in mH.GetNPCManager().GetCommanders())
                {
                    if (CollisionHelper.IntersectPixelsRadius(a, this, 16, 32) != new Vector2(-1))
                    {
                        if (mH.GetNPCManager().GetAllies(a.GetAffiliation()).Count < mH.GetGametype().GetPopCap())
                        {
                            taken = true;
                            mH.GetGametype().UpdateDotsRecruited(a.GetPersonalAffilation());

                            for (int i = 0;
                                 i < 50 && !mH.GetGametype().Spawn(mH, a.GetAffiliation(), GetOriginPosition());
                                 i++)
                            {
                                //Ain't this just a gem?
                            }
                        }
                    }
                }
            }
            else
            {
                if (counter > 7) // counter greater than x number of seconds
                {
                    Activate();
                    counter = 0;
                }
                else
                    counter += mH.GetGameTime().ElapsedGameTime.TotalSeconds;
            }
        }

        public override void Draw(SpriteBatch sB, Vector2 d, ManagerHelper mH)
        {
            if (!taken)
            {
                base.Draw(sB, d, mH);
            }
        }

        public void Activate()
        {
            taken = false;
        }
    }
}