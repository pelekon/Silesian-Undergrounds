using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Silesian_Undergrounds.Engine.Utils;

using Silesian_Undergrounds.Engine.Common;

namespace Silesian_Undergrounds.Engine.Scene
{
    public class Camera
    {
        private readonly Player _player;

        public Matrix Transform { get; private set; }

        public Camera(Player player)
        {
            _player = player;
            Transform = CalculateCameraPosition();
        }

        public void Update(GameTime gameTime)
        {
            Transform = CalculateCameraPosition();
        }

        private Matrix CalculateCameraPosition()
        {
            if (_player != null)
            {
                Matrix playerPosition = Matrix.CreateTranslation(-_player.position.X - (_player.Rectangle.Width / 2), -_player.position.Y - (_player.Rectangle.Height / 2), 0);
                Matrix screenCenterTransform = Matrix.CreateTranslation(ResolutionMgr.GameWidth / 2, ResolutionMgr.GameHeight / 2, 0);
                return playerPosition * screenCenterTransform;
            }

            return Matrix.CreateTranslation(0, 0, 0);
        }
    }
}
