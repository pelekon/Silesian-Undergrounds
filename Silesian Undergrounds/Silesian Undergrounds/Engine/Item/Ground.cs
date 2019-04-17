using System;
using Silesian_Undergrounds.Engine.Common;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Silesian_Undergrounds.Engine.Collisions;

namespace Silesian_Undergrounds.Engine.Item {
    public class Ground : GameObject {

        public Scene.Scene Scene;

        public Ground(Texture2D texture, Vector2 position, Vector2 size, int layer, Scene.Scene scene) : base(texture, position, size, layer)
        {
            Scene = scene;
        }

        public void SetScene(Scene.Scene s)
        {
            Scene = s;
        }
    }
}
