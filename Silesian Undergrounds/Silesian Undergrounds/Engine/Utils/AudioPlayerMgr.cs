using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Silesian_Undergrounds.Engine.Utils
{
    public sealed class AudioPlayerMgr
    {
        #region SOUND_CONSTANTS
        //TODO: add this in settings
        public const float BACKGROUND_MUSIC_VOLUME = 0.5f;
        public const float SOUND_EFFECT_VOLUME = 1.0f;
        #endregion
        private Song backgroundSong = null;
        // for "optimization" reasons
        private const int MaxSoundTriggersPerFrame = 15;
        Queue<SoundEffect> soundEffectsQueue = new Queue<SoundEffect>();
        private static AudioPlayerMgr instance = null;

        private AudioPlayerMgr() { }

        public static AudioPlayerMgr Instance
        {
            get
            {
                if (instance == null)
                    instance = new AudioPlayerMgr();

                return instance;
            }
        }

        // background music 
        public void PlayBackgroundMusic(string name)
        {
            backgroundSong = SoundMgr.Instance.GetSong(name);
            MediaPlayer.Volume = BACKGROUND_MUSIC_VOLUME;
            MediaPlayer.Play(backgroundSong);
            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
        }

        private void MediaPlayer_MediaStateChanged(object sender, System.EventArgs e)
        {
            // 0.0f is silent, 1.0f is full volume
            MediaPlayer.Play(backgroundSong);
        }

        public void StopBackgroundMusic()
        {
            if (backgroundSong != null)
            {
                backgroundSong = null;
                MediaPlayer.Stop();
            } 
        }

        public void AddSoundEffect(string soundEffectName)
        {
            SoundEffect soundEffect = SoundMgr.Instance.GetSoundEffect(soundEffectName);

            soundEffectsQueue.Enqueue(soundEffect);
        }

        public void Update()
        {
            int playCount = 0;

            while (soundEffectsQueue.Count > 0 && playCount <= MaxSoundTriggersPerFrame)
            {
                var soundEffect = soundEffectsQueue.Dequeue();
                soundEffect.Play(SOUND_EFFECT_VOLUME, 0.0f, 0.0f);
            }
        }
    }
}