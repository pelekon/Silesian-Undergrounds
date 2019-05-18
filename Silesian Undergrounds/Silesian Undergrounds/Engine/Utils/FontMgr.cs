using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Silesian_Undergrounds.Engine.Utils
{
    public sealed class FontMgr
    {
        private static FontMgr instance = null;

        private FontMgr() { fonts = new Dictionary<string, SpriteFont>(); }

        public static FontMgr Instance
        {
            get
            {
                if (instance == null)
                    instance = new FontMgr();

                return instance;
            }
        }

        private ContentManager contentMgr;
        private Dictionary<string, SpriteFont> fonts;

        public void SetCurrentContentMgr(ContentManager mgr) { contentMgr = mgr; }

        public SpriteFont GetFont(string name)
        {
            if (!fonts.ContainsKey(name))
            {
                LoadIfNeeded(name);
                if (!fonts.ContainsKey(name))
                    return null;
            }

            SpriteFont returned;
            if (!fonts.TryGetValue(name, out returned))
                return null;

            var method = returned.GetType().GetMethod("MemberwiseClone", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            return (SpriteFont)method.Invoke(returned, null);
        }

        public void LoadIfNeeded(string name)
        {
            if (fonts.ContainsKey(name))
                return;

            SpriteFont font = contentMgr.Load<SpriteFont>(name);
            fonts.Add(name, font);
        }
    }
}
