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

        //Helpers
        protected ManagerHelper managers;
        protected PathHelper paths;
        protected SpawnHelper spawns;

        //Others
        private Vector2 SizeOfLevel;
        protected List<SpawnPoint> spawnplaces;
        protected List<Vector2> sniperSpots;
        protected Gametype typeOfGame;

        #endregion

        #region Defaults

        public Vector2 DEFAUT_SCREEN_SIZE = new Vector2(1248, 720);

        #endregion

        /*PURPOSE: This public class is the Level public class. As all levels "do" the same things, with perhaps a few exceptions, there is no need 
               to repeat code. Thus this public class will serve as a template for all "real" levels to be based on. Thus whatever this 
               public class can do, the "real" levels can too.
        */

        protected Level(Gametype gT, Dictionary<Type, int> pL, Vector2 lS)
        {
            //Set up gametype
            typeOfGame = gT;
            cameras = new CameraManager(pL, DEFAUT_SCREEN_SIZE);

            SizeOfLevel = lS;

            sniperSpots = new List<Vector2>();
        }

        public virtual void Initialize() //actually setting up needed materials
        {
            //Initialize Managers (the thing that will hold all textures used)
            textures = new TextureManager();
            agents = new NPCManager();
            projectiles = new ProjectileManager(1000, 100, 100);
            objects = new EnvironmentManager();
            backgrounds = new BackgroundManager();
            spawnplaces = new List<SpawnPoint>();
            paths = new PathHelper();
            sounds = new AudioManager();
            spawns = new SpawnHelper(spawnplaces);
            particles = new ParticleManager(1000, 200, 200, 50);
            abilities = new AbilityManager(5, 20, 3, 10);

            //NOTE: Upper level creates gametype & cameraManager

            //Once all the managers and helpers are done, initalize ManagerHelper
            managers = new ManagerHelper(textures, agents, projectiles, objects, cameras, typeOfGame, this, paths,
                                         sounds, particles, spawns, abilities);

            //Pass the new ManagerHelper to all the other Managers
            agents.Initialize(managers);
            projectiles.Initialize(managers);
            objects.Initialize(managers);
            typeOfGame.Initialize(managers);
            paths.Initialize(managers, 10);
            particles.Initialize(managers);
            abilities.Initialize(managers);
            backgrounds.Initialize(managers);

            #region Default Textures

            textures.Add("Dots/Red/grunt_red", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Red/bombardier_red", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Red/specialist_red", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Red/juggernaut_red", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Red/sniper_red", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Red/medic_red", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Red/gunner_red", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Red/commander_red", new Rectangle(0, 0, 32, 32));

            textures.Add("Dots/Blue/grunt_blue", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Blue/bombardier_blue", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Blue/specialist_blue", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Blue/juggernaut_blue", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Blue/sniper_blue", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Blue/medic_blue", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Blue/gunner_blue", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Blue/commander_blue", new Rectangle(0, 0, 32, 32));

            textures.Add("Dots/Green/grunt_green", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Green/bombardier_green", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Green/specialist_green", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Green/juggernaut_green", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Green/sniper_green", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Green/medic_green", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Green/gunner_green", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Green/commander_green", new Rectangle(0, 0, 32, 32));

            textures.Add("Dots/Yellow/grunt_yellow", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Yellow/bombardier_yellow", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Yellow/specialist_yellow", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Yellow/juggernaut_yellow", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Yellow/sniper_yellow", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Yellow/medic_yellow", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Yellow/gunner_yellow", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Yellow/commander_yellow", new Rectangle(0, 0, 32, 32));

            textures.Add("Dots/Red/bomber_red", new Rectangle(0, 0, 96, 64));
            textures.Add("Dots/Blue/bomber_blue", new Rectangle(0, 0, 96, 64));
            textures.Add("Dots/Green/bomber_green", new Rectangle(0, 0, 96, 64));
            textures.Add("Dots/Yellow/bomber_yellow", new Rectangle(0, 0, 96, 64));

            textures.Add("Projectiles/bullet_standard", new Rectangle(0, 0, 6, 7));
            textures.Add("Projectiles/bullet_sniper", new Rectangle(0, 0, 6, 7));
            textures.Add("Projectiles/bullet_rocket", new Rectangle(0, 0, 6, 7));
            textures.Add("Projectiles/bullet_shotgun", new Rectangle(0, 0, 6, 7));

            textures.Add("Projectiles/grenade", new Rectangle(0, 0, 15, 15));
            textures.Add("Projectiles/bullet_bombs", new Rectangle(0, 0, 14, 5));

            textures.Add("Effects/particle_smoke", new Rectangle(0, 0, 8, 8));
            textures.Add("Effects/smoke_red", new Rectangle(0, 0, 9, 9));
            textures.Add("Effects/smoke_blue", new Rectangle(0, 0, 9, 9));
            textures.Add("Effects/smoke_green", new Rectangle(0, 0, 9, 9));
            textures.Add("Effects/smoke_yellow", new Rectangle(0, 0, 9, 9));
            textures.Add("Effects/smoke_standard", new Rectangle(0, 0, 30, 30));

            textures.Add("Effects/spark_red", new Rectangle(0, 0, 5, 5));
            textures.Add("Effects/spark_blue", new Rectangle(0, 0, 5, 5));
            textures.Add("Effects/spark_green", new Rectangle(0, 0, 5, 5));
            textures.Add("Effects/spark_yellow", new Rectangle(0, 0, 5, 5));

            textures.Add("Effects/particle_blood_red", new Rectangle(0, 0, 8, 8));
            textures.Add("Effects/particle_blood_blue", new Rectangle(0, 0, 8, 8));
            textures.Add("Effects/particle_blood_green", new Rectangle(0, 0, 8, 8));
            textures.Add("Effects/particle_blood_yellow", new Rectangle(0, 0, 8, 8));
            textures.Add("Effects/particle_fire", new Rectangle(0, 0, 14, 14));
            textures.Add("Effects/particle_heal", new Rectangle(0, 0, 16, 16));

            textures.Add("Effects/PI_redCommander", new Rectangle(0, 0, 64, 64));
            textures.Add("Effects/PI_blueCommander", new Rectangle(0, 0, 64, 64));
            textures.Add("Effects/PI_greenCommander", new Rectangle(0, 0, 64, 64));
            textures.Add("Effects/PI_yellowCommander", new Rectangle(0, 0, 64, 64));

            textures.Add("Dots/Grey/grey_claimable", new Rectangle(0, 0, 36, 36));
            textures.Add("Dots/Grey/grey_suicide", new Rectangle(0, 0, 32, 32));
            textures.Add("health_station", new Rectangle(0, 0, 64, 64));

            textures.Add("Projectiles/flare_red", new Rectangle(0, 0, 15, 15));
            textures.Add("Projectiles/flare_blue", new Rectangle(0, 0, 15, 15));
            textures.Add("Projectiles/flare_green", new Rectangle(0, 0, 15, 15));
            textures.Add("Projectiles/flare_yellow", new Rectangle(0, 0, 15, 15));

            textures.Add("Effects/explodeTop", new Rectangle(0, 0, 128, 128));
            textures.Add("bases", new Rectangle(0, 0, 64, 64));
            textures.Add("Objectives/flag_red", new Rectangle(0, 0, 32, 32));
            textures.Add("Objectives/flag_blue", new Rectangle(0, 0, 32, 32));
            textures.Add("Objectives/flag_green", new Rectangle(0, 0, 32, 32));
            textures.Add("Objectives/flag_yellow", new Rectangle(0, 0, 32, 32));

            //HUD elements
            textures.Add("HUD/bar_health", new Rectangle(0, 0, 127, 14));
            textures.Add("HUD/bar_special", new Rectangle(0, 0, 84, 14));
            textures.Add("HUD/hud_background", new Rectangle(0, 0, 240, 60));
            textures.Add("HUD/hud_blue", new Rectangle(0, 0, 240, 60));
            textures.Add("HUD/hud_green", new Rectangle(0, 0, 240, 60));
            textures.Add("HUD/hud_gun", new Rectangle(0, 0, 55, 38));
            textures.Add("HUD/hud_red", new Rectangle(0, 0, 240, 60));
            textures.Add("HUD/hud_toss", new Rectangle(0, 0, 20, 20));
            textures.Add("HUD/hud_yellow", new Rectangle(0, 0, 240, 60));
            textures.Add("HUD/2playerSplit", cameras.GetFullSize().Bounds);
            textures.Add("HUD/4playerSplit", cameras.GetFullSize().Bounds);
            textures.Add("HUD/pauseOverlay", new Rectangle(0, 0, 372, 123));

            //Guts
            textures.Add("Effects/Guts/Red/deadGruntRed", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Red/deadGunnerRed", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Red/deadJuggRed", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Red/deadBombRed", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Red/deadSniperRed", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Red/deadMedicRed", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Red/deadSpecialistRed", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Red/deadCommanderRed", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Blue/deadGruntBlue", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Blue/deadGunnerBlue", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Blue/deadJuggBlue", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Blue/deadBombBlue", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Blue/deadSniperBlue", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Blue/deadMedicBlue", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Blue/deadSpecialistBlue", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Blue/deadCommanderBlue", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Green/deadGruntGreen", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Green/deadGunnerGreen", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Green/deadJuggGreen", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Green/deadBombGreen", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Green/deadSniperGreen", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Green/deadMedicGreen", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Green/deadSpecialistGreen", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Green/deadCommanderGreen", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Yellow/deadGruntYellow", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Yellow/deadGunnerYellow", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Yellow/deadJuggYellow", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Yellow/deadBombYellow", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Yellow/deadSniperYellow", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Yellow/deadMedicYellow", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Yellow/deadSpecialistYellow", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Yellow/deadCommanderYellow", new Rectangle(0, 0, 32, 32));

            //Abilities
            textures.Add("Abilities/ability_red", new Rectangle(0, 0, 64, 64));
            textures.Add("Abilities/ability_green", new Rectangle(0, 0, 64, 64));
            textures.Add("Abilities/ability_blue_splash", new Rectangle(0, 0, 64, 64));
            textures.Add("Abilities/ability_blue_spread2", new Rectangle(0, 0, 64, 64));
            textures.Add("Abilities/yellow_test", new Rectangle(0, 0, 32, 16));

            textures.Add("square", new Rectangle(0, 0, 32, 32));

            #endregion

            #region Default Audio
            //Shooting sounds
            sounds.Add("Guns/standardShoot");
            sounds.Add("Guns/sniperShoot");
            sounds.Add("Guns/shotgunShoot");
            sounds.Add("Guns/staticCall");
            sounds.Add("Guns/machineShoot");
            sounds.Add("Other/planeFly");
            sounds.Add("Other/explosion");
            sounds.Add("Guns/rocketShoot");
            sounds.Add("Guns/flareShoot");
            sounds.Add("Guns/grenadeShoot");
            sounds.Add("Guns/healShoot");

            sounds.Add("Abilities/fireballSound");
            sounds.Add("Abilities/waterSound");
            sounds.Add("Abilities/rockSound");
            sounds.Add("Abilities/sparkSound");

            sounds.Add("Explosion1");
            #endregion
        }

        public virtual void LoadContent(ContentManager cM)
        {
            //Load all of the Textures
            textures.LoadContent(cM);

            //Load content for other Managers
            backgrounds.LoadContent(textures);
            cameras.LoadContent(managers);
            sounds.LoadContent(cM);
            paths.LoadContent(textures);
            abilities.LoadContent(managers);

            //TODO: Make in game music
        }

        //Where the objects will be updated (this means any actions that they can do can actually be done)
        public virtual Level Update(GameTime gT)
        {
            managers.Update(gT, Keyboard.GetState());

            //End game if the game is ended
            if (typeOfGame.Update(managers))
            {
                return new ResultsScreen(this);
            }

            cameras.UpdateStateBefore();

            if (cameras.GetPauser() == null)
            {
                agents.Update();
                projectiles.Update();
                objects.Update();
                paths.Update();
                spawns.Update(managers);
                particles.Update();
                abilities.Update();
                sounds.Update(managers);
            }

            cameras.Update(managers);
            cameras.UpdateStateAfter();

            return this;
        }

        //Does exactly as the method is named, draws the objects on the screen
        public virtual void Draw(SpriteBatch sB, GraphicsDeviceManager gM)
        {
            foreach (CameraManager.Camera c in cameras.GetCameras())
            {
                managers.SetCurrentCam(c);
                gM.GraphicsDevice.Viewport = c.port;
                Vector2 displacement = cameras.GetDisplacement(c.commanderType);

                sB.Begin();

                backgrounds.DrawBackgrounds(sB, displacement);
                paths.Draw(sB, displacement);
                objects.DrawBottom(sB, displacement);
                particles.DrawBottom(sB, displacement);
                typeOfGame.DrawBottom(sB, displacement);

                abilities.DrawBottom(sB, displacement);
                projectiles.Draw(sB, displacement);
                agents.DrawLowest(sB, displacement);

                abilities.DrawTop(sB, displacement);
                particles.DrawTop(sB, displacement);
                objects.DrawTop(sB, displacement);
                typeOfGame.DrawTop(sB, displacement);
                agents.DrawHighest(sB, displacement);
                backgrounds.Drawforegrounds(sB, displacement);
                typeOfGame.DrawHUD(sB);
                cameras.GetHud(c).Draw(sB, managers);
                sB.End();
            }

            gM.GraphicsDevice.Viewport = cameras.GetFullSize();
            sB.Begin();
            cameras.DrawSplit(sB, managers);
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
        #endregion

    }
}