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
        private Vector2 velocity;

        public SpawnPoint(Vector2 sP, NPC.AffliationTypes a, ManagerHelper mH)
            :base(null, sP)
        {
            spawnPoint = sP;
            affilation = a;
            isGoodSpawnPoint = true;
            spawnCounter = 0;
            spawnTime = mH.GetGametype().GetSpawnTime()*1.1; //so same spot wont be used twice in a row
            velocity = Vector2.Zero;
            
           
            if (a == NPC.AffliationTypes.grey)
                spawnTime = 100000000;
        }

        public override void Update(ManagerHelper mH)
        {
            if (affilation == NPC.AffliationTypes.black)
            {
                spawnTime = 10000;
                spawnCounter = 0;
            }

            else
                spawnTime = 8;

            NPCManager temp = mH.GetNPCManager();

            if (spawnCounter > spawnTime)
            {
                isGoodSpawnPoint = temp.GetAllButAlliesInRadius(affilation, spawnPoint, 100).Count == 0;
            }
            else
            {
                spawnCounter += mH.GetGameTime().ElapsedGameTime.TotalSeconds;
            }
        }

        public void NPCUpdate(ManagerHelper mH)
        {
            base.Update(mH);
        }

        public void Spawn()
        {
            isGoodSpawnPoint = false;
            spawnCounter = 0;
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