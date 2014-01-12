using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DotWars
{
    public class Level
    {
        #region Declarations

        //stating whatever is needed
        //Managers
        private AbilityManager abilities;
        protected NPCManager agents;
        protected BackgroundManager backgrounds;
        protected CameraManager cameras;
        protected EnvironmentManager objects;
        private ParticleManager particles;
        protected ProjectileManager projectiles;
        protected AudioManager sounds;
        protected TextureManager textures;
        protected StatisticsManager statistics;

        //Helpers
        protected ManagerHelper managers;
        protected PathHelper paths;
        protected SpawnHelper spawns;

        //Others
        private Vector2 SizeOfLevel;
        protected List<SpawnPoint> spawnplaces;
        protected List<SuicideSpawnPoint> suicideSpawnplaces;
        protected List<Vector2> sniperSpots;
        protected Gametype typeOfGame;

        private bool initialized;
        private bool loaded;

        #endregion

        #region Defaults

        public static Vector2 DEFAUT_SCREEN_SIZE = new Vector2(1248, 720);

        #endregion

        /*PURPOSE: This public class is the Level public class. As all levels "do" the same things, with perhaps a few exceptions, there is no need 
               to repeat code. Thus this public class will serve as a template for all "real" levels to be based on. Thus whatever this 
               public class can do, the "real" levels can too.
        */

        protected Level(Gametype gT, Dictionary<Type, int> pL, Vector2 lS, TextureManager tM, AudioManager audio)
        {
            //Set up gametype
            typeOfGame = gT;
            cameras = new CameraManager(pL, gT, DEFAUT_SCREEN_SIZE);

            SizeOfLevel = lS;

            sniperSpots = new List<Vector2>();

            textures = tM;

            sounds = audio;

            initialized = false;
            loaded = false;
        }

        public virtual void Initialize() //actually setting up needed materials
        {
            //Initialize Managers (the thing that will hold all textures used)
            agents = new NPCManager();
            projectiles = new ProjectileManager(400, 20, 15);
            objects = new EnvironmentManager();
            backgrounds = new BackgroundManager();
            spawnplaces = new List<SpawnPoint>();
            suicideSpawnplaces = new List<SuicideSpawnPoint>();
            paths = new PathHelper();
            spawns = new SpawnHelper(spawnplaces, suicideSpawnplaces);
            particles = new ParticleManager(200, 40, 40, 20);
            abilities = new AbilityManager(3, 25, 10, 20, managers);
            statistics = new StatisticsManager(typeOfGame.GetTeams());            

            //NOTE: Upper level creates gametype & cameraManager

            //Once all the managers and helpers are done, initalize ManagerHelper
            managers = new ManagerHelper(textures, agents, projectiles, objects, cameras, typeOfGame, this, paths,
                                         sounds, particles, spawns, abilities, statistics);

            //Pass the new ManagerHelper to all the other Managers
            agents.Initialize(managers);
            projectiles.Initialize(managers);
            objects.Initialize(managers);
            typeOfGame.Initialize(managers);
            paths.Initialize(managers, 10);
            particles.Initialize(managers);
            abilities.Initialize(managers);
            backgrounds.Initialize(managers);
            spawns.Intialize(managers);
            statistics.Intitialize(managers);

            initialized = true;
        }

        public virtual void LoadContent(ContentManager cM)
        {
            //Load content for other Managers
            backgrounds.LoadContent(textures);
            cameras.LoadContent(managers);
            paths.LoadContent(textures);
            abilities.LoadContent(managers);
            projectiles.LoadContent();

            loaded = true;
        }

        //Where the objects will be updated (this means any actions that they can do can actually be done)
        public virtual Level Update(GameTime gT)
        {
            managers.Update(gT, Keyboard.GetState());

            cameras.UpdateStateBefore();

            if (cameras.GetPauser() == null)
            {
                //End game if the game is ended
                if (typeOfGame.Update(managers))
                {
                    return new ResultsScreen(this, textures, sounds);
                }

                agents.Update();
                projectiles.Update();
                objects.Update();
                paths.Update();
                spawns.Update();
                particles.Update();
                abilities.Update();
                sounds.Update();
                backgrounds.Update();
                statistics.Update(managers);
            }
            else
            {
                if (cameras.GetCameras()[cameras.GetPauserIndex()].GetState().IsButtonDown(Buttons.Back))
                {
                    return new Menu(textures, sounds);
                }
            }

            cameras.Update(managers);
            cameras.UpdateStateAfter();

            return this;
        }

        //Does exactly as the method is named, draws the objects on the screen
        public virtual void Draw(SpriteBatch sB, GraphicsDeviceManager gM, bool drawHUD)
        {
            foreach (CameraManager.Camera c in cameras.GetCameras())
            {
                managers.SetCurrentCam(c);
                gM.GraphicsDevice.Viewport = c.port;
                Vector2 displacement = cameras.GetDisplacement(c.commanderType);

                sB.Begin();

                backgrounds.DrawBackgrounds(sB, displacement);
                paths.Draw(sB, displacement);
                abilities.DrawBottom(sB, displacement);
                objects.DrawBottom(sB, displacement);
                particles.DrawBottom(sB, displacement);
                typeOfGame.DrawBottom(sB, displacement);
                
                projectiles.Draw(sB, displacement);
                agents.DrawLowest(sB, displacement);
                
                abilities.DrawTop(sB, displacement);
                spawns.Draw(sB, displacement);
                particles.DrawTop(sB, displacement);
                objects.DrawTop(sB, displacement);
                typeOfGame.DrawTop(sB, displacement);
                agents.DrawHighest(sB, displacement);
                backgrounds.Drawforegrounds(sB, displacement);
                
                if (drawHUD && cameras.GetPauser() == null)
                {
                    cameras.GetHud(c).Draw(sB, managers);
                }
                sB.End();
            }

            gM.GraphicsDevice.Viewport = cameras.GetFullSize();

            sB.Begin();
            cameras.DrawSplit(sB, managers);

            if (drawHUD && cameras.GetPauser() == null)
            {
                cameras.GetHud(cameras.GetCameras()[0]).DrawScores(sB, managers);
            }

            cameras.DrawPause(sB, managers);
            sB.End();
        }

        #region Gets
        public List<SpawnPoint> GetSpawnPoints()
        {
            return spawnplaces;
        }

        public List<Vector2> GetSniperSpots()
        {
            return sniperSpots;
        }

        public ManagerHelper GetManagerHelper()
        {
            return managers;
        }

        public Vector2 GetSizeOfLevel()
        {
            return SizeOfLevel;
        }

        public bool HasInitialized()
        {
            return initialized;
        }

        public bool HasLoaded()
        {
            return loaded;
        }
        #endregion

        public override String ToString()
        {
            return "";
        }
    }
}