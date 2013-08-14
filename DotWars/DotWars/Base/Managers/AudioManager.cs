using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace DotWars
{
    public class AudioManager
    {
        #region Declarations

        private AudioEngine theAudio;
        private WaveBank theWaves;
        private SoundBank theSounds;

        private List<Cue> keptSounds;

        #endregion

        #region Location Constants

        public const string AUDIO = "Audio/";
        public const string MUSIC = "Music/";

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
            return (float)(mH.GetRandom().NextDouble()/2) + 0.5f;
        }

        public static float RandomPitch(ManagerHelper mH)
        {
            return (float) (mH.GetRandom().NextDouble()/4) - 0.125f;
        }
    }
}