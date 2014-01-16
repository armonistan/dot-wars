#region

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

#endregion

namespace DotWars
{
    public class Switch : Level
    {
        public Crane theCrane;

        public Switch(Gametype gT, Dictionary<Type, int> pL, TextureManager tM, AudioManager audio) :
            base(gT, pL, new Vector2(800, 800), tM, audio)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            backgrounds.AddBackground(new Sprite("Backgrounds/Switch/switchBackground", new Vector2(400, 400)));
            backgrounds.AddForeground(new Sprite("Backgrounds/Switch/switchForeground", new Vector2(400, 400)));

            sniperSpots.Add(new Vector2(80, 20));
            sniperSpots.Add(new Vector2(770, 720));
            sniperSpots.Add(new Vector2(230, 470));
            sniperSpots.Add(new Vector2(330, 570));
        }

        public override void LoadContent(ContentManager cM)
        {
            base.LoadContent(cM);

            theCrane = new Crane(new Vector2(516, 288));
            objects.AddTopObject(theCrane);

            objects.AddBotObject(new SwitchBeltNetwork(theCrane));
            objects.AddStaticBlocker(new InDestructable("Backgrounds/Switch/switchBlockers", new Vector2(400, 400)));

            objects.AddImpassable(new SwitchElectricity(new Vector2(400, 400)));

            if (typeOfGame is Assasssins)
            {
                //spawn points
                spawnplaces.Add(new SpawnPoint(new Vector2(300, 36), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(770, 480), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(380, 720), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(90, 246), NPC.AffliationTypes.grey, managers));
            }

            else if (typeOfGame is CaptureTheFlag)
            {
                //set up gametype
                var test = (CaptureTheFlag) typeOfGame;
                var tempBases = new List<CTFBase>();

                //Set up bases
                tempBases.Add(new CTFBase(test.GetTeams()[0], new Vector2(96, 160), managers));
                tempBases.Add(new CTFBase(test.GetTeams()[1], new Vector2(640, 704), managers));
                test.Initialize(managers, tempBases);

                //sniper positions
                //to be done

                //spawn points
                //team one
                spawnplaces.Add(new SpawnPoint(new Vector2(300, 36), test.GetTeams()[0], managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(90, 246), test.GetTeams()[0], managers));
                spawnplaces.Add(new SpawnPoint(test.GetAllyBase(test.GetTeams()[0]).GetOriginPosition(),
                                               test.GetTeams()[0], managers));
                //team two
                spawnplaces.Add(new SpawnPoint(new Vector2(770, 480), test.GetTeams()[1], managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(380, 720), test.GetTeams()[1], managers));
                spawnplaces.Add(new SpawnPoint(test.GetAllyBase(test.GetTeams()[1]).GetOriginPosition(),
                                               test.GetTeams()[1], managers));
            }
        }

        public override String ToString()
        {
            return "Switch";
        }
    }
}