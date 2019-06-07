using System;
using Silesian_Undergrounds.Engine.Common;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Silesian_Undergrounds.Engine.Scene {
    public class Ground : GameObject {

        public Scene Scene;

        public Ground(Texture2D texture, Vector2 position, Vector2 size, int layer, Scene scene) : base(texture, position, size, layer)
        {
            Scene = scene;
        }

        public void SetScene(Scene s)
        {
            Scene = s;
        }
    }
}
