using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace DotWars
{
    public class Plaza : Level
    {
        private Vector2 savingTime = new Vector2(16);

        public Plaza(Gametype gT, Dictionary<Type, int> pL, TextureManager tM, AudioManager audio) :
            base(gT, pL, new Vector2(1296, 976), tM, audio)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            backgrounds.AddForeground(new Sprite("Backgrounds/Plaza/plazaLighting", new Vector2(664, 504) - savingTime));
            backgrounds.AddBackground(new Sprite("Backgrounds/Plaza/plazaBackground", new Vector2(664, 504) - savingTime));

            sniperSpots.Add(new Vector2(80, 270));
            sniperSpots.Add(new Vector2(930, 100));
            sniperSpots.Add(new Vector2(930, 950));
            sniperSpots.Add(new Vector2(1280, 270));
        }

        public override void LoadContent(ContentManager cM)
        {
            base.LoadContent(cM);

            objects.AddStaticBlocker(new InDestructable("Backgrounds/Plaza/plazaBlockers", new Vector2(664, 504) - savingTime));
            objects.AddTopObject(new AnimatedLight(new Vector2(516, 328) - savingTime, 0));
            objects.AddTopObject(new AnimatedLight(new Vector2(872, 326) - savingTime, MathHelper.Pi * .25f));
            objects.AddTopObject(new AnimatedLight(new Vector2(872, 656) - savingTime, MathHelper.Pi * .5f));

            if (typeOfGame is Assault)
            {
                //Set up bases
                var temp = (Assault)typeOfGame;
                var tempBases = new List<AssaultBase>();
                //attacker
                tempBases.Add(new AssaultBase(temp.GetTeams()[0], new Vector2(1132, 110) - savingTime, managers));
                //defender
                tempBases.Add(new AssaultBase(temp.GetTeams()[1], new Vector2(90, 905) - savingTime, managers));

                //intitialize bases
                temp.Initialize(managers, tempBases);

                //sniper positions
                sniperSpots.Add(new Vector2(1290, 930) - savingTime);
                sniperSpots.Add(new Vector2(200, 160) - savingTime);
                sniperSpots.Add(new Vector2(1230, 155) - savingTime);
                sniperSpots.Add(new Vector2(195, 860) - savingTime);

                //set up spawns
                //team one (attackers) spawns
                spawnplaces.Add(new SpawnPoint(temp.GetAllyBase(temp.GetAttacker()).GetOriginPosition() - savingTime,
                                               temp.GetTeams()[0], managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(1050, 150) - savingTime, temp.GetTeams()[0], managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(1290, 405) - savingTime, temp.GetTeams()[0], managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(520, 120) - savingTime, temp.GetTeams()[0], managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(1140, 670) - savingTime, temp.GetTeams()[0], managers));
                
                //team two (defenders) spawns
                spawnplaces.Add(new SpawnPoint(temp.GetAllyBase(temp.GetDefender()).GetOriginPosition() - savingTime,
                                               temp.GetTeams()[1], managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(100, 920) - savingTime, temp.GetTeams()[1], managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(100, 445) - savingTime, temp.GetTeams()[1], managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(550, 960) - savingTime, temp.GetTeams()[1], managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(445, 565) - savingTime, temp.GetTeams()[1], managers));
                
            }

            else if (typeOfGame is Survival)
            {
                var temp = (Survival)typeOfGame;

                var tempClaimables = new List<Claimable>();
                tempClaimables.Add(new Claimable(new Vector2(930, 270) - savingTime));
                tempClaimables.Add(new Claimable(new Vector2(1050, 785) - savingTime));
                tempClaimables.Add(new Claimable(new Vector2(515, 205) - savingTime));
                tempClaimables.Add(new Claimable(new Vector2(380, 630) - savingTime));

                spawnplaces.Add(new SpawnPoint(new Vector2(60, 945) - savingTime, NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(60, 860) - savingTime, NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(155, 945) - savingTime, NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(155, 860) - savingTime, NPC.AffliationTypes.grey, managers));

                suicideSpawnplaces.Add(new SuicideSpawnPoint(new Vector2(520, 120) - savingTime, managers));
                suicideSpawnplaces.Add(new SuicideSpawnPoint(new Vector2(1050, 155) - savingTime, managers));
                suicideSpawnplaces.Add(new SuicideSpawnPoint(new Vector2(200, 160) - savingTime, managers));
                suicideSpawnplaces.Add(new SuicideSpawnPoint(new Vector2(1290, 405) - savingTime, managers));

                spawns.LoadTextures(textures);

                temp.Initialize(managers, tempClaimables);
            }
        }

        public override String ToString()
        {
            return "Plaza";
        }
    }
}