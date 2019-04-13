using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Silesian_Undergrounds.Engine;
using Silesian_Undergrounds.Engine.Item;
using Silesian_Undergrounds.Engine.Enum;

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
        
        public void SetScene(Scene.Scene s)
        {
            scene = s;
        }

        public override void NotifyCollision(GameObject gameobject) {
            if(isBuyable && (gameobject is Player))
            {
                Player player = ((Player)gameobject);
                player.RemoveMoney(GetItemPrice());

                return ;
            }
            
        }

        private int GetItemPrice()
        {
            if(this is Chest)
            {
                return (int)ItemPricesEnum.Chest;
            } else if(this is Food)
            {
                return (int)ItemPricesEnum.Food;
            } else if(this is Heart)
            {
                return (int)ItemPricesEnum.Heart;
            } else if(this is Key)
            {
                return (int)ItemPricesEnum.Key;
            } else
            {
                throw new System.Exception();
            }
        }

    }
}
