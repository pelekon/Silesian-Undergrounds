using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Silesian_Undergrounds.Engine.Player;

namespace Silesian_Undergrounds.Engine.Common
{
    public class PickableItem : Gameobject
    {
        Game1 game;

        public PickableItem(Texture2D texture, Vector2 position, Vector2 size, Game1 g) : base(texture, position, size)
        {
            game = g;
        }

        public override void NotifyCollision(Gameobject obj)
        {
            if (obj is Player.Player)
                game.ScheduleDeletionOfObject(this);
        }
    }
}
