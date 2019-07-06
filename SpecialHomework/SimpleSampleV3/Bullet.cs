using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace SimpleSampleV3
{
    public class Bullet : GameObject
    {
        const float speed = 12.0f;

        Character owner;

        int destroyTimer;
        const int timeToLive = 180;

        public Bullet()
        {
            active = false;
        }

        public override void Load(ContentManager content)
        {
            texture = TextureLoader.Load(@"graphics\Weapons\Bomb", content);
            base.Load(content);
        }

        public override void Update(List<GameObject> gameObjects, TiledMap map)
        {
            if (active == false)
                return;

            position += direction * speed;

            CheckCollisions(gameObjects, map);

            destroyTimer--;
            if (destroyTimer <= 0 && active == true)
            {
                Destroy();
            }

            base.Update(gameObjects, map);
        }

        internal void Fire(Character inputOwner, Vector2 inputPosition, Vector2 inputDirection)
        {
            owner = inputOwner;
            position = inputPosition;
            direction = inputDirection;
            active = true;
            destroyTimer = timeToLive;
        }

        private void CheckCollisions(List<GameObject> gameObjects, TiledMap map)
        {
            foreach (var gameObject in gameObjects)
            {
                if (gameObject.active == true && gameObject != owner && gameObject.CheckCollision(BoundingBox) == true)
                {
                    Destroy();
                    gameObject.BulletResponse();
                    return;
                }
            }

            if (map.CheckCollision(BoundingBox) != Rectangle.Empty)
            {
                Destroy();
            }
        }

        private void Destroy()
        {
            active = false;
        }
    }
}
