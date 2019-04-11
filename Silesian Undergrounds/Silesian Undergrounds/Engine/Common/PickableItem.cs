using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Silesian_Undergrounds.Engine;

namespace Silesian_Undergrounds.Engine.Common
{
    public class PickableItem : GameObject
    {
        public Scene.Scene scene;
        protected bool isBuyable { get; set; }

        public PickableItem(Texture2D texture, Vector2 position, Vector2 size, int layer, Scene.Scene scene, bool isBuyable = false) : base(texture, position, size, layer)
        {
            this.scene = scene;
            this.isBuyable = isBuyable;
        }

    }
}
