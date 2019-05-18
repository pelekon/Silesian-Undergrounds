using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Silesian_Undergrounds.Engine.Common;
using Silesian_Undergrounds.Engine.Collisions;

namespace Silesian_Undergrounds.Engine.Item.Specials {
    class AttackBooster : SpecialItem {

        private const float PLAYER_ATTACK_INCREASE_BY = 1.0f;

        public AttackBooster(Texture2D texture, Vector2 position, Vector2 size, int layer, Scene.Scene scene) : base(texture, position, size, layer, scene)
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
                pl.IncreaseAttackValueBy(PLAYER_ATTACK_INCREASE_BY);
                this.scene.DeleteObject(this);
            }
        }
    }
}
