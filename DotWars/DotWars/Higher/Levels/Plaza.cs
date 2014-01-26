#region

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

#endregion

namespace DotWars
{
    public class Plaza : Level
    {
        public Plaza(Gametype gT, Dictionary<Type, int> pL, TextureManager tM, AudioManager audio) :
            base(gT, pL, new Vector2(1328, 1008), tM, audio)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            backgrounds.AddForeground(new Sprite("Backgrounds/Plaza/plazaLighting", new Vector2(664, 504)));
            backgrounds.AddBackground(new Sprite("Backgrounds/Plaza/plazaBackground", new Vector2(664, 504)));

            sniperSpots.Add(new Vector2(80, 270));
            sniperSpots.Add(new Vector2(930, 100));
            sniperSpots.Add(new Vector2(930, 950));
            sniperSpots.Add(new Vector2(1280, 270));
        }

        public override void LoadContent(ContentManager cM)
        {
            base.LoadContent(cM);

            objects.AddStaticBlocker(new InDestructable("Backgrounds/Plaza/plazaBlockers",
                                                        new Vector2(664, 504)));
            objects.AddTopObject(new AnimatedLight(new Vector2(516, 328), 0));
            objects.AddTopObject(new AnimatedLight(new Vector2(872, 326), MathHelper.Pi*.25f));
            objects.AddTopObject(new AnimatedLight(new Vector2(872, 656), MathHelper.Pi*.5f));

            if (typeOfGame is Assault)
            {
                //Set up bases
                var temp = (Assault) typeOfGame;
                var tempBases = new List<AssaultBase>();
                //attacker
                tempBases.Add(new AssaultBase(temp.GetTeams()[0], new Vector2(1132, 110), managers));
                //defender
                tempBases.Add(new AssaultBase(temp.GetTeams()[1], new Vector2(90+128, 905-128), managers));

                //intitialize bases
                temp.Initialize(managers, tempBases);

                //sniper positions
                sniperSpots.Add(new Vector2(1290, 930));
                sniperSpots.Add(new Vector2(200, 160));
                sniperSpots.Add(new Vector2(1230, 155));
                sniperSpots.Add(new Vector2(195, 860));

                //set up spawns
                //team one (attackers) spawns
                spawnplaces.Add(new SpawnPoint(temp.GetAllyBase(temp.GetAttacker()).GetOriginPosition(),
                                               temp.GetTeams()[0], managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(1050, 150), temp.GetTeams()[0], managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(1290, 405), temp.GetTeams()[0], managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(520, 120), temp.GetTeams()[0], managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(1140, 670), temp.GetTeams()[0], managers));

                //team two (defenders) spawns
                spawnplaces.Add(new SpawnPoint(temp.GetAllyBase(temp.GetDefender()).GetOriginPosition(),
                                               temp.GetTeams()[1], managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(100, 920), temp.GetTeams()[1], managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(100, 445), temp.GetTeams()[1], managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(550, 960), temp.GetTeams()[1], managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(445, 565), temp.GetTeams()[1], managers));
            }

            else if (typeOfGame is Survival)
            {
                var temp = (Survival) typeOfGame;

                var tempClaimables = new List<Claimable>();
                tempClaimables.Add(new Claimable(new Vector2(930, 270)));
                tempClaimables.Add(new Claimable(new Vector2(1050, 785)));
                tempClaimables.Add(new Claimable(new Vector2(515, 205)));
                tempClaimables.Add(new Claimable(new Vector2(380, 630)));

                spawnplaces.Add(new SpawnPoint(new Vector2(60, 945), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(60, 860), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(155, 945), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(155, 860), NPC.AffliationTypes.grey, managers));

                suicideSpawnplaces.Add(new SuicideSpawnPoint(new Vector2(520, 120), managers));
                suicideSpawnplaces.Add(new SuicideSpawnPoint(new Vector2(1050, 155), managers));
                suicideSpawnplaces.Add(new SuicideSpawnPoint(new Vector2(200, 160), managers));
                suicideSpawnplaces.Add(new SuicideSpawnPoint(new Vector2(1290, 405), managers));

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