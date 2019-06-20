using Silesian_Undergrounds.Engine.Common;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Silesian_Undergrounds.Engine.Collisions;
using Silesian_Undergrounds.Engine.Enum;

namespace Silesian_Undergrounds.Engine.Item {
    public class Food : PickableItem {

        public int hungerRefil { get; private set; }

        public FoodEnum type;

        public Food(Texture2D texture, Vector2 position, Vector2 size, int layer, Scene.Scene scene, FoodEnum foodEnum, bool isBuyable = false) : base(texture, position, size, layer, scene, isBuyable)
        {
            this.type = foodEnum;

            if (this.type == FoodEnum.Meat)
                hungerRefil = 20;
            else
                hungerRefil = 30;

            BoxCollider collider = new BoxCollider(this, 20, 20, 0, 0, true);
            AddComponent(collider);
        }

        public override void NotifyCollision(GameObject obj, ICollider source, RectCollisionSides collisionSides)
        {
            base.NotifyCollision(obj, source, collisionSides);

            if (obj is Player && !isBuyable)
            {
                Player pl = (Player)obj;
                if (pl.MaxHungerValue > pl.HungerValue)
                {
                    if (pl.PlayerStatistic.PickupDouble)
                    {
                        pl.RefilHunger(this.hungerRefil * 2);
                    }
                    else
                    {
                        pl.RefilHunger(this.hungerRefil);
                    }
                    this.scene.DeleteObject(this);
                }
            }
        }
    }
}
