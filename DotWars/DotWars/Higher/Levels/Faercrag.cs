#region

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

#endregion

namespace DotWars
{
    public class Faercrag : Level
    {
        #region Declarations

        #endregion

        public Faercrag(Gametype gT, Dictionary<Type, int> pL, TextureManager tM, AudioManager audio) :
            base(gT, pL, new Vector2(1312, 992), tM, audio)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            //Set up background
            backgrounds.AddBackground(new Lava("Backgrounds/Caged/lava", new Vector2(151+180, 272), new Vector2(1f, 0), 1+180, 600+180));
            backgrounds.AddBackground(new Lava("Backgrounds/Caged/lava", new Vector2(451+180, 272), new Vector2(1f, 0), 1+180, 600+180));
            backgrounds.AddBackground(new Lava("Backgrounds/Caged/lava", new Vector2(151+180, 720), new Vector2(1f, 0), 1+180, 600+180));
            backgrounds.AddBackground(new Lava("Backgrounds/Caged/lava", new Vector2(451+180, 720), new Vector2(1f, 0), 1+180, 600+180));
            backgrounds.AddBackground(new Sprite("Backgrounds/Faercrag/faercragBackground", new Vector2(656, 496)));

            //sniper points
            sniperSpots.Add(new Vector2(320, 940));
            sniperSpots.Add(new Vector2(975, 53));
            sniperSpots.Add(new Vector2(352, 32));
            sniperSpots.Add(new Vector2(960, 960));
        }

        public override void LoadContent(ContentManager cM)
        {
            base.LoadContent(cM);

            objects.AddStaticBlocker(new InDestructable("Backgrounds/Faercrag/faercragBlockers", new Vector2(656, 496)));
            //objects.AddImpassable(new Impassable("Backgrounds/Faercrag/faercragImpassable", new Vector2(656, 496)));
            objects.AddImpathable(new LavaBurner("Backgrounds/Faercrag/faercragLava", new Vector2(656, 496)), true);

            if (typeOfGame is CaptureTheFlag)
            {
                //set up gametype
                var temp = (CaptureTheFlag) typeOfGame;
                var tempBases = new List<CTFBase>();

                //Set up bases
                tempBases.Add(new CTFBase(temp.GetTeams()[0], new Vector2(80, 86), managers));
                tempBases.Add(new CTFBase(temp.GetTeams()[1], new Vector2(1250, 920), managers));
                temp.Initialize(managers, tempBases);

                //spawnpoints
                //team one spawns
                spawnplaces.Add(new SpawnPoint(temp.GetAllyBase(temp.GetTeams()[0]).GetOriginPosition(),
                                               temp.GetTeams()[0], managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(128, 320), temp.GetTeams()[0], managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(128, 672), temp.GetTeams()[0], managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(512, 128), temp.GetTeams()[0], managers));
                //team two spawns
                spawnplaces.Add(new SpawnPoint(temp.GetAllyBase(temp.GetTeams()[1]).GetOriginPosition(),
                                               temp.GetTeams()[1], managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(1184, 320), temp.GetTeams()[1], managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(1184, 672), temp.GetTeams()[1], managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(800, 896), temp.GetTeams()[1], managers));
                //neutral spawns
                spawnplaces.Add(new SpawnPoint(new Vector2(992, 672), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(992, 320), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(320, 672), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(320, 320), NPC.AffliationTypes.grey, managers));
            }
            else if (typeOfGame is Conquest)
            {
                var tempCon = (Conquest) typeOfGame;
                var tempBases = new List<ConquestBase>();

                //Bases
                tempBases.Add(new ConquestBase(new Vector2(80, 86)));
                tempBases[0].AddSpawnpoint(new Vector2(128, 320), managers);
                tempBases[0].AddSpawnpoint(new Vector2(512, 128), managers);
                tempBases.Add(new ConquestBase(new Vector2(1250, 920)));
                tempBases[1].AddSpawnpoint(new Vector2(1184, 672), managers);
                tempBases[1].AddSpawnpoint(new Vector2(800, 896), managers);
                tempBases.Add(new ConquestBase(new Vector2(960, 320)));
                tempBases[2].AddSpawnpoint(new Vector2(896, 256), managers);
                tempBases[2].AddSpawnpoint(new Vector2(896, 384), managers);
                tempBases.Add(new ConquestBase(new Vector2(352, 672)));
                tempBases[3].AddSpawnpoint(new Vector2(416, 576), managers);
                tempBases[3].AddSpawnpoint(new Vector2(416, 736), managers);

                //Spawnpoints
                spawnplaces.Add(new SpawnPoint(new Vector2(128, 672), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(1184, 320), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(992, 672), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(320, 320), NPC.AffliationTypes.grey, managers));

                tempCon.Initialize(managers, tempBases);
                spawns.InitializeConquest(tempBases);
            }
        }

        public override String ToString()
        {
            return "Faercrag";
        }
    }
}