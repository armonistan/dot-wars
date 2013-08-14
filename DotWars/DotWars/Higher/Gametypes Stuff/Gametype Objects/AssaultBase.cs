using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DotWars
{
    public class AssaultBase : Environment
    {
        #region Declarations

        //spawner counters
        private readonly Flag myFlag;
        private readonly double spawnTime;

        public NPC.AffliationTypes affiliation;
        private List<NPC> allies;
        private double spawnCounter;

        #endregion

        public AssaultBase(NPC.AffliationTypes a, Vector2 p, ManagerHelper mH)
            : base("bases", p, Vector2.Zero)
        {
            var temp = (Assault) mH.GetGametype();

            affiliation = a;

            if (affiliation == temp.GetAttacker())
                myFlag = null;
            else
            {
                switch (affiliation)
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

            spawnCounter = 0;
            spawnTime = 1.5; //1.5 seconds
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
                Vector2 tempVector = myFlag.GetOriginPosition();
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
            int aT = mH.GetRandom().Next(3, 6);
            var ass = (Assault) mH.GetGametype();

            if (spawnCounter > spawnTime && allies.Count < mH.GetGametype().GetPopCap())
            {
                if (aT != 3 && aT != 6)
                {
                    if (affiliation == ass.GetAttacker())
                        aT = 5;
                    else
                        aT = 4;
                }

                mH.GetGametype().Spawn(mH, affiliation, mH.GetSpawnHelper().Spawn(affiliation), aT);

                spawnCounter = 0;
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