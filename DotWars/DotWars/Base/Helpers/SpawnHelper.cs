#region

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace DotWars
{
    public class SpawnHelper
    {
        private readonly List<SpawnPoint> spawnPoints;
        private readonly List<SuicideSpawnPoint> suicideSpawnPoints;
        private ManagerHelper managers;

        public SpawnHelper(List<SpawnPoint> sP)
        {
            spawnPoints = sP;
        }

        public SpawnHelper(List<ConquestBase> cB, List<SpawnPoint> sPs)
        {
            spawnPoints = sPs;

            InitializeConquest(cB);
        }

        public SpawnHelper(List<SpawnPoint> sP, List<SuicideSpawnPoint> sSP)
        {
            spawnPoints = sP;
            suicideSpawnPoints = sSP;
        }

        public void Intialize(ManagerHelper mH)
        {
            managers = mH;
        }

        public void LoadTextures(TextureManager cM)
        {
            foreach (SuicideSpawnPoint sPP in suicideSpawnPoints)
                sPP.LoadContent(cM);
        }

        public void InitializeConquest(List<ConquestBase> cB)
        {
            foreach (ConquestBase c in cB)
                foreach (SpawnPoint sP in c.spawns)
                    spawnPoints.Add(sP);
        }

        public void Update()
        {
            foreach (SpawnPoint sP in spawnPoints)
                sP.Update(managers);
            foreach (SuicideSpawnPoint sPP in suicideSpawnPoints)
                sPP.Update(managers);
        }

        public Vector2 Spawn(NPC.AffliationTypes a)
        {
            for (int i = 0; i < 50; i++)
            {
                int x = managers.GetRandom().Next(spawnPoints.Count);
                SpawnPoint sP = spawnPoints[x];

                if (sP.IsGoodSpawnPoint() && (a == sP.GetAffilation() || sP.GetAffilation() == NPC.AffliationTypes.grey))
                {
                    sP.Spawn();
                    return sP.spawnPoint;
                }
            }

            return LastPickSpawn(a);
        }

        public Vector2 Spawn(NPC.AffliationTypes a, bool canUseNeutral)
        {
            for (int i = 0; i < 50; i++)
            {
                int x = managers.GetRandom().Next(spawnPoints.Count);
                SpawnPoint sP = spawnPoints[x];

                if (sP.IsGoodSpawnPoint() &&
                    (a == sP.GetAffilation() || (sP.GetAffilation() == NPC.AffliationTypes.grey && canUseNeutral)))
                {
                    sP.Spawn();
                    return sP.spawnPoint;
                }
            }

            //less picky choice returned
            return LastPickSpawn(a, canUseNeutral);
        }

        public Vector2 SpawnSucideDots()
        {
            int spawnPick;
            bool proceed = false;
            int counter = 0;

            do
            {
                spawnPick = managers.GetRandom().Next(0, suicideSpawnPoints.Count);
                counter++;

                List<NPC> suicides = managers.GetNPCManager().GetAllies(NPC.AffliationTypes.black);
                int suicideCount = 0;

                foreach (NPC suicide in suicides)
                {
                    if (NPCManager.IsNPCInRadius(suicide, suicideSpawnPoints[spawnPick].GetOriginPosition(), 60))
                    {
                        suicideCount++;
                    }

                    if (suicideCount >= 1)
                    {
                        break;
                    }
                }

                if (suicideCount < 1)
                    proceed = true;
            } while (!proceed && counter < 240);

            return suicideSpawnPoints[spawnPick].GetOriginPosition();
        }

        private Vector2 LastPickSpawn(NPC.AffliationTypes a, bool canUseNeutral)
        {
            var v = new Vector2();

            for (int i = 0; i < 1000; i++)
            {
                int x = managers.GetRandom().Next(spawnPoints.Count);
                SpawnPoint sP = spawnPoints[x];

                if (a == sP.GetAffilation() || (sP.GetAffilation() == NPC.AffliationTypes.grey && canUseNeutral))
                {
                    sP.Spawn();
                    return sP.spawnPoint;
                }
            }

            return v;
        }

        private Vector2 LastPickSpawn(NPC.AffliationTypes a)
        {
            var v = new Vector2();

            for (int i = 0; i < 1000; i++)
            {
                int x = managers.GetRandom().Next(spawnPoints.Count);
                SpawnPoint sP = spawnPoints[x];

                if (a == sP.GetAffilation() || (sP.GetAffilation() == NPC.AffliationTypes.grey && sP.IsGoodSpawnPoint()))
                {
                    sP.Spawn();
                    return sP.spawnPoint;
                }
            }

            return v;
        }

        public void Draw(SpriteBatch sB, Vector2 d)
        {
            foreach (SpawnPoint sP in suicideSpawnPoints)
                sP.Draw(sB, d, managers);
        }
    }
}