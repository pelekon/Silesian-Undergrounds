using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Silesian_Undergrounds.Engine.Collisions;

namespace Silesian_Undergrounds.Engine.Common {
    public class SpecialItem : GameObject {

        public Scene.Scene scene;

        private Func<bool> OnPlayerPicked;

        public void SetOnPicked(Func<bool> functionOnPicked)
        {
            OnPlayerPicked += functionOnPicked;
        }

        public SpecialItem(Texture2D texture, Vector2 position, Vector2 size, int layer, Scene.Scene scene) : base(texture, position, size, layer)
        {
            this.scene = scene;
        }

        public override void NotifyCollision(GameObject gameobject, ICollider source, RectCollisionSides collisionSides)
        {
            OnPlayerPicked.Invoke();
        }
    }
}
