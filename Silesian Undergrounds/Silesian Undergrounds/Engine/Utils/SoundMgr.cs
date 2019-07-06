using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Silesian_Undergrounds.Engine.Utils
{
    public sealed class SoundMgr
    {
        private static SoundMgr instance = null;

        private SoundMgr() {
            soundEffects = new Dictionary<string, SoundEffect>();
            songs = new Dictionary<string, Song>();
        }

        public static SoundMgr Instance
        {
            get
            {
                if (instance == null)
                    instance = new SoundMgr();

                return instance;
            }
        }

        private ContentManager contentMgr;
        private Dictionary<string, SoundEffect> soundEffects;
        private Dictionary<string, Song> songs;

        public void SetCurrentContentMgr(ContentManager mgr) { contentMgr = mgr; }

        public Song GetSong(string name)
        {
            if (!songs.ContainsKey(name))
            {
                LoadSongIfNeeded(name);
                if (!songs.ContainsKey(name))
                    return null;
            }

            Song returned;
            if (!songs.TryGetValue(name, out returned))
                return null;

            var method = returned.GetType().GetMethod("MemberwiseClone", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            return (Song)method.Invoke(returned, null);
        }

        //TOOD: refactor this
        public void LoadSongIfNeeded(string name)
        {
            if (songs.ContainsKey(name))
                return;

            Song song = contentMgr.Load<Song>(name);
            songs.Add(name, song);
        }

        //TODO: refactor this
        public void LoadSoundEffectIfNeeded(string name)
        {
            if (soundEffects.ContainsKey(name))
                return;

            SoundEffect soundEffect = contentMgr.Load<SoundEffect>(name);
            soundEffects.Add(name, soundEffect);
        }

        public SoundEffect GetSoundEffect(string name)
        {
            if (!soundEffects.ContainsKey(name))
            {
                LoadSoundEffectIfNeeded(name);
                if (!soundEffects.ContainsKey(name))
                    return null;
            }

            SoundEffect returned;
            if (!soundEffects.TryGetValue(name, out returned))
                return null;

            var method = returned.GetType().GetMethod("MemberwiseClone", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            return (SoundEffect)method.Invoke(returned, null);
        }



    }
}