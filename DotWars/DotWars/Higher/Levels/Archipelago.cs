using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace DotWars
{
    public class Archipelago : Level
    {
        #region Declarations

        private float danielIsGay = 10;
        private float danielsGayness = 0;

        #endregion

        public Archipelago(Gametype gT, Dictionary<Type, int> pL, TextureManager tM, AudioManager audio) :
            base(gT, pL, new Vector2(1312, 992), tM, audio)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            backgrounds.AddBackground(new Sprite("Backgrounds/Archipelago/archipelago_background", new Vector2(656, 496)));
            backgrounds.AddForeground(new Sprite("Backgrounds/Archipelago/archipelago_trees", new Vector2(656, 496)));
        }

        public override void LoadContent(ContentManager cM)
        {
            base.LoadContent(cM);

            objects.AddStaticBlocker(new InDestructable("Backgrounds/Archipelago/archipelago_solid",
                                                        new Vector2(656, 496)));
            objects.AddImpathable(new WhirlPool(new Vector2(640, 384)));

            if (typeOfGame is Assault)
            {
                //Set up bases
                var temp = (Assault) typeOfGame;
                var tempBases = new List<AssaultBase>();
                //attacker
                tempBases.Add(new AssaultBase(temp.GetTeams()[0], new Vector2(156, 180), managers));
                //defender
                tempBases.Add(new AssaultBase(temp.GetTeams()[1], new Vector2(876, 798), managers));

                //intitialize bases
                temp.Initialize(managers, tempBases);

                //sniper positions
                sniperSpots.Add(new Vector2(60, 464));
                sniperSpots.Add(new Vector2(960, 160));
                sniperSpots.Add(new Vector2(390, 880));
                sniperSpots.Add(new Vector2(1050, 600));

                //set up spawns
                //team one (attackers) spawns
                spawnplaces.Add(new SpawnPoint(temp.GetAllyBase(temp.GetAttacker()).GetOriginPosition(),
                                               temp.GetTeams()[0], managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(50, 60), temp.GetTeams()[0], managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(60, 390), temp.GetTeams()[0], managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(670, 40), temp.GetTeams()[0], managers));
                //team two (defenders) spawns
                spawnplaces.Add(new SpawnPoint(temp.GetAllyBase(temp.GetDefender()).GetOriginPosition(),
                                               temp.GetTeams()[1], managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(830, 950), temp.GetTeams()[1], managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(1160, 780), temp.GetTeams()[1], managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(1170, 580), temp.GetTeams()[1], managers));
                //neutral spawns
                spawnplaces.Add(new SpawnPoint(new Vector2(1120, 220), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(400, 880), NPC.AffliationTypes.grey, managers));
            }

            else if (typeOfGame is Conquest)
            {
                //Set up bases
                var tempCon = (Conquest) typeOfGame;
                var tempBases = new List<ConquestBase>();

                //mid left
                tempBases.Add(new ConquestBase(new Vector2(680, 80)));
                //bot left
                tempBases.Add(new ConquestBase(new Vector2(240, 612)));
                //top right
                tempBases.Add(new ConquestBase(new Vector2(1080, 220)));
                //fortress
                tempBases.Add(new ConquestBase(new Vector2(840, 795)));

                //sniper positions
                sniperSpots.Add(new Vector2(60, 464));
                sniperSpots.Add(new Vector2(960, 160));
                sniperSpots.Add(new Vector2(390, 880));
                sniperSpots.Add(new Vector2(1050, 600));

                //base related spawns
                tempBases[0].AddSpawnpoint(tempBases[0].GetOriginPosition(), managers);
                tempBases[0].AddSpawnpoint(new Vector2(760, 70), managers);
                tempBases[0].AddSpawnpoint(new Vector2(280, 70), managers);

                tempBases[1].AddSpawnpoint(tempBases[1].GetOriginPosition(), managers);
                tempBases[1].AddSpawnpoint(new Vector2(60, 330), managers);
                tempBases[1].AddSpawnpoint(new Vector2(140, 660), managers);

                tempBases[2].AddSpawnpoint(tempBases[2].GetOriginPosition(), managers);
                tempBases[2].AddSpawnpoint(new Vector2(1100, 140), managers);
                tempBases[2].AddSpawnpoint(new Vector2(1260, 360), managers);

                tempBases[3].AddSpawnpoint(tempBases[3].GetOriginPosition(), managers);
                tempBases[3].AddSpawnpoint(new Vector2(800, 960), managers);
                tempBases[3].AddSpawnpoint(new Vector2(1200, 900), managers);


                //netural spawns
                spawnplaces.Add(new SpawnPoint(new Vector2(290, 50), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(1230, 900), NPC.AffliationTypes.grey, managers));

                //set up new spawnhelper for conquest gametype
                tempCon.Initialize(managers, tempBases);
                spawns.InitializeConquest(tempBases);
            }
        }

        public override String ToString()
        {
            return "Archipelago";
        }
    }
}