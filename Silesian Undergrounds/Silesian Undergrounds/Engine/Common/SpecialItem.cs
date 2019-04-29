using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Silesian_Undergrounds.Engine.Common {
    public class SpecialItem : GameObject {

        public Scene.Scene scene;

        public SpecialItem(Texture2D texture, Vector2 position, Vector2 size, int layer, Scene.Scene scene) : base(texture, position, size, layer)
        {
            this.scene = scene;
        }

        public override void NotifyCollision(GameObject obj)
        {
            base.NotifyCollision(obj);
        }
    }
}
