using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Silesian_Undergrounds.Engine.Common;

namespace Silesian_Undergrounds.Engine.Scene
{
    public class Camera
    {
        private readonly Player _player;
        public Vector2 position { get; private set; }

        public Camera(Player player)
        {
            _player = player;
            position = new Vector2(_player.position.X, _player.position.Y);
        }

        public void Update(GameTime gameTime)
        {
            if (position != _player.position)
            {

            }
        }
    }
}
