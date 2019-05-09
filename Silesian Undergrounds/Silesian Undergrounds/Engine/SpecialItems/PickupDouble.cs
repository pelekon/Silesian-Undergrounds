using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Silesian_Undergrounds.Engine.Common;
using Silesian_Undergrounds.Engine.Scene;
using Silesian_Undergrounds.Engine.Collisions;


namespace Silesian_Undergrounds.Engine.SpecialItems {
    class PickupDouble : SpecialItem {

        public PickupDouble(Texture2D texture, Vector2 position, Vector2 size, int layer, Scene.Scene scene) : base(texture, position, size, layer, scene)
        {
            BoxCollider collider = new BoxCollider(this, 20, 20, 0, 0, true);
            AddComponent(collider);
        }

        public override void NotifyCollision(GameObject obj, ICollider source)
        {
            base.NotifyCollision(obj, source);

            if (obj is Player)
            {
                Player pl = (Player)obj;
                pl.GrandPickupDouble();
                System.Diagnostics.Debug.WriteLine("CYK");
                this.scene.DeleteObject(this);

            }
        }
    }
}
