using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Silesian_Undergrounds.Engine.Player;

namespace Silesian_Undergrounds.Engine.Common
{
    class PickableItem : Gameobject
    {
        Game1 game;

        public PickableItem(Game1 g)
        {

        }

        public override void NotifyCollision(Gameobject obj)
        {
            if (obj is Player.Player)
                game.ScheduleDeletionOfObject(this);
        }
    }
}
