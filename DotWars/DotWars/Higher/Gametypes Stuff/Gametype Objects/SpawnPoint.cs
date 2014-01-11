using Microsoft.Xna.Framework;

namespace DotWars
{
    public class SpawnPoint : NPC
    {
        public NPC.AffliationTypes affilation;
        private bool isGoodSpawnPoint;
        private double spawnCounter; //
        public Vector2 spawnPoint;
        private double spawnTime; //
        private bool isOneUse;
        private bool isUsed;

        public SpawnPoint(Vector2 sP, NPC.AffliationTypes a, ManagerHelper mH)
            :base(null, sP)
        {
            spawnPoint = sP;
            affilation = a;
            isGoodSpawnPoint = true;
            spawnCounter = 0;
            spawnTime = mH.GetGametype().GetSpawnTime()*1.1; //so same spot wont be used twice in a row
            velocity = Vector2.Zero;
            isOneUse = (mH.GetGametype() is Conquest) && (affilation == NPC.AffliationTypes.grey);
            isUsed = false;
        }

        public override void Update(ManagerHelper mH)
        {
            //if black dont work
            if (affilation == NPC.AffliationTypes.black)
            {
                spawnTime = 10000;
                spawnCounter = 0;
            }

            else if(isOneUse && !isUsed)
            {
                spawnTime = 8;
                NPCManager temp = mH.GetNPCManager();

                if (spawnCounter > spawnTime)
                {
                    isGoodSpawnPoint = true;

                    foreach (var agent in mH.GetNPCManager().GetNPCs())
                    {
                        if (agent.GetAffiliation() != affiliation && NPCManager.IsNPCInRadius(agent, spawnPoint, 100))
                        {
                            isGoodSpawnPoint = false;
                        }
                    }
                }
                else
                {
                    spawnCounter += mH.GetGameTime().ElapsedGameTime.TotalSeconds;
                }
            }

            else if(!isUsed)
            {
                spawnTime = 8;

                NPCManager temp = mH.GetNPCManager();

                if (spawnCounter > spawnTime)
                {
                    isGoodSpawnPoint = true;

                    foreach (var agent in mH.GetNPCManager().GetNPCs())
                    {
                        if (agent.GetAffiliation() != affiliation && NPCManager.IsNPCInRadius(agent, spawnPoint, 100))
                        {
                            isGoodSpawnPoint = false;
                        }
                    }
                }
                else
                {
                    spawnCounter += mH.GetGameTime().ElapsedGameTime.TotalSeconds;
                }
            }
        }

        public void NPCUpdate(ManagerHelper mH)
        {
            base.Update(mH);
        }

        public void Spawn()
        {
            if (isOneUse)
                isUsed = true;

            isGoodSpawnPoint = false;
            spawnCounter = 0;
        }

        public bool IsUsedUp()
        {
            return isUsed && isOneUse;
        }

        public bool IsGoodSpawnPoint()
        {
            return isGoodSpawnPoint;
        }

        public NPC.AffliationTypes GetAffilation()
        {
            return affilation;
        }
    }
}