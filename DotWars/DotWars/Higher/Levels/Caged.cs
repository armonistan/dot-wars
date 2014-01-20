#region

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

#endregion

namespace DotWars
{
    public class Caged : Level
    {
        public Caged(Gametype gT, Dictionary<Type, int> pL, TextureManager tM, AudioManager audio) :
            base(gT, pL, new Vector2(864, 544), tM, audio)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            //Set up background
            backgrounds.AddBackground(new Lava(new Vector2(232, 272), new Vector2(1, 0)));
            backgrounds.AddBackground(new Lava(new Vector2(432, 272), new Vector2(1, 0)));
            backgrounds.AddBackground(new Sprite("Backgrounds/Caged/cagedBackground", new Vector2(432, 272)));
        }

        public override void LoadContent(ContentManager cM)
        {
            base.LoadContent(cM);

            objects.AddStaticBlocker(new InDestructable("Backgrounds/Caged/cagedBlockers", new Vector2(432, 272)));
            objects.AddImpathable(new LavaBurner(new Vector2(432, 272)));

            if (typeOfGame is Assassins)
            {
                spawnplaces.Add(new SpawnPoint(new Vector2(300, 265), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(615, 265), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(155, 80), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(765, 80), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(115, 465), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(710, 465), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(730, 265), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(150, 265), NPC.AffliationTypes.grey, managers));
            }
            else if (typeOfGame is Survival)
            {
                var temp = (Survival) typeOfGame;

                var tempClaimables = new List<Claimable>();
                tempClaimables.Add(new Claimable(new Vector2(107, 181)));
                tempClaimables.Add(new Claimable(new Vector2(433, 43)));
                tempClaimables.Add(new Claimable(new Vector2(433, 510)));
                tempClaimables.Add(new Claimable(new Vector2(784, 371)));

                spawnplaces.Add(new SpawnPoint(new Vector2(316, 165), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(554, 173), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(563, 347), NPC.AffliationTypes.grey, managers));
                spawnplaces.Add(new SpawnPoint(new Vector2(312, 343), NPC.AffliationTypes.grey, managers));

                suicideSpawnplaces.Add(new SuicideSpawnPoint(new Vector2(16, 16), managers));
                suicideSpawnplaces.Add(new SuicideSpawnPoint(new Vector2(16, 528), managers));
                suicideSpawnplaces.Add(new SuicideSpawnPoint(new Vector2(800, 16), managers));
                suicideSpawnplaces.Add(new SuicideSpawnPoint(new Vector2(800, 528), managers));

                spawns.LoadTextures(textures);

                temp.Initialize(managers, tempClaimables);
            }
        }

        public override Level Update(GameTime gT)
        {
            return base.Update(gT);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sB, GraphicsDeviceManager gM,
                                  bool drawHUD)
        {
            foreach (CameraManager.Camera c in cameras.GetCameras())
            {
                managers.SetCurrentCam(c);
                gM.GraphicsDevice.Viewport = c.port;
                Vector2 displacement = cameras.GetDisplacement(c.commanderType);
            }

            base.Draw(sB, gM, drawHUD);
        }

        public override String ToString()
        {
            return "Caged";
        }
    }
}