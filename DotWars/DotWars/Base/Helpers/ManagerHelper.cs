using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DotWars
{
    public class ManagerHelper
    {
        #region Declarations

        private readonly AbilityManager abilities;
        private readonly NPCManager agents; // Holds the NPCs
        private readonly CameraManager cameras;
        private readonly EnvironmentManager objects; //Holds the environments and effects
        private readonly ParticleManager particles;
        private readonly PathHelper paths;
        private readonly ProjectileManager projectiles; // Holds the projectiles
        private readonly Random rand;
        private readonly AudioManager sounds;
        private readonly SpawnHelper spawns;
        private readonly TextureManager textures; // Holds the textureManager
        private readonly Level theLevel;
        private readonly Gametype typeOfGame; //Used to change scores
        private CameraManager.Camera currentCam;
        private KeyboardState currentState;
        private StatisticsManager statistics;
        private GameTime time; //Used to get the current gametime from anywhere
        private float deltaSeconds;

        #endregion

        public ManagerHelper(TextureManager tM, NPCManager nM, ProjectileManager pM, EnvironmentManager eM,
                             CameraManager cM, Gametype tG, Level l, PathHelper pH, AudioManager aM, ParticleManager ptM,
                             SpawnHelper sH, AbilityManager abM, StatisticsManager sM)
        {
            textures = tM;
            agents = nM;
            projectiles = pM;
            objects = eM;
            cameras = cM;
            theLevel = l;
            paths = pH;
            sounds = aM;
            particles = ptM;
            spawns = sH;
            abilities = abM;
            statistics = sM;

            typeOfGame = tG;
            rand = new Random();
        }

        public void Update(GameTime gT, KeyboardState kS)
        {
            time = gT;
            deltaSeconds = (float) gT.ElapsedGameTime.TotalSeconds;

            currentState = kS;
        }

        public PathHelper GetPathHelper()
        {
            return paths;
        }

        public AudioManager GetAudioManager()
        {
            return sounds;
        }

        public ParticleManager GetParticleManager()
        {
            return particles;
        }

        public SpawnHelper GetSpawnHelper()
        {
            return spawns;
        }

        public AbilityManager GetAbilityManager()
        {
            return abilities;
        }

        #region Misc Methods

        public GameTime GetGameTime()
        {
            return time;
        }

        public float GetDeltaSeconds()
        {
            return deltaSeconds;
        }

        public Gametype GetGametype()
        {
            return typeOfGame;
        }

        public Random GetRandom()
        {
            return rand;
        }

        public Level GetLevel()
        {
            return theLevel;
        }

        public KeyboardState GetCurrentState()
        {
            return currentState;
        }

        public Vector2 GetLevelSize()
        {
            return theLevel.GetSizeOfLevel();
        }

        #endregion

        public TextureManager GetTextureManager()
        {
            return textures;
        }

        public NPCManager GetNPCManager()
        {
            return agents;
        }

        public ProjectileManager GetProjectileManager()
        {
            return projectiles;
        }

        public EnvironmentManager GetEnvironmentManager()
        {
            return objects;
        }

        public CameraManager GetCameraManager()
        {
            return cameras;
        }

        public CameraManager.Camera GetCurrentCam()
        {
            return currentCam;
        }

        public StatisticsManager GetStatisticsManager() 
        {
            return statistics;
        }

        public void SetCurrentCam(CameraManager.Camera c)
        {
            currentCam = c;
        }
    }
}