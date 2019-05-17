using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Silesian_Undergrounds.Engine.Common;

namespace Silesian_Undergrounds.Engine.Components
{
    internal struct AnimationData
    {
        internal int lenght;
        internal List<Texture2D> textures;
    }

    public class Animator : IComponent
    {
        // IComponent inherited
        public Vector2 Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Rectangle Rect { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public GameObject Parent { get; private set; }
        // Animator variables
        private Dictionary<string, AnimationData> animations;

        public Animator(GameObject parent)
        {
            animations = new Dictionary<string, AnimationData>();

            Parent = parent;
        }

        public void CleanUp()
        {
            Parent = null;
        }

        public void RegisterSelf() { }
        public void UnRegisterSelf() { }

        public void Update(GameTime gameTime)
        {
            
        }

        public void Draw(SpriteBatch batch) { }

        public void AddAnimation(string name, List<Texture2D> textures, int lenght)
        {
            if (animations.ContainsKey(name))
                return;

            AnimationData animData = new AnimationData();
            animData.lenght = lenght;
            animData.textures = textures;
            animations.Add(name, animData);
        }
    }
}
