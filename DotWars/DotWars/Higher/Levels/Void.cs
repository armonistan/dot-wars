using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DotWars
{
    public class Void : Level
    {
        #region Declarations

        #endregion

        public Void(Gametype gT, Dictionary<Type, int> pL, TextureManager tM, AudioManager audio) :
            base(gT, pL, new Vector2(1312, 992), tM, audio)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            textures.Add("Backgrounds/Plaza/plazaBkg", new Rectangle(0, 0, 1312, 992));

            //Set up background
            backgrounds.AddBackground(new Sprite("Backgrounds/Plaza/plazaBkg", new Vector2(656, 496)));
        }

        public override void LoadContent(ContentManager cM)
        {
            base.LoadContent(cM);

            //Creates a basic spawn based on gametype
            if (typeOfGame is Assasssins || typeOfGame is Deathmatch)
            {
                //agents.Add(new RedCommander(new Vector2(300, 36)));
                //agents.Add(new BlueCommander(new Vector2(300, 600)));
                //agents.Add(new RedCommander(new Vector2(300, 36)));
                //agents.Add(new BlueCommander(new Vector2(300, 600)));
                //agents.Add(new RedCommander(new Vector2(300, 36)));
                //agents.Add(new BlueCommander(new Vector2(300, 600)));
                //agents.Add(new RedCommander(new Vector2(300, 36)));
                //agents.Add(new RedCommander(new Vector2(300, 36)));
                //agents.Add(new BlueCommander(new Vector2(300, 600)));
                //agents.Add(new BlueCommander(new Vector2(300, 600)));
                //agents.Add(new RedCommander(new Vector2(300, 36)));
                //agents.Add(new BlueCommander(new Vector2(300, 600)));
                //agents.Add(new RedCommander(new Vector2(300, 36)));
                //agents.Add(new BlueCommander(new Vector2(300, 600)));
                //agents.Add(new RedCommander(new Vector2(300, 36)));
                //agents.Add(new BlueCommander(new Vector2(300, 600)));
                //agents.Add(new RedCommander(new Vector2(300, 36)));
                //agents.Add(new RedCommander(new Vector2(300, 36)));
                //agents.Add(new BlueCommander(new Vector2(300, 600)));
                //agents.Add(new BlueCommander(new Vector2(300, 600)));
                //agents.Add(new GreenCommander(new Vector2(300, 500)));
                //agents.Add(new YellowCommander(new Vector2(500, 224)));

                agents.Add(new RedGrunt(new Vector2(260, 56)));
                agents.Add(new RedGrunt(new Vector2(260, 56)));
                agents.Add(new RedGrunt(new Vector2(260, 56)));
                agents.Add(new RedGrunt(new Vector2(260, 56)));
                agents.Add(new RedGrunt(new Vector2(260, 56)));
                agents.Add(new BlueGrunt(new Vector2(700, 256)));
                agents.Add(new BlueGrunt(new Vector2(700, 256)));
                agents.Add(new BlueGrunt(new Vector2(700, 256)));
                agents.Add(new BlueGrunt(new Vector2(700, 256)));
                agents.Add(new RedGrunt(new Vector2(260, 56)));
                agents.Add(new RedGrunt(new Vector2(260, 56)));
                agents.Add(new BlueGrunt(new Vector2(700, 256)));
                agents.Add(new BlueGrunt(new Vector2(700, 256)));
                agents.Add(new BlueGrunt(new Vector2(700, 256)));


                //Set up spawn
            }

            if (typeOfGame is Deathmatch)
            {
                //Set up claimables
                var test = (Deathmatch) typeOfGame;
                var tempClaimables = new List<Claimable>();
                tempClaimables.Add(new Claimable(new Vector2(250, 100)));
                tempClaimables.Add(new Claimable(new Vector2(570, 200)));
                //tempClaimables.Add(new Claimable(new Vector2(98, 300)));
                //tempClaimables.Add(new Claimable(new Vector2(320, 400)));
                //tempClaimables.Add(new Claimable(new Vector2(090, 150)));
                //tempClaimables.Add(new Claimable(new Vector2(120, 250)));
                //tempClaimables.Add(new Claimable(new Vector2(340, 350)));
                //tempClaimables.Add(new Claimable(new Vector2(120, 450)));
                test.Initialize(managers, tempClaimables);

                agents.Add(new RedGrunt(new Vector2(0, 56)));

                //the red army
                //agents.Add(new RedGrunt(new Vector2(0, 56)));
                //agents.Add(new RedGrunt(new Vector2(32, 56)));
                //agents.Add(new RedGunner(new Vector2(260, 56)));
                //agents.Add(new RedBombardier(new Vector2(128, 56)));
                //agents.Add(new RedSniper(new Vector2(64, 56)));
                //agents.Add(new RedJuggernaut(new Vector2(96, 36)));
                //agents.Add(new RedSpecialist(new Vector2(255, 36)));
                //agents.Add(new RedMedic(new Vector2(192, 36)));

                //the blue army
                //agents.Add(new BlueGrunt(new Vector2(700, 0)));
                //agents.Add(new BlueGrunt(new Vector2(700, 32)));
                //agents.Add(new BlueGunner(new Vector2(700, 256)));
                //agents.Add(new BlueBombardier(new Vector2(700, 64)));
                //agents.Add(new BlueSniper(new Vector2(700, 96)));
                //agents.Add(new BlueJuggernaut(new Vector2(700, 128)));
                //agents.Add(new BlueSpecialist(new Vector2(700, 160)));
                //agents.Add(new BlueMedic(new Vector2(700, 192)));

                //the green army
                //agents.Add(new GreenGrunt(new Vector2(100, 500)));
                //agents.Add(new GreenGrunt(new Vector2(32, 500)));
                //agents.Add(new GreenGunner(new Vector2(350, 500)));
                //agents.Add(new GreenBombardier(new Vector2(128, 500)));
                //agents.Add(new GreenSniper(new Vector2(64, 500)));
                //agents.Add(new GreenJuggernaut(new Vector2(96, 500)));
                //agents.Add(new GreenSpecialist(new Vector2(255, 500)));
                //agents.Add(new GreenMedic(new Vector2(192, 500)));

                //the yellow army
                //agents.Add(new YellowGrunt(new Vector2(500, 0)));
                //agents.Add(new YellowGrunt(new Vector2(500, 32)));
                //agents.Add(new YellowGunner(new Vector2(500, 256)));
                //agents.Add(new YellowBombardier(new Vector2(500, 64)));
                //agents.Add(new YellowSniper(new Vector2(500, 96)));
                //agents.Add(new YellowJuggernaut(new Vector2(500, 128)));
                //agents.Add(new YellowSpecialist(new Vector2(500, 160)));
                //agents.Add(new YellowMedic(new Vector2(500, 192)));

                spawnplaces.Add(new SpawnPoint(new Vector2(160, 672), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(288, 544), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(448, 672), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(448, 352), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(800, 384), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(736, 64), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(992, 160), NPC.AffliationTypes.grey, managers));
            }

            if (typeOfGame is Conquest)
            {
                agents.Add(new RedCommander(new Vector2(100, 36)));
                agents.Add(new BlueCommander(new Vector2(500, 224)));
                //agents.Add(new GreenCommander(new Vector2(0, 0)));
                //agents.Add(new YellowCommander(new Vector2(500, 500)));

                //Set up bases
                var test = (Conquest) typeOfGame;
                var tempBases = new List<ConquestBase>();
                tempBases.Add(new ConquestBase(new Vector2(100, 200)));
                tempBases.Add(new ConquestBase(new Vector2(500, 200)));
                tempBases.Add(new ConquestBase(new Vector2(100, 400)));
                tempBases.Add(new ConquestBase(new Vector2(500, 400)));
                test.Initialize(managers, tempBases);
            }

            if (typeOfGame is Survival)
            {
                agents.Add(new RedCommander(new Vector2(1216, 608)));
                agents.Add(new BlueCommander(new Vector2(60, 900)));


                var temp = (Survival) typeOfGame;
                var tempClaimables = new List<Claimable>();

                tempClaimables.Add(new Claimable(new Vector2(250, 100)));
                tempClaimables.Add(new Claimable(new Vector2(570, 200)));
                tempClaimables.Add(new Claimable(new Vector2(98, 300)));


                temp.Initialize(managers, tempClaimables);
            }

            if (typeOfGame is CaptureTheFlag)
            {
                var temp = (CaptureTheFlag) typeOfGame;
                var tempBases = new List<CTFBase>();
                tempBases.Add(new CTFBase(NPC.AffliationTypes.red, new Vector2(100, 100), managers));
                tempBases.Add(new CTFBase(NPC.AffliationTypes.blue, new Vector2(780, 600), managers));
                temp.Initialize(managers, tempBases);

                agents.Add(new RedCommander(new Vector2(300)));
                //agents.Add(new RedGrunt(new Vector2(0, 56)));
                //agents.Add(new RedGrunt(new Vector2(32, 56)));


                agents.Add(new BlueCommander(new Vector2(350)));
            }

            if (typeOfGame is Assault)
            {
                var temp = (Assault) typeOfGame;
                var tempBases = new List<AssaultBase>();
                tempBases.Add(new AssaultBase(NPC.AffliationTypes.red, new Vector2(32, 100), managers));
                tempBases.Add(new AssaultBase(NPC.AffliationTypes.blue, new Vector2(580, 400), managers));
                temp.Initialize(managers, tempBases);
            }
        }

        public override Level Update(GameTime gT)
        {
            return base.Update(gT);
        }

        public override void Draw(SpriteBatch sB, GraphicsDeviceManager gM)
        {
            base.Draw(sB, gM);
            //TODO: Level specific
        }

        public override String ToString()
        {
            return "Void";
        }
    }
}