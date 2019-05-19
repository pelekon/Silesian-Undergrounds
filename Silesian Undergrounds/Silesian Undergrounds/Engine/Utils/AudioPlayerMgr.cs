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
        private Song backgroundSong = null;
        // for "optimization" reasons
        private const int MaxSoundTriggersPerFrame = 15;
        Queue<PlayInfo> soundEffectsQueue = new Queue<PlayInfo>();

        public struct PlayInfo
        {
            public SoundEffect soundEffect;
            public float volume;
            public float pitch;
            public float pan;
        }

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
        //"Music\background-game\background_game"
        public void PlayBackgroundMusic(string name)
        {
            backgroundSong = SoundMgr.Instance.GetSong(name);
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

        public void AddSoundEffect(SoundEffect soundEffect, float volume, float pitch, float pan)
        {
            soundEffectsQueue.Enqueue(new PlayInfo()
            {
                soundEffect = soundEffect,
                volume = volume,
                pitch = pitch,
                pan = pan
            });
        }

        public void Update()
        {
            int playCount = 0;

            while (soundEffectsQueue.Count > 0 && playCount <= MaxSoundTriggersPerFrame)
            {
                var playInfo = soundEffectsQueue.Dequeue();
                playInfo.soundEffect.Play(playInfo.volume, playInfo.pitch, playInfo.pan);
            }
        }
    }
}