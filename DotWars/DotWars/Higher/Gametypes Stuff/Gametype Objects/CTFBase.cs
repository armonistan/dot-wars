#region

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace DotWars
{
    public class CTFBase : Environment
    {
        #region Declarations

        //spawner counters
        private readonly Flag myFlag;
        private readonly double spawnTime;

        public NPC.AffliationTypes affiliation;
        private List<NPC> allies;
        private double spawnCounter;

        #endregion

        public CTFBase(NPC.AffliationTypes aT, Vector2 p, ManagerHelper mH)
            : base("bases", p, Vector2.Zero)
        {
            affiliation = aT;

            spawnCounter = 0;
            spawnTime = 1.5f; //1.5 seconds

            switch (aT)
            {
                case NPC.AffliationTypes.red:
                    myFlag = new RedFlag(GetOriginPosition());
                    break;
                case NPC.AffliationTypes.blue:
                    myFlag = new BlueFlag(GetOriginPosition());
                    break;
                case NPC.AffliationTypes.green:
                    myFlag = new GreenFlag(GetOriginPosition());
                    break;
                case NPC.AffliationTypes.yellow:
                    myFlag = new YellowFlag(GetOriginPosition());
                    break;
            }
        }

        public override void LoadContent(TextureManager tM)
        {
            if (myFlag != null)
                myFlag.LoadContent(tM);

            base.LoadContent(tM);

            switch (affiliation)
            {
                case NPC.AffliationTypes.red:
                    frameIndex = 1;
                    break;
                case NPC.AffliationTypes.blue:
                    frameIndex = 2;
                    break;
                case NPC.AffliationTypes.green:
                    frameIndex = 3;
                    break;
                case NPC.AffliationTypes.yellow:
                    frameIndex = 4;
                    break;
            }
        }

        public override void Update(ManagerHelper mH)
        {
            if (myFlag != null)
            {
                myFlag.Update(mH);
            }
            Spawn(mH);
            base.Update(mH);
        }

        public override void Draw(SpriteBatch sB, Vector2 d, ManagerHelper mH)
        {
            base.Draw(sB, d, mH);
        }

        public void DrawFlag(SpriteBatch sB, Vector2 d, ManagerHelper mH)
        {
            if (myFlag != null)
            {
                myFlag.Draw(sB, d, mH);
            }
        }

        private void Spawn(ManagerHelper mH)
        {
            allies = mH.GetNPCManager().GetAllies(affiliation);

            if (spawnCounter > spawnTime && allies.Count < mH.GetGametype().GetPopCap())
            {
                for (int i = 0;
                     i < 50 && !mH.GetGametype().Spawn(mH, affiliation, mH.GetSpawnHelper().Spawn(affiliation), mH.GetRandom().Next(6, 9));
                     i++)
                {
                    //Ain't this just a gem?
                }

                spawnCounter = 0.0;
            }

            else
            {
                spawnCounter += mH.GetGameTime().ElapsedGameTime.TotalSeconds;
            }
        }

        //allows us to access myFlag
        public Flag GetMyFlag()
        {
            return myFlag;
        }
    }
}