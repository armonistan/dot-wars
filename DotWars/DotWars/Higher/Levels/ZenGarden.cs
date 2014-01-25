#region

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

#endregion

namespace DotWars
{
    public class ZenGarden : Level
    {
        #region Declarations

        #endregion

        public ZenGarden(Gametype gT, Dictionary<Type, int> pL, TextureManager tM, AudioManager audio) :
            base(gT, pL, new Vector2(1312, 992), tM, audio)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            //Set up background
            backgrounds.AddBackground(new Sprite("Backgrounds/ZenGarden/midwinterBackground", new Vector2(656, 496)));
            backgrounds.AddForeground(new Sprite("Backgrounds/ZenGarden/midwinterForeground", new Vector2(656, 496)));

            sniperSpots.Add(new Vector2(90, 60));
            sniperSpots.Add(new Vector2(290, 780));
            sniperSpots.Add(new Vector2(670, 250));
            sniperSpots.Add(new Vector2(670, 780));
            sniperSpots.Add(new Vector2(1180, 960));
            sniperSpots.Add(new Vector2(1290, 340));
        }

        public override void LoadContent(ContentManager cM)
        {
            base.LoadContent(cM);

            objects.AddStaticBlocker(new InDestructable("Backgrounds/ZenGarden/midwinterBlockers", new Vector2(656, 496)));

            if (typeOfGame is Deathmatch)
            {
                var tempDM = (Deathmatch) typeOfGame;
                var tempClaimables = new List<Claimable>();

                //Claimables
                tempClaimables.Add(new Claimable(new Vector2(159, 509)));
                tempClaimables.Add(new Claimable(new Vector2(434, 525)));
                tempClaimables.Add(new Claimable(new Vector2(821, 40)));
                tempClaimables.Add(new Claimable(new Vector2(859, 519)));
                tempClaimables.Add(new Claimable(new Vector2(800, 979)));
                tempClaimables.Add(new Claimable(new Vector2(1084, 478)));

                tempDM.Initialize(managers, tempClaimables);

                //spawn points
                spawnplaces.Add(new SpawnPoint(new Vector2(493, 90), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(1286, 169), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(332, 725), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(1094, 943), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(1029, 246), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(298, 246), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(271, 910), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(734, 767), NPC.AffliationTypes.grey, managers));
            }

            else if (typeOfGame is Survival)
            {
                var temp = (Survival) typeOfGame;

                var tempClaimables = new List<Claimable>();
                tempClaimables.Add(new Claimable(new Vector2(107, 181)));
                tempClaimables.Add(new Claimable(new Vector2(433, 43)));
                tempClaimables.Add(new Claimable(new Vector2(433, 510)));
                tempClaimables.Add(new Claimable(new Vector2(784, 371)));

                suicideSpawnplaces.Add(new SuicideSpawnPoint(new Vector2(123, 16), managers));
                suicideSpawnplaces.Add(new SuicideSpawnPoint(new Vector2(16, 138), managers));
                suicideSpawnplaces.Add(new SuicideSpawnPoint(new Vector2(16, 502), managers));
                suicideSpawnplaces.Add(new SuicideSpawnPoint(new Vector2(16, 903), managers));
                suicideSpawnplaces.Add(new SuicideSpawnPoint(new Vector2(123, 992), managers));
                suicideSpawnplaces.Add(new SuicideSpawnPoint(new Vector2(1191, 16), managers));
                suicideSpawnplaces.Add(new SuicideSpawnPoint(new Vector2(1312, 150), managers));
                suicideSpawnplaces.Add(new SuicideSpawnPoint(new Vector2(1312, 585), managers));
                suicideSpawnplaces.Add(new SuicideSpawnPoint(new Vector2(1312, 889), managers));
                suicideSpawnplaces.Add(new SuicideSpawnPoint(new Vector2(1212, 992), managers));

                spawnplaces.Add(new SpawnPoint(new Vector2(316, 266), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(554, 212), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(782, 787), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(1028, 780), NPC.AffliationTypes.grey, managers));

                spawns.LoadTextures(textures);

                temp.Initialize(managers, tempClaimables);
            }
        }

        public override String ToString()
        {
            return "Zen Garden";
        }
    }
}