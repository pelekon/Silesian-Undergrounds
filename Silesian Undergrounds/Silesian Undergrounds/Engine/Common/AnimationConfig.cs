using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Silesian_Undergrounds.Engine.Common
{
    public class AnimationConfig
    {
        public AnimationConfig(string name, List<Texture2D> textures, int animDuration, bool repeatable = false, bool useFirstFrameAsTexture = false, bool isPermanent = false)
        {
            this.Name = name;
            this.Textures = textures;
            this.AnimDuration = animDuration;
            this.Repeatable = repeatable;
            this.IsPermanent = isPermanent;
            this.UseFirstFrameAsTexture = useFirstFrameAsTexture;
        }
        public string Name { get; }
        public int AnimDuration { get; }
        public bool Repeatable { get; }
        public bool UseFirstFrameAsTexture { get; }
        public bool IsPermanent { get; }
        public List<Texture2D> Textures { get; }
    }
}
