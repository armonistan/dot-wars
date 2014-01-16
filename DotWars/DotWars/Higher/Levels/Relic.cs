#region

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

#endregion

namespace DotWars
{
    public class Relic : Level
    {
        #region Declarations

        #endregion

        public Relic(Gametype gT, Dictionary<Type, int> pL, TextureManager tM, AudioManager audio) :
            base(gT, pL, new Vector2(1088, 864), tM, audio)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            //Set up background
            backgrounds.AddBackground(new Sprite("Backgrounds/Relic/relicBackground", new Vector2(544, 432)));

            //Sniper spots
            sniperSpots.Add(new Vector2(256, 64));
            sniperSpots.Add(new Vector2(1024, 64));
            sniperSpots.Add(new Vector2(64, 800));
            sniperSpots.Add(new Vector2(832, 800));
            sniperSpots.Add(new Vector2(192, 480));
            sniperSpots.Add(new Vector2(896, 416));
        }

        public override void LoadContent(ContentManager cM)
        {
            base.LoadContent(cM);

            objects.AddStaticBlocker(new Meteor(new Vector2(544, 432)));
            objects.AddStaticBlocker(new InDestructable("Backgrounds/Relic/relicBlockers", new Vector2(544, 432)));

            if (typeOfGame is CaptureTheFlag)
            {
                var tempCTF = (CaptureTheFlag) typeOfGame;
                var tempBases = new List<CTFBase>();
                tempBases.Add(new CTFBase(tempCTF.GetTeams()[0], new Vector2(64, 64), managers));
                tempBases.Add(new CTFBase(tempCTF.GetTeams()[1], new Vector2(1024, 800), managers));
                tempCTF.Initialize(managers, tempBases);

                spawnplaces.Add(new SpawnPoint(tempBases[0].GetOriginPosition(), tempCTF.GetTeams()[0], managers));
                spawnplaces.Add(new SpawnPoint(tempBases[1].GetOriginPosition(), tempCTF.GetTeams()[1], managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(64, 128), tempCTF.GetTeams()[0], managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(416, 96), tempCTF.GetTeams()[0], managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(1024, 736), tempCTF.GetTeams()[1], managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(736, 736), tempCTF.GetTeams()[1], managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(224, 448), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(928, 480), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(736, 96), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(288, 768), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(64, 800), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(1024, 64), NPC.AffliationTypes.grey, managers));
            }
            else if (typeOfGame is Deathmatch)
            {
                var tempDM = (Deathmatch) typeOfGame;
                var tempClaimables = new List<Claimable>();

                tempClaimables.Add(new Claimable(new Vector2(256, 64)));
                tempClaimables.Add(new Claimable(new Vector2(832, 800)));
                tempClaimables.Add(new Claimable(new Vector2(224, 448)));
                tempClaimables.Add(new Claimable(new Vector2(928, 480)));
                tempClaimables.Add(new Claimable(new Vector2(736, 96)));
                tempClaimables.Add(new Claimable(new Vector2(288, 768)));
                /*tempClaimables.Add(new Claimable(new Vector2(, )));
                tempClaimables.Add(new Claimable(new Vector2(, )));*/

                tempDM.Initialize(managers, tempClaimables);

                spawnplaces.Add(new SpawnPoint(new Vector2(64, 128), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(416, 96), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(1024, 736), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(736, 736), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(64, 800), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(1024, 64), NPC.AffliationTypes.grey, managers));
            }
        }

        public override String ToString()
        {
            return "Relic";
        }
    }
}