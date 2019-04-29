using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Silesian_Undergrounds.Engine.Utils
{
    public sealed class TextureMgr
    {
        private static TextureMgr instance = null;

        private TextureMgr() { textures = new Dictionary<string, Texture2D>(); }

        public static TextureMgr Instance
        {
            get
            {
                if (instance == null)
                    instance = new TextureMgr();

                return instance;
            }
        }

        private ContentManager contentMgr;
        private Dictionary<string, Texture2D> textures;

        public void SetCurrentContentMgr(ContentManager mgr) { contentMgr = mgr; }

        public Texture2D GetTexture(string name)
        {
            if (!textures.ContainsKey(name))
            {
                LoadIfNeeded(name);
                if (!textures.ContainsKey(name))
                    return null;
            }
            
            return textures[name];
        }

        public void LoadIfNeeded(string name)
        {
            if (textures.ContainsKey(name))
                return;

            Texture2D texture = contentMgr.Load<Texture2D>(name);
            textures.Add(name, texture);
        }

        public void LoadIfNeeded(Dictionary<int, string>.ValueCollection list)
        {
            foreach (var txt in list)
                LoadIfNeeded(txt);
        }
    }
}
