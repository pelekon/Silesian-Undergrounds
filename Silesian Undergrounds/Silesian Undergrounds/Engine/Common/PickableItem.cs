using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Silesian_Undergrounds.Engine.Player;

namespace Silesian_Undergrounds.Engine.Common
{
    public class PickableItem : Gameobject
    {
        Game1 game;

        public PickableItem(Texture2D texture, Vector2 position, Game1 g)
        {
            this.texture = texture;
            this.position = position;
            game = g;
        }

        public override void NotifyCollision(Gameobject obj)
        {
            if (obj is Player.Player)
                game.ScheduleDeletionOfObject(this);
        }
    }
}
