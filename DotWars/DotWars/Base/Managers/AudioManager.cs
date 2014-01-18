#region

using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

#endregion

namespace DotWars
{
    public class AudioManager
    {
        #region Declarations

        private readonly AudioEngine theAudio;
        private WaveBank theWaves;
        private readonly SoundBank theSounds;

        private readonly List<Cue> keptSounds;

        #endregion

        #region Location Constants

        public static string DOT_WARS = "DotWars";
        public static string CONFIRM = "confirm";
        public static string RETURN = "return";

        public static string STANDARD_SHOOT = "standardShoot";
        public static string GRENADE_SHOOT = "grenadeShoot";
        public static string SNIPER_SHOOT = "sniperShoot";
        public static string SPECIALIST_SHOOT = "rocketShoot";
        public static string HEAL_SOUND = "healShoot";
        public static string STATIC = "staticCall";
        public static string PLANE = "planeFly";
        public static string JUGGERNAUT_RICOHET = "juggernautRicohet";

        public static string COMMANDER_SHOOT = "commanderShoot";
        public static string COMMANDER_SHOTGUN = "commanderShotgun";
        public static string COMMANDER_GRENADE = "commanderGrenade";
        public static string COMMANDER_FLARE = "commanderFlare";

        public static string FIREBALL = "fireballSound";
        public static string LARGE_ROCK = "rockSound";
        public static string SPARK = "sparkSound";
        public static string WATER = "waterSound";

        public static string EXPLOSION = "explosion";

        #endregion

        public AudioManager()
        {
            theAudio = new AudioEngine("Content/Audio/GameAudio.xgs");
            theWaves = new WaveBank(theAudio, "Content/Audio/Wave Bank.xwb");
            theSounds = new SoundBank(theAudio, "Content/Audio/Sound Bank.xsb");

            keptSounds = new List<Cue>();
        }

        public void Update()
        {
            theAudio.Update();
        }

        public void KillSounds()
        {
            foreach (Cue sound in keptSounds)
            {
                sound.Stop(AudioStopOptions.Immediate);
            }
        }

        public void Play(string a, float v, float pI, float pA, bool keep)
        {
            Cue sound = theSounds.GetCue(a);
            sound.SetVariable("Volume", v);
            sound.SetVariable("Pitch", pI);
            sound.Play();

            if (keep)
            {
                keptSounds.Add(sound);
            }
        }

        public static float RandomVolume(ManagerHelper mH)
        {
            return (float) (mH.GetRandom().NextDouble()/2) + 0.5f;
        }

        public static float RandomPitch(ManagerHelper mH)
        {
            return (float) (mH.GetRandom().NextDouble()/4) - 0.125f;
        }

        public bool IsPlaying(string sound)
        {
            foreach (Cue cue in keptSounds)
            {
                if (cue.Name == sound)
                {
                    return true;
                }
            }

            return false;
        }
    }
}